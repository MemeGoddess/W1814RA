using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using System.Reflection;

namespace W1814_Remedial_Action
{
    /// <summary>
    /// comp properties are injected into ranged weapons 
    /// </summary>
    [StaticConstructorOnStartup]
    public static class ShotCounterInit
    {
        static ShotCounterInit()
        {
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (def.IsRangedWeapon)
                    def.comps.Add(new W1814RA_CompProperties());
            }
            new Harmony("W1814_Remedial_Action").PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    /// <summary>
    /// increment added stats once fired
    /// </summary>
    [HarmonyPatch(typeof(Verb_LaunchProjectile), "TryCastShot")]

    public static class Patch_Verb_LaunchProjectile
    {
        public static bool Prefix(Verb_LaunchProjectile __instance)
        {
            ThingWithComps source = __instance.EquipmentSource;
            CompShotCounter comp = source?.TryGetComp<CompShotCounter>();
            W1814RA_ModExtension ext = source?.def.GetModExtension<W1814RA_ModExtension>();

            if (comp == null || ext == null) return true;
            if (comp.fault != CompShotCounter.WeaponFault.NoFault) return false;
            
            float grimePercent = ext.maxGrime > 0 ? (float)comp.weaponGrime / ext.maxGrime * 100f : 0f;
            float wearPercent = ext.maxServiceLife > 0 ? (float)comp.weaponWear / ext.maxServiceLife * 100f : 0f;
            float FailMin = 0.0001f;
            float FailTotal = (grimePercent*2) + wearPercent;

            float grimeFail = 100f * (float)Math.Pow(.90f, (100f - grimePercent));
            float wearFail = 100f * (float)Math.Pow(.94f, (100f - wearPercent));
            float FailChance = (grimeFail+ wearFail);
                        
            if (!Rand.Chance(FailChance/100f)) return true;

            bool stoppage = Rand.Chance((grimePercent * 2) / (FailTotal + FailMin));
            comp.fault = stoppage
                ? CompShotCounter.WeaponFault.Stoppage
                : CompShotCounter.WeaponFault.Malfunction;

            string key = stoppage ? "W1814_RA_WeaponStoppage" : "W1814_RA_WeaponMalfunction";
            Messages.Message(key.Translate(source.def.label.ToString()), MessageTypeDefOf.RejectInput, false);
            return false;
         
        }

        /*
         * 
         * original method, very linear

        public static bool Prefix(Verb_LaunchProjectile __instance)
        {
            ThingWithComps source = __instance.EquipmentSource;
            if (source == null) return true;

            CompShotCounter comp = __instance.EquipmentSource?.TryGetComp<CompShotCounter>();
            W1814RA_ModExtension ext = __instance.EquipmentSource.def.GetModExtension<W1814RA_ModExtension>();

            if (comp == null || ext == null) return true;
            {
                if (comp.weaponWear >= ext.maxServiceLife)
                {
                    Messages.Message("W1814_RA.WeaponMalfunction".Translate(source.def.label.ToString()), MessageTypeDefOf.RejectInput, false);
                    return false;
                }

                if (comp.weaponGrime >= ext.maxGrime)
                {
                    Messages.Message("W1814_RA.WeaponStoppage".Translate(source.def.label.ToString()), MessageTypeDefOf.RejectInput, false);
                    return false;
                }
                return true;
            }
        }

        */

        public static void Postfix(Verb_LaunchProjectile __instance, bool __result)
        {
            if (!__result) return;

            CompShotCounter comp = __instance.EquipmentSource?.TryGetComp<CompShotCounter>();
            if (comp != null)
            {
                comp.weaponWear++;
                comp.weaponGrime++;
            }
        }
    }

    /// <summary>
    /// will return gizmos for all selected pawns of player's faction 
    /// </summary>
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
    public static class Patch_Pawn_GetGizmos
    {
        public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Pawn __instance)
        {
            foreach (Gizmo shotsFiredGizmo in __result)
                yield return shotsFiredGizmo;

            if (__instance.Faction != Faction.OfPlayer) yield break;

            if (!DebugSettings.godMode) yield break;

            CompShotCounter comp = __instance.equipment?.Primary?.TryGetComp<CompShotCounter>();
            if (comp != null)
                yield return new W1814RA_Gizmo(comp);
        }
    }
}
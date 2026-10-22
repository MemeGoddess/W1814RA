using RimWorld.Planet;
using Verse;

namespace W1814_Remedial_Action
{
    public class W1814RA_CompProperties : CompProperties
    {
        public W1814RA_CompProperties()
        {
            compClass = typeof(CompShotCounter);
        }
    }
    public class CompShotCounter : ThingComp
    {
        public enum WeaponFault
        {
            NoFault,
            Stoppage,
            Malfunction
        }
        public int weaponWear;
        public int weaponGrime;
        public WeaponFault fault = WeaponFault.NoFault;
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref weaponWear, "W1814_weaponWear", 0);
            Scribe_Values.Look(ref weaponGrime, "W1814_weaponGrime", 0);
            Scribe_Values.Look(ref fault, "W1814_fault", WeaponFault.NoFault);
        }
    }
}
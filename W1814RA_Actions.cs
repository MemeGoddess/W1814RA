using UnityEngine;


namespace W1814_Remedial_Action
{
    public enum MaintNeeded
    {
        None,
        Clean,
        Repair
    }
    public static class W1814RA_Actions
    {
        public static MaintNeeded GetMaintNeeded(CompShotCounter comp, W1814RA_ModExtension ext)
        {
            if (comp == null || ext == null) return MaintNeeded.None;

            float grimePercent = ext.maxGrime > 0 ? (float)comp.weaponGrime / ext.maxGrime * 100f : 0f;
            float wearPercent = ext.maxServiceLife > 0 ? (float)comp.weaponWear / ext.maxServiceLife * 100f : 0f;

            if (comp.fault == CompShotCounter.WeaponFault.Malfunction)
                return ext.W1814_Repairable ? MaintNeeded.Repair : MaintNeeded.None

/*
 * set checks for if repairable or not, if not stop, if true look for parts
 * 
 * 
 */
           }



        public static void ClearStoppage(CompShotCounter comp)
        {
            if (comp.fault == CompShotCounter.WeaponFault.Stoppage)
                comp.fault = CompShotCounter.WeaponFault.NoFault;
        }
        public static void WeaponCleaning(CompShotCounter comp)
        {
            comp.weaponGrime = Mathf.Max(0, comp.weaponGrime - 5);
        }
        public static void WeaponCleanAndRepair(CompShotCounter comp)
        {
            if (comp.fault == CompShotCounter.WeaponFault.Malfunction)
                comp.fault = CompShotCounter.WeaponFault.NoFault;
            comp.weaponWear = 0;
            comp.weaponGrime = 0;
        }
    }
}

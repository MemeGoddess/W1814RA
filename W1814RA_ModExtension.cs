using RimWorld;
using Verse;

namespace W1814_Remedial_Action
{
    public class W1814RA_ModExtension : DefModExtension
    {
        // all the shit I wanted added to the XML patch side 

        public int maxGrime = 100;//how many rounds before the weapon is considered too dirty to continue to fire
        public int maxServiceLife = 2500;//how many rounds expected for life expectancy of weapon

        public bool W1814_Repairable = true; //set false for single use weapons that should not be repairable or reusable, 
        public int repairTicks = 900; //number of ticks to fully repair the weapon

        public int cleanTicks = 100; //number of ticks to fully clean the weapon
        public int cleanIncrements = 20; //number of steps to fully clean the weapon. this number should be less than or equal to cleanTicks
        public int patchesPerClean = 15; //number of CleaningPatches will be needed for one step of cleaning 

        public float cleanAtGrimePercent = 50f; //at what percent of grime should a pawn look to clean their weapon
        public float repairAtWearPercent = 50f; //at what percent of wear should a pawn attempt to repair their weapon




    }
}
/* 

<Patch>

...

    <Operation Class="PatchOperationAddModExtension">
    <xpath>Defs/ThingDef[defName="Gun_AssaultRifle"]</xpath>
        <value>
            <li Class="W1814_Remedial_Action.W1814RA_ModExtension">
                <maxGrime>10</maxGrime>
                <maxServiceLife>50</maxServiceLife>
W1814_Repairable
repairTicks
                <cleanTicks>100</cleanTicks>
cleanIncrements
atchesPerClean
cleanAtGrimePercent
repairAtWearPercent


            </li>
        </value>
    </Operation>

...

</Patch>

 */
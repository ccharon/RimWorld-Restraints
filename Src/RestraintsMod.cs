using HugsLib;
using HugsLib.Settings;
using RimWorld;
using Verse;

namespace Restraints
{
    public class RestraintsMod : ModBase
    {
        internal static HediffDef RestraintsHediff;
        internal static JobDef RestrainJob, FreeJob;
        internal static ThoughtDef RestrainsMemory;
        
        public override string ModIdentifier => "Restraints";
        public override void DefsLoaded()
        {
            RestraintsHediff = HediffDef.Named("bdew_restraints");
            RestrainJob = DefDatabase<JobDef>.GetNamed("bdew_restraints_add");
            FreeJob = DefDatabase<JobDef>.GetNamed("bdew_restraints_free");
            RestrainsMemory = ThoughtDef.Named("bdew_restraints_memory");
        }
    }
}
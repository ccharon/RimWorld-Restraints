using HarmonyLib;
using RimWorld;
using Verse;

namespace Restraints
{
    public class RestraintsMod : Mod
    {
        public RestraintsMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("Nukulartechniker.Restraints");
            harmony.PatchAll();
            Log.Message("Restraints loaded");
        }

        internal static JobDef RestrainJob => DefDatabase<JobDef>.GetNamed("nukulartechniker_restraints_add");
        internal static JobDef FreeJob => DefDatabase<JobDef>.GetNamed("nukulartechniker_restraints_free");
        
        // usually people don't want to be restrained
        internal static HediffDef RestraintsHediff => HediffDef.Named("nukulartechniker_restraints");
        internal static ThoughtDef RestrainsMemory => ThoughtDef.Named("nukulartechniker_restraints_memory");

        // on the other hand some might like it
        internal static HediffDef RestraintsMasochistHediff => HediffDef.Named("nukulartechniker_masochist_restraints");
        internal static ThoughtDef RestrainsMasochistMemory => ThoughtDef.Named("nukulartechniker_masochist_restraints_memory");
    }
}
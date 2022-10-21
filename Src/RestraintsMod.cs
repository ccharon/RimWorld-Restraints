﻿using RimWorld;
using Verse;

namespace Restraints
{
    public class RestraintsMod : Mod
    {
        public RestraintsMod(ModContentPack content) : base(content)
        {

        }

        internal static HediffDef RestraintsHediff => HediffDef.Named("bdew_restraints");
        internal static JobDef RestrainJob => DefDatabase<JobDef>.GetNamed("bdew_restraints_add");
        internal static JobDef FreeJob => DefDatabase<JobDef>.GetNamed("bdew_restraints_free");
        internal static ThoughtDef RestrainsMemory => ThoughtDef.Named("bdew_restraints_memory");
    }
}
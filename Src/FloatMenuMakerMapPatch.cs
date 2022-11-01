using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Restraints
{
    // ReSharper disable once UnusedMember.Global
    [HarmonyPatch(typeof(FloatMenuMakerMap), nameof(FloatMenuMakerMap.ChoicesAtFor))]
    public class FloatMenuMakerMapPatch
    {
        public static void Postfix(List<FloatMenuOption> __result, Vector3 clickPos, Pawn pawn)
        {
            var pawnIsNotAbleToRestrainOthers = !clickPos.InBounds(pawn.Map)
                                        || !pawn.IsColonistPlayerControlled 
                                        || pawn.Map != Find.CurrentMap
                                        || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);

            if (pawnIsNotAbleToRestrainOthers) return;

            foreach (var dest in GenUI.TargetsAt(clickPos, Utils.RestrainTarget(pawn), true))
            {
                if (!dest.HasThing) continue;
                if (!(dest.Thing is Pawn target)) continue;
                if (!pawn.CanReach(dest, PathEndMode.ClosestTouch, Danger.Deadly)) continue;

                __result.Add(target.health.hediffSet.hediffs.Exists(h => h.def == RestraintsMod.RestraintsHediff || h.def == RestraintsMod.RestraintsMasochistHediff)
                    ? RemoveRestraint(pawn, target)
                    : AddRestraint(pawn, target));
            }
        }
        
        private static FloatMenuOption AddRestraint(Pawn pawn, Pawn target)
        {
            return new FloatMenuOption("Restraints.TryToRestrain".Translate(target), () =>
            {
                var steel = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map,
                    ThingRequest.ForDef(ThingDefOf.Steel), PathEndMode.OnCell, TraverseParms.For(pawn));

                if (steel != null)
                    pawn.jobs.TryTakeOrderedJob(new Job(RestraintsMod.RestrainJob, target, steel));
                else
                    Messages.Message("Restraints.NeedSteel".Translate(), pawn, MessageTypeDefOf.RejectInput, false);
            });
        }
        
        private static FloatMenuOption RemoveRestraint(Pawn pawn, Pawn target)
        {
            return new FloatMenuOption("Restraints.Remove".Translate(target),
                () => { pawn.jobs.TryTakeOrderedJob(new Job(RestraintsMod.FreeJob, target)); });
        }
    }
}
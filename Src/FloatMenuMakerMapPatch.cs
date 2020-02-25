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
        // ReSharper disable once InconsistentNaming
        public static void Postfix(List<FloatMenuOption> __result, Vector3 clickPos, Pawn pawn)
        {
            if (!clickPos.InBounds(pawn.Map) || !pawn.IsColonistPlayerControlled || pawn.Map != Find.CurrentMap || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
                return;
            foreach (LocalTargetInfo dest in GenUI.TargetsAt(clickPos, Utils.RestrainTarget(pawn), true))
            {
                if (dest.HasThing && dest.Thing is Pawn target)
                {
                    if (pawn.CanReach(dest, PathEndMode.ClosestTouch, Danger.Deadly))
                    {
                        if (target.health.hediffSet.hediffs.Exists(h => h.def == RestraintsMod.RestraintsHediff))
                        {
                            __result.Add(new FloatMenuOption("Restraints.Remove".Translate(target), () => { pawn.jobs.TryTakeOrderedJob(new Job(RestraintsMod.FreeJob, target)); }));
                        }
                        else
                        {
                            __result.Add(new FloatMenuOption("Restraints.TryToRestrain".Translate(target), () =>
                            {
                                Thing steel = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Steel), PathEndMode.OnCell, TraverseParms.For(pawn));
                                if (steel != null)
                                {
                                    pawn.jobs.TryTakeOrderedJob(new Job(RestraintsMod.RestrainJob, target, steel));
                                }
                                else
                                {
                                    Messages.Message("Restraints.NeedSteel".Translate(), pawn, MessageTypeDefOf.RejectInput, false);
                                }
                            }));
                        }
                    }
                }
            }
        }
    }
}
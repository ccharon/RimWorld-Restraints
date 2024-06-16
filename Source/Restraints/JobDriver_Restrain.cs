﻿using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Restraints
{
    public class JobDriver_Restrain : JobDriver
    {
        private const TargetIndex PawnInd = TargetIndex.A;
        private const TargetIndex SteelInd = TargetIndex.B;

        private Pawn Target => job.GetTarget(PawnInd).Thing as Pawn;

        private Thing Steel => job.GetTarget(SteelInd).Thing;


        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed)
                   && pawn.Reserve(Steel, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_General.DoAtomic(() => job.count = 1);

            yield return Toils_Reserve.Reserve(SteelInd);

            yield return Toils_Goto.GotoThing(SteelInd, PathEndMode.ClosestTouch)
                .FailOnDespawnedNullOrForbidden(SteelInd)
                .FailOnSomeonePhysicallyInteracting(SteelInd)
                .FailOn(toil => !pawn.CanReach(Steel, PathEndMode.ClosestTouch, Danger.Deadly));

            yield return Toils_Haul.StartCarryThing(SteelInd, false, true, true)
                .FailOnDestroyedNullOrForbidden(SteelInd);

            var gotoToil = Toils_Goto.GotoThing(PawnInd, PathEndMode.ClosestTouch)
                .FailOnDespawnedNullOrForbidden(PawnInd)
                .FailOnSomeonePhysicallyInteracting(PawnInd)
                .FailOn(toil => !pawn.CanReach(Target, PathEndMode.ClosestTouch, Danger.Deadly));

            yield return gotoToil;

            yield return Toils_General.Wait(50)
                .WithProgressBarToilDelay(PawnInd)
                .PlaySustainerOrSound(SoundDefOf.TrapArm)
                .JumpIf(() => !pawn.CanReachImmediate(Target, PathEndMode.ClosestTouch), gotoToil);

            yield return new Toil
            {
                initAction = () =>
                {
                    Target.health.AddHediff(Target.story.traits.HasTrait(TraitDef.Named("Masochist"))
                        ? RestraintsMod.RestraintsMasochistHediff
                        : RestraintsMod.RestraintsHediff);

                    Steel.Destroy();
                }
            };
        }
    }
}
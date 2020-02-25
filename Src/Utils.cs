using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace Restraints
{
    public class Utils
    {
        internal static TargetingParameters RestrainTarget(Pawn performer)
        {
            return new TargetingParameters()
            {
                canTargetPawns = true,
                canTargetBuildings = false,
                mapObjectTargetsMustBeAutoAttackable = false,
                validator = target =>
                {
                    if (!target.HasThing)
                        return false;
                    return target.Thing is Pawn targetPawn && targetPawn != performer && (targetPawn.IsColonistPlayerControlled || targetPawn.IsPrisonerOfColony);
                }
            };
        }
    }
}
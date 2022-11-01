using RimWorld;
using Verse;

namespace Restraints
{
    public static class Utils
    {
        internal static TargetingParameters RestrainTarget(Pawn performer)
        {
            return new TargetingParameters
            {
                canTargetPawns = true,
                canTargetBuildings = false,
                mapObjectTargetsMustBeAutoAttackable = false,
                validator = target => target.HasThing 
                                      && target.Thing is Pawn targetPawn 
                                      && targetPawn != performer 
                                      && (targetPawn.IsColonistPlayerControlled || targetPawn.IsPrisonerOfColony || targetPawn.IsSlaveOfColony)
            };
        }
    }
}
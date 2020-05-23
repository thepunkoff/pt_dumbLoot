using System.Collections;
using System.Linq;

public class PunchProgram : BotProgram
{
    public override void Init(Unit unit)
    {
        base.Init(unit);
    }
    public override IEnumerator Step(BattleContext context)
    {
        var cowardState = unit.GetComponent<AIBot>().programs.Where(x => x.Program is CowardProgram c).FirstOrDefault().Program as CowardProgram;
        if (cowardState != null && cowardState.isFrightened)
        {
            yield break;
        }

        var (target, distance, _) = BattleHelper.FindClosestHumanUnit(unit, context);

        if (distance <= Punch.Range)
        {
            var punch = (Punch)context.CurrentActions.Where(x => x is Punch).FirstOrDefault();

            punch.SetTarget(target);

            yield return context.StateMachine.StartCoroutine(punch.Execute());
        }
    }
}

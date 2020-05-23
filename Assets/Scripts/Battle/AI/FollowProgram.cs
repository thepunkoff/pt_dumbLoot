using System.Collections;
using System.Linq;

public class FollowProgram : BotProgram
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

        var (_, distance, direction) = BattleHelper.FindClosestHumanUnit(unit, context);

        if (distance > Punch.Range)
        {
            var moveLeft = context.CurrentActions.Where(x => x is MoveLeft).FirstOrDefault();
            var moveRight = context.CurrentActions.Where(x => x is MoveRight).FirstOrDefault();

            if (direction > 0)
                yield return context.StateMachine.StartCoroutine(moveRight.Execute());
            else
                yield return context.StateMachine.StartCoroutine(moveLeft.Execute());
        }
    }
}
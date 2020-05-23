using System.Collections;
using System.Linq;

public class NaivePuncherProgram : BotProgram
{
    public override void Init(Unit unit)
    {
        base.Init(unit);
    }

    public override IEnumerator Step(BattleContext context)
    {
        var (target, distance, direction) = BattleHelper.FindClosestHumanUnit(unit, context);

        if (distance <= Punch.Range)
        {
            var punch = (Punch)context.CurrentActions.Where(x => x is Punch).FirstOrDefault();

            punch.SetTarget(target);

            yield return context.StateMachine.StartCoroutine(punch.Execute());
        }
        else
        {
            var moveLeft = context.CurrentActions.Where(x => x is MoveLeft).FirstOrDefault();
            var moveRight =  context.CurrentActions.Where(x => x is MoveRight).FirstOrDefault();

            if (direction > 0)
                yield return context.StateMachine.StartCoroutine(moveRight.Execute());
            else
                yield return context.StateMachine.StartCoroutine(moveLeft.Execute());
        }
    }
}
using System.Collections;
using System.Linq;
using UnityEngine;

public class CowardProgram : BotProgram
{
    public bool isFrightened;

    public override void Init(Unit unit)
    {
        base.Init(unit);
    }

    public override IEnumerator Step(BattleContext context)
    {
        if ((float)unit.stats.health / unit.stats.maxHealth <= 0.2f && !isFrightened)
        {
            isFrightened = true;
        }

        if (isFrightened)
        {
            Debug.Log($"{unit.name} tries to run away!");

            var orderedHumans = context.AllUnits.Where(x => !x.isAI).OrderBy(x => x.transform.position.x);
            var mostLeftHuman = orderedHumans.First();
            var mostRightHuman = orderedHumans.Last();
            var unitPosition = unit.transform.position.x;
            var moveLeft = context.CurrentActions.Where(x => x is MoveLeft).FirstOrDefault();
            var moveRight = context.CurrentActions.Where(x => x is MoveRight).FirstOrDefault();

            if (mostRightHuman.transform.position.x <= unitPosition)
            {
                Debug.Log("Go -> coward");
                yield return context.StateMachine.StartCoroutine(moveRight.Execute());
            }
            else if (mostLeftHuman.transform.position.x >= unitPosition)
            {
                Debug.Log("Go <- coward");
                yield return context.StateMachine.StartCoroutine(moveLeft.Execute());
            }
            else
            {
                var (distanceToMostLeft, _) = BattleHelper.GetDistanceDirection(unit, mostLeftHuman);
                var (distanceToMostRight, _) = BattleHelper.GetDistanceDirection(unit, mostRightHuman);

                // Позже учесть, что в той стороне может быть больше врагов, и принимать решение. Хотя трус тупой.
                if (distanceToMostRight <= distanceToMostLeft)
                    yield return context.StateMachine.StartCoroutine(moveRight.Execute());
                else 
                    yield return context.StateMachine.StartCoroutine(moveLeft.Execute());
            }
        }
    }
}
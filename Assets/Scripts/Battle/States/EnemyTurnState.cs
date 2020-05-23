using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyTurnState : State
{
    private Unit enemy;
    private BattleContext context;
    public EnemyTurnState(Unit enemy, BattleContext context, StateMachine stateMachine) : base(stateMachine)
    {
        if (enemy.GetComponent<AIBot>() == null)
            throw new Exception($"An enemy unit should have {nameof(AIBot)} component attached");

        this.enemy = enemy;
        this.context = context;
    }

    public override IEnumerator Start()
    {
        if (!context.AllUnits.Any(x => !x.isAI))
            yield return EndTurn();

        while (EnoughActionPoints())
        {
            if (!context.AllUnits.Any(x => !x.isAI))
                yield return EndTurn();

            yield return enemy.GetComponent<AIBot>().program.Step(context);
        }

        yield return EndTurn();
    }

    private bool EnoughActionPoints()
    {
        return context.CurrentActions.Any(x => x.actionPoints <= enemy.stats.actionPoints);
    }
}
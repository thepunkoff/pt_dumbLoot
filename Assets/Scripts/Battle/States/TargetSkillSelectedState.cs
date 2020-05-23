using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TargetSkillSelectedState : State
{
    private Action selectedAction;
    private Unit selectedUnit;

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public Action GetSelectedAction()
    {
        return selectedAction;
    }

    public TargetSkillSelectedState(Action action, Unit selectedUnit, StateMachine stateMachine) : base(stateMachine)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        this.selectedAction = action;
        this.selectedUnit = selectedUnit;
    }

    public override IEnumerator Start()
    {
        if (selectedAction is TargetAction ta)
        {
            if (ta.HasTarget())
            {
                yield return StateMachine.StartCoroutine(ta.Execute());
                StateMachine.SetState(new WaitingForActionState(StateMachine));
            }
            else
            {
                if (selectedUnit != null)
                {
                    ta.SetTarget(selectedUnit);
                    StateMachine.StartCoroutine(ta.Execute());
                    StateMachine.SetState(new WaitingForActionState(StateMachine));
                }
                else
                {
                    Debug.Log("Select target.");
                }
            }
        }

        if (selectedAction is SelfAction sa)
        {
            yield return StateMachine.StartCoroutine(sa.Execute());
            StateMachine.SetState(new WaitingForActionState(StateMachine));
        }

        yield break;
    }

    public override IEnumerator SelectAction(Action action)
    {
        if (selectedAction != null && ReferenceEquals(selectedAction, action))
        {
            StateMachine.SetState(new WaitingForActionState(StateMachine));
            yield break;
        }

        StateMachine.SetState(new TargetSkillSelectedState(action, null, StateMachine));
        yield break;
    }

    public override IEnumerator SelectUnit(Unit targetUnit)
    {
        StateMachine.SetState(new TargetUnitSelectedState(selectedAction, targetUnit, StateMachine));
        yield break;
    }
}

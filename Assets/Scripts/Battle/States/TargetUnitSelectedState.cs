using System;
using System.Collections;
using UnityEngine;

public class TargetUnitSelectedState : State
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

    public TargetUnitSelectedState(Action slectedAction, Unit selectedUnit, StateMachine stateMachine) : base(stateMachine)
    {
        if (selectedUnit == null) throw new ArgumentNullException(nameof(selectedUnit));

        this.selectedAction = slectedAction;
        this.selectedUnit = selectedUnit;
    }

    public override IEnumerator Start()
    {
        if (selectedAction != null)
        {
            if (selectedAction is TargetAction ts)
            {
                if (ts.HasTarget()) // self skills
                {
                    yield return StateMachine.StartCoroutine(selectedAction.Execute());
                    StateMachine.SetState(new WaitingForActionState(StateMachine));
                }
                else // simple target skills
                {
                    ts.SetTarget(selectedUnit);
                    yield return StateMachine.StartCoroutine(selectedAction.Execute());
                    StateMachine.SetState(new WaitingForActionState(StateMachine));
                }
            }

            if (selectedAction is SelfAction)
            {
                yield return StateMachine.StartCoroutine(selectedAction.Execute());
            }
        }
        else
        {
            Debug.Log("Select skill.");
        }

        yield break;
    }

    public override IEnumerator SelectUnit(Unit unit)
    {
        if (selectedUnit != null && ReferenceEquals(selectedUnit, unit))
        {
            StateMachine.SetState(new WaitingForActionState(StateMachine));
            yield break;
        }

        StateMachine.SetState(new TargetUnitSelectedState(null, unit, StateMachine));
        yield break;
    }

    public override IEnumerator SelectAction(Action action)
    {
        StateMachine.SetState(new TargetSkillSelectedState(action, selectedUnit, StateMachine));
        yield break;
    }
}

using System.Collections;
using UnityEngine;

public class WaitingForActionState : State
{
    public WaitingForActionState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override IEnumerator Start()
    {
        Debug.Log("Select target or action.");
        yield break;
    }

    public override IEnumerator SelectAction(Action action)
    {
        StateMachine.SetState(new TargetSkillSelectedState(action, null, StateMachine));
        yield break;
    }

    public override IEnumerator SelectUnit(Unit unit)
    {
        StateMachine.SetState(new TargetUnitSelectedState(null, unit, StateMachine));
        yield break;
    }
}

using System.Collections;

public abstract class State
{
    protected StateMachine StateMachine;

    public State(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator SelectUnit(Unit unit)
    {
        yield break;
    }

    public virtual IEnumerator SelectAction(Action action)
    {
        yield break;
    }

    public virtual IEnumerator EndTurn()
    {
        StateMachine.EndTurn();
        yield break;
    }

    public override string ToString()
    {
        return GetType().Name;
    }
}
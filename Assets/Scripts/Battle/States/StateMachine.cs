using System;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State State;
    public event EventHandler turnEnded;
    public event EventHandler<State> preStateStarted;

    public void SetState(State state)
    {
        var was = State?.ToString() ?? null;
        State = state;
        Debug.Log($"State changed from {was ?? "null"} to {State}");
        preStateStarted?.Invoke(this, state);
        StartCoroutine(state.Start());
    }

    public void EndTurn()
    {
        turnEnded?.Invoke(this, new EventArgs());
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;

public class BattleContext
{
    public StateMachine StateMachine { get; private set; }
    public ReadOnlyCollection<Unit> AllUnits { get; private set; }
    public ReadOnlyCollection<Action> CurrentActions { get; private set; }

    public BattleContext(ReadOnlyCollection<Unit> allUnits, ReadOnlyCollection<Action> currentActions, StateMachine stateMachine)
    {
        StateMachine = stateMachine;
        AllUnits = allUnits;
        CurrentActions = currentActions;
    }
}
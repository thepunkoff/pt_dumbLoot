using System.Collections;
using UnityEngine;

public abstract class BotProgram : ScriptableObject
{
    protected Unit unit;
    public virtual void Init(Unit unit)
    {
        this.unit = unit;
    }

    public abstract IEnumerator Step(BattleContext context);
}
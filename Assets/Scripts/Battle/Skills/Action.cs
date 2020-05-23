using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public string actionName;
    public int actionPoints = 20;
    public GameObject UIObject;

    public Unit User;

    protected Action() { }

    protected Action(Unit user)
    {
        User = user; 
    }

    public virtual IEnumerator Execute()
    {
        User.stats.actionPoints -= actionPoints;

        yield break;
    }

    public virtual bool IsAvailableForUser(IEnumerable<Unit> context)
    {
        return User.stats.actionPoints >= actionPoints;
    }

    public abstract Action Clone();
    public abstract void Init(Unit user);
}

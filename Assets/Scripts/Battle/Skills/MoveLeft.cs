using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : SelfAction
{
    private float? distance;

    public override void Init(Unit user)
    {
        distance = -1;
        actionName = "Move Left";
        User = user;
    }

    public override IEnumerator Execute()
    {
        if (!distance.HasValue) throw new Exception("distance was null");
        User.transform.Translate(distance.Value, 0, 0);
        Debug.Log($"{User.name} went {distance}.");
        yield return base.Execute();
    }

    public override bool IsAvailableForUser(IEnumerable<Unit> context)
    {
        return User.stats.health >= 5 && base.IsAvailableForUser(context);
    }

    public override Action Clone()
    {
        var clone = CreateInstance<MoveLeft>();
        clone.distance = distance;
        clone.actionName = actionName;
        clone.actionPoints = actionPoints;
        clone.User = User;

        return clone;
    }
}
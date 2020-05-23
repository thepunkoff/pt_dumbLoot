using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : TargetAction
{
    public const float Range = 1;
    
    public override void Init(Unit user)
    {
        actionName = "Punch";
        User = user;
        Target = null;
        range = Range;
    }

    public override IEnumerator Execute()
    {
        if (TryHit())
        {
            var damage = User.stats.attack - Target.stats.defense;

            Target.stats.health -= damage;

            Debug.Log($"{User.name} punched {Target.name}. {damage} damage!");
        }
        else
        {
            Debug.Log($"{User.name} tried to punch {Target.name}, but missed!");
        }

        yield return base.Execute();
    }

    public override Action Clone()
    {
        var clone = CreateInstance<Punch>();
        clone.actionName = actionName;
        clone.actionPoints = actionPoints;
        clone.range = range;
        clone.User = User;
        clone.Target = null;

        return clone;
    }

    public override bool IsAvailableForUser(IEnumerable<Unit> context)
    {
        return base.IsAvailableForUser(context);
    }

    public override bool IsAvailableWithTarget(Unit target)
    {
        return base.IsAvailableWithTarget(target);
    }

    public override bool TryHit()
    {
        return new System.Random().NextDouble() > 0.3f;
    }
}

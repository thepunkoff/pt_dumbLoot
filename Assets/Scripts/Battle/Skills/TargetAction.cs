using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class TargetAction : Action
{
    protected Unit Target;
    protected float range;

    protected TargetAction()
    {
        range = 100;
    }

    public TargetAction(Unit user) : base(user)
    {
    }

    public TargetAction(Unit user, Unit target) : base(user)
    {
        Target = target;
    }

    public override IEnumerator Execute()
    {
        if (Target.stats.health <= 0)
            Target.Die();
        yield return base.Execute();
    }

    public bool HasTarget()
    {
        return Target != null;
    }

    public void SetTarget(Unit target)
    {
        Target = target;
    }

    public override bool IsAvailableForUser(IEnumerable<Unit> context)
    {
        return context.Any(other => other.isAI != User.isAI &&
               !ReferenceEquals(User, other) &&
               BattleHelper.IsUnitInDistance(User, other, range)) &&
               base.IsAvailableForUser(context);
    }

    public virtual bool IsAvailableWithTarget(Unit target)
    {
        return BattleHelper.IsUnitInDistance(User, target, range);
    }

    public virtual bool TryHit()
    {
        return true;
    }
}

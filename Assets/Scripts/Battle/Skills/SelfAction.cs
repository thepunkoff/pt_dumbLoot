using System.Collections;
using System.Collections.Generic;

public abstract class SelfAction : Action
{
    public override IEnumerator Execute()
    {
        yield return base.Execute();
    }

    public override bool IsAvailableForUser(IEnumerable<Unit> context)
    {
        return base.IsAvailableForUser(context);
    }
}

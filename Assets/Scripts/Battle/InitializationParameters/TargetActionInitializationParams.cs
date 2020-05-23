public class TargetActionInitializationParams : ActionInitializationParams
{
    public Unit target;

    public TargetActionInitializationParams(string name, Unit user, Unit target) : base (name, user)
    {
        this.name = name;
        this.target = target;
    }
}

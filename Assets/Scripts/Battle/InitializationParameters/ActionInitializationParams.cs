public abstract class ActionInitializationParams
{
    public string name;
    public Unit user;

    public ActionInitializationParams(string name, Unit user)
    {
        this.name = name;
        this.user = user;
    }
}
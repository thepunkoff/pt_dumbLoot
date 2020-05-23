public class MoveInitializationParams : SelfActionInitializationParams
{
    public float distance;

    public MoveInitializationParams(float distance, string name, Unit user) : base(name, user)
    {
        this.distance = distance;
    }
}

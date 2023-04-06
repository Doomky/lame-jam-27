namespace Framework.Managers
{
    public interface ITimeManagerClient
    {
        int Priority { get; }
        float GetTimeScale();
    }
}
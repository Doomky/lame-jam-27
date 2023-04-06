namespace OldGame.Managers
{
    public interface ITimeManagerClient
    {
        int Priority { get; }
        float GetTimeScale();
    }
}
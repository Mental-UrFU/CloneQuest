public class LevelProgressTrack : IStarCollectHandler, ILevelSoftResetEndHandler
{
    public int StarCount { get; private set; }
    LevelProgressTrack() { EventBus.Subscribe<IStarCollectHandler>(this); EventBus.Subscribe<ILevelSoftResetEndHandler>(this); }
    ~LevelProgressTrack() { EventBus.Unsubscribe<IStarCollectHandler>(this); EventBus.Unsubscribe<ILevelSoftResetEndHandler>(this); }
    public void OnStarCollected() => StarCount++;
    public void OnSoftResetEnd() => StarCount = 0;
}

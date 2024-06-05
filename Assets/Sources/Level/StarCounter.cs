public class StarCounter : IStarCollectHandler, ILevelSoftResetEndHandler
{
    public int Count { get; private set; } = 0;

    public void OnStarCollected() { Count += 1; }
    public void OnSoftResetEnd() { Count = 0; }

    public StarCounter() { EventBus.Subscribe<IStarCollectHandler>(this); EventBus.Subscribe<ILevelSoftResetEndHandler>(this); }
    ~StarCounter() { EventBus.Unsubscribe<IStarCollectHandler>(this); EventBus.Unsubscribe<ILevelSoftResetEndHandler>(this); }

}

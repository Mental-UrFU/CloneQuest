using UnityEngine;

public class Star : MonoBehaviour, ILevelSoftResetEndHandler
{
    public static int CurrentLevelStarsCount = 0; // TODO REMOVE >:(
    public static int TotalStarCount = 0; // TODO REMOVE >:(

    public void OnSoftResetEnd()
    {
        enabled = true;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EventBus.Invoke<IStarCollected>(x => x.OnStarCollected());
        enabled = false;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
    }

    private void Unsubscribe()
    {
        EventBus.Unsubscribe<ILevelSoftResetEndHandler>(this);
    }
}
 
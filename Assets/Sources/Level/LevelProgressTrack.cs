using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressTrack : MonoBehaviour, IStarCollected, ILevelSoftResetStartHandler
{
    public int StarCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        EventBus.Subscribe<IStarCollected>(this);        
    }

    private void Unsubscribe()
    {
        EventBus.Unsubscribe<IStarCollected>(this);        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStarCollected()
    {
        StarCount++;
    }

    public void OnSoftResetStart(float duration)
    {
        StarCount = 0;
    }
}

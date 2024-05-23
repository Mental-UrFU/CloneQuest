using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public static int CurrentLevelStarsCount = 0;
    public static int TotalStarCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CurrentLevelStarsCount += 1;
        EventBus.Invoke<IStarCollected>(x => x.OnStarCollected());
        Destroy(this.gameObject);
    }
}

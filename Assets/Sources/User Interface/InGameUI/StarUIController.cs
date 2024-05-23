using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarUIController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> stars;

    public void DisplayStarsCount(int count)
    {
        var clampedValue = Math.Clamp(count, 0, 3);
        for(int i = 0; i < clampedValue; i++)
        {
            stars[i].SetActive(true);
        }
    }
}

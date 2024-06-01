using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarcounterSetter : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> stars;

    [SerializeField]
    private GameObject zamok;

    public void DisplayStarsCount(int count)
    {
        var clampedValue = System.Math.Clamp(count, 0, 3);
        for (int i = 0; i < clampedValue; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void DisplayLock()
    {
        gameObject.GetComponent<Image>().color = new Color32(192, 192, 192, 128);
    }
}

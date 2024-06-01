using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCompletionController : MonoBehaviour, ILevelFinishHandler, ILevelLoadHandler, IStarCollected, ILevelSoftResetEndHandler
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private List<GameObject> stars;
    [SerializeField] private GameObject levelCompletionScreen;

    private LevelContext _levelContext;
    private int _gainedStars = 0;


    public void OnLevelFinish()
    {
        if (levelCompletionScreen != null)
        {
            levelCompletionScreen.SetActive(true);

            for (int i = 0; i < Math.Min(stars.Count, _gainedStars); i++)
            {
                stars[i].SetActive(true);
            }
        }
    }

    public void OnLevelLoad(LevelContext levelContext)
    {
        _levelContext = levelContext;
        _levelText.text = $"УРОВЕНЬ {_levelContext.Index + 1}";
        _gainedStars = 0;
    }

    public void OnSoftResetEnd()
    {
        _gainedStars = 0;
    }

    public void OnStarCollected()
    {
        _gainedStars++;
    }

    void Awake()
    {
        Subscribe();
    }

    void OnDestroy()
    {
        UnSubscribe();
    }

    void Subscribe()
    {
        EventBus.Subscribe<ILevelFinishHandler>(this);
        EventBus.Subscribe<ILevelLoadHandler>(this);
        EventBus.Subscribe<IStarCollected>(this);
        EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
    }

    void UnSubscribe()
    {
        EventBus.Subscribe<ILevelFinishHandler>(this);
        EventBus.Subscribe<ILevelLoadHandler>(this);
        EventBus.Subscribe<IStarCollected>(this);
        EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
    }
}

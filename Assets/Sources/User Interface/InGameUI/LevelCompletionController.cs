using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletionController : MonoBehaviour, ILevelFinishHandler, ILevelLoadHandler, IStarCollected, ILevelSoftResetEndHandler
{
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] List<GameObject> stars;
    [SerializeField] GameObject levelCompletionScreen;
    
    private LevelContext _levelContext;
    private int _gainedStars = 0;


    public void OnLevelFinish()
    {
        levelCompletionScreen.SetActive(true);
        for(int i = 0; i < Math.Min(stars.Count, _gainedStars); i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void OnLevelLoad(LevelContext levelContext)
    {
        _levelContext = levelContext;
        _levelText.text = $"спнбемэ {_levelContext.Id}";
    }

    public void OnSoftResetEnd()
    {
        _gainedStars = 0;
    }

    public void OnStarCollected()
    {
        _gainedStars++;
    }
    
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameCanvasEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseScreen;
    [SerializeField]
    private GameObject _levelCompletionScreen;
    [SerializeField]
    private LevelProgressTrack _levelProgressTracker;

    public void OnLevelCompletion()
    {
        if (!_levelCompletionScreen.gameObject.activeInHierarchy)
        {
            _levelCompletionScreen.SetActive(true);            
            PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel] =
                PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel] <
                _levelProgressTracker.StarCount ?
                _levelProgressTracker.StarCount :
                PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel];
            PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel + 1] =
                PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel + 1] ==
                -1 ? 0 :
                PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel + 1];
        }           
    }

    public void OnPausePress()
    {
        _pauseScreen.SetActive(true);
    }
}

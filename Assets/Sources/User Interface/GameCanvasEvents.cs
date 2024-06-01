using TMPro;
using UnityEngine;

public class GameCanvasEvents : MonoBehaviour, ILevelSoftResetEndHandler, ILevelReadyHandler
{
    [SerializeField]
    private GameObject _pauseScreen;
    [SerializeField]
    private GameObject _levelCompletionScreen;
    [SerializeField]
    private LevelProgressTrack _levelProgressTracker;
    [SerializeField]
    private TextMeshProUGUI _clonesTextCount;

    public void OnLevelCompletion()
    {
        if (!_levelCompletionScreen.gameObject.activeInHierarchy)
        {
            //_levelCompletionScreen.SetActive(true);
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
        EventBus.Subscribe<ILevelReadyHandler>(this);
        EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
    }

    void UnSubscribe()
    {
        EventBus.Subscribe<ILevelReadyHandler>(this);
        EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
    }

    public void OnLevelReady()
    {
        _clonesTextCount.text = $"{CloneSystem.CloneCount}/{CloneSystem.MaxClonesCount}";
    }

    public void OnPausePress()
    {
        _pauseScreen.SetActive(true);
    }

    public void OnSoftResetEnd()
    {
        _clonesTextCount.text = $"{CloneSystem.CloneCount}/{CloneSystem.MaxClonesCount}";
    }
}

using TMPro;
using UnityEngine;

public class GameCanvasEvents : MonoBehaviour
{
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _levelCompletionScreen;
    [SerializeField] private LevelProgressTrack _levelProgressTracker;
    [SerializeField] private TextMeshProUGUI _clonesTextCount;

    private CloneSystem _cloneSystem;

    public void Init(CloneSystem cloneSystem)
    {
        _cloneSystem = cloneSystem;
        _cloneSystem.OnUpdate += UpdateCloneCounter;
        UpdateCloneCounter();
    }

    public void OnLevelCompletion()
    {
        if (!_levelCompletionScreen.activeInHierarchy) // TODO >:(
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
    public void UpdateCloneCounter()
    {
        _clonesTextCount.text = $"{_cloneSystem.CloneCount}/{_cloneSystem.MaxClonesCount}";
    }

    public void OnPausePress()
    {
        _pauseScreen.SetActive(true);
    }
}

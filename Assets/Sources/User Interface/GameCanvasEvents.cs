using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private CloneCounter _cloneCounter;

    public void Init(LevelContext levelContext, CloneSystem cloneSystem)
    {
        cloneSystem.OnUpdate += () => _cloneCounter.UpdateCounter(cloneSystem.CloneCount, cloneSystem.MaxClonesCount);
        _cloneCounter.UpdateCounter(cloneSystem.CloneCount, cloneSystem.MaxClonesCount);
    }

    // public void OnLevelCompletion()
    // {
    //     if (!_levelCompletionScreen.activeInHierarchy) // TODO >:(
    //     {
    //         //_levelCompletionScreen.SetActive(true);
    //         PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel] =
    //             PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel] <
    //             _levelProgressTracker.StarCount ?
    //             _levelProgressTracker.StarCount :
    //             PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel];
    //         PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel + 1] =
    //             PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel + 1] ==
    //             -1 ? 0 :
    //             PersistentLevelData.LevelStars[PersistentLevelData.CurrentLevel + 1];
    //     }
    // }

    public void ShowPauseMenu() { _pauseMenu.Show(); }
}

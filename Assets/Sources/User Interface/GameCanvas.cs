using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private CloneCounter _cloneCounter;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private LevelCompleteScreen _levelComplete;
    [SerializeField] private PressAnyButtonScreen _pressAnyButtonScreen;

    public void Init(CloneSystem cloneSystem)
    {
        cloneSystem.OnUpdate += () => _cloneCounter.UpdateCounter(cloneSystem.CloneCount, cloneSystem.MaxClonesCount);
        _cloneCounter.UpdateCounter(cloneSystem.CloneCount, cloneSystem.MaxClonesCount);
    }

    public void ShowLevelCompleteMenu(int levelNumber, int starCount)
    {
        if (_pauseMenu.IsOpened) { _pauseMenu.Hide(); }
        _levelComplete.Show(levelNumber, starCount);
    }
    public void ShowPauseMenu() { _pauseMenu.Show(); }
    public void ShowAnyButtonScreen() { _pressAnyButtonScreen.Show(); }
    public void HideAnyButtonScreen() { _pressAnyButtonScreen.Hide(); }

#if UNITY_EDITOR
    private void OnValidate()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
#endif
}

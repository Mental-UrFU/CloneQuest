using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour, ILevelSoftResetStartHandler, ILevelSoftResetEndHandler, ILevelReadyHandler, ILevelStartHandler
{
    [SerializeField] private GameObject _clonePrefab;

    private CloneSystem _cloneSystem;
    private PlayerActions _input;
    private bool _pause;


    private void Start()
    {
        var playerControls = FindObjectOfType<PlayerControls>();

        _cloneSystem = new CloneSystem(new(playerControls), _clonePrefab, playerControls.transform.position);

        _input = new PlayerActions();
        _input.Game.Clone.started += (ctx) => { _cloneSystem.AddCloneAndRestart(); };
        _input.Game.Restart.started += (ctx) => { ReloadLevel(); };
        _input.Game.Esc.started += (ctx) => { TogglePause(); };
        _input.Any.Any.started += (ctx) => { EventBus.Invoke<ILevelStartHandler>(obj => obj.OnLevelStart()); };
        _input.Game.Enable();
        _input.Any.Enable();

        EventBus.Invoke<ILevelReadyHandler>(obj => obj.OnLevelReady());
    }

    private void TogglePause()
    {
        Time.timeScale = _pause ? 1f : 0f;
        _pause = !_pause;
    }

    public void OnLevelReady()
    {
        _input.Any.Enable();
    }

    public void OnLevelStart()
    {
        _input.Any.Disable();
        _cloneSystem.Start();
    }

    public void OnSoftResetStart(float duration)
    {
        StartCoroutine(WaitForSoftResetEnd());
        IEnumerator WaitForSoftResetEnd()
        {
            yield return new WaitForSeconds(duration);
            EventBus.Invoke<ILevelSoftResetEndHandler>(obj => obj.OnSoftResetEnd());
            EventBus.Invoke<ILevelReadyHandler>(obj => obj.OnLevelReady());
        }
    }

    public void OnSoftResetEnd() { }

    private void ReloadLevel()
    {
        EventBus.Invoke<IBeforeLevelReloadHandler>(obj => obj.OnBeforeLevelReload());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Awake()
    {
        EventBus.Subscribe<ILevelReadyHandler>(this);
        EventBus.Subscribe<ILevelStartHandler>(this);
        EventBus.Subscribe<ILevelSoftResetStartHandler>(this);
        EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ILevelReadyHandler>(this);
        EventBus.Unsubscribe<ILevelStartHandler>(this);
        EventBus.Unsubscribe<ILevelSoftResetStartHandler>(this);
        EventBus.Unsubscribe<ILevelSoftResetEndHandler>(this);
    }
}
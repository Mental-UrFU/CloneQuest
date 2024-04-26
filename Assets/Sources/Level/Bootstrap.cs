using System.Collections;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class Bootstrap : MonoBehaviour, ILevelSoftResetStartHandler, ILevelSoftResetEndHandler, ILevelReadyHandler, ILevelStartHandler, IPauseToggled, IRestart
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
        _input.Game.Enable();

        EventBus.Invoke<ILevelReadyHandler>(obj => obj.OnLevelReady());
    }

    public void OnPauseToggled()
    {
        TogglePause();
    }

    public void OnRestarted()
    {
        ReloadLevel();
    }

    private void TogglePause()
    {
        Time.timeScale = _pause ? 1f : 0f;
        _pause = !_pause;
        // TODO Disable inputs
    }

    public void OnLevelReady()
    {
        _input.Game.Move.actionMap.actionTriggered += OnAnyButtonPressed;
        if(_pause)
            TogglePause();
    }

    private void OnAnyButtonPressed(CallbackContext ctx) => EventBus.Invoke<ILevelStartHandler>(obj => obj.OnLevelStart());

    public void OnLevelStart()
    {
        _input.Game.Move.actionMap.actionTriggered -= OnAnyButtonPressed;
        _cloneSystem.Start();
        if(_pause)
            TogglePause();
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
        OnLevelReady();
    }

    private void Awake()
    {
        EventBus.Subscribe<ILevelReadyHandler>(this);
        EventBus.Subscribe<ILevelStartHandler>(this);
        EventBus.Subscribe<ILevelSoftResetStartHandler>(this);
        EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
        EventBus.Subscribe<IPauseToggled>(this);
        EventBus.Subscribe<IRestart>(this);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ILevelReadyHandler>(this);
        EventBus.Unsubscribe<ILevelStartHandler>(this);
        EventBus.Unsubscribe<ILevelSoftResetStartHandler>(this);
        EventBus.Unsubscribe<ILevelSoftResetEndHandler>(this);
        EventBus.Unsubscribe<IPauseToggled>(this);
        EventBus.Unsubscribe<IRestart>(this);
    }
}
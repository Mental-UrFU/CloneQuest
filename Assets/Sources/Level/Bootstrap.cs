using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class Bootstrap : MonoBehaviour, ILevelLoadHandler, ILevelSoftResetStartHandler, ILevelReadyHandler, ILevelStartHandler, IPauseToggleHandler, ILevelReloadHandler,
    IBeforeLevelUnloadHandler
{
    [SerializeField] private int _maxClones;
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private GameCanvasEvents _gameCanvas;

    private LevelContext _levelContext;
    private RecordingPlayerInput _playerInput;
    private CloneSystem _cloneSystem;
    private PlayerActions _input;
    private bool _pause;

    public void OnLevelLoad(LevelContext levelContext)
    {
        EventBus.Unsubscribe<ILevelLoadHandler>(this);
        _levelContext = levelContext;
    }

    private void PrepareLevel()
    {
        var playerControls = FindObjectOfType<PlayerControls>();
        var playerFinisher = playerControls.GetComponent<LevelFinisher>();
        _playerInput = new RecordingPlayerInput(playerControls);
        _cloneSystem = new CloneSystem(_playerInput, _clonePrefab, playerControls.transform.position, _maxClones);

        _input = new PlayerActions();
        _input.Game.Clone.started += (ctx) => { _cloneSystem.AddCloneAndRestart(); };
        _input.Game.Undo.started += (ctx) => { _cloneSystem.Restart(); };
        _input.Game.Restart.started += (ctx) => { OnLevelRestart(); };
        _input.Game.Esc.started += (ctx) => { TogglePause(); };
        _input.Game.Enable();

        _gameCanvas.Init(_cloneSystem);
        playerFinisher.OnAnimationStart.AddListener(DisableInput);
        EventBus.Invoke<ILevelReadyHandler>(obj => obj.OnLevelReady());
    }

    private void DisableInput()
    {
        _input.Disable();
        _playerInput.Enable = false;
    }

    private void EnableInput()
    {
        _input.Enable();
        _playerInput.Enable = true;
    }

    public void OnLevelReady()
    {
        _input.Game.Move.actionMap.actionTriggered += OnAnyButtonPressed;
        if (_pause) { TogglePause(); }
    }

    private void OnAnyButtonPressed(CallbackContext ctx) => EventBus.Invoke<ILevelStartHandler>(obj => obj.OnLevelStart());

    public void OnLevelStart()
    {
        _input.Game.Move.actionMap.actionTriggered -= OnAnyButtonPressed;
        _cloneSystem.Start();
        if (_pause) { TogglePause(); }
    }

    public void OnLevelRestart()
    {
        LevelManager.Load(_levelContext);
        if (_pause) { TogglePause(); }
    }

    public void OnPauseToggled() => TogglePause();

    private void TogglePause()
    {
        Time.timeScale = _pause ? 1f : 0f;
        _pause = !_pause;
        // TODO Disable inputs
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

    private void Subscribe()
    {
        EventBus.Subscribe<ILevelLoadHandler>(this);
        EventBus.Subscribe<ILevelReadyHandler>(this);
        EventBus.Subscribe<ILevelStartHandler>(this);
        EventBus.Subscribe<ILevelSoftResetStartHandler>(this);
        EventBus.Subscribe<IPauseToggleHandler>(this);
        EventBus.Subscribe<ILevelReloadHandler>(this);
        EventBus.Subscribe<IBeforeLevelUnloadHandler>(this);
    }

    private void Unsubscribe()
    {
        _input.Dispose();
        EventBus.Unsubscribe<ILevelReadyHandler>(this);
        EventBus.Unsubscribe<ILevelStartHandler>(this);
        EventBus.Unsubscribe<ILevelSoftResetStartHandler>(this);
        EventBus.Unsubscribe<IPauseToggleHandler>(this);
        EventBus.Unsubscribe<ILevelReloadHandler>(this);
        EventBus.Unsubscribe<IBeforeLevelUnloadHandler>(this);
    }

    private void Awake() => Subscribe();

#if UNITY_EDITOR
    [SerializeField] GameObject _audioControlsPrefab;
#endif
    private void Start()
    {
#if UNITY_EDITOR
        if (_levelContext == null)
        {
            _levelContext = new(0, Array.AsReadOnly(new string[] { SceneManager.GetActiveScene().name }), "");
            Debug.LogWarning($"LevelContext is null set {_levelContext.Id}");
        }
        if (AudioControl.Instance == null) { Instantiate(_audioControlsPrefab); }
#endif
        PrepareLevel();
    }
    private void OnDestroy()
    {
        EventBus.Invoke<IBeforeLevelUnloadHandler>(obj => obj.OnBeforeLevelUnload());
        Unsubscribe();
    }

    public void OnBeforeLevelUnload()
    {
        if (_pause) { TogglePause(); }
    }
}

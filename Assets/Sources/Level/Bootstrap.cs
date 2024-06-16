using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class Bootstrap : MonoBehaviour, ILevelLoadHandler, ILevelReadyHandler, ILevelStartHandler,
                                        ILevelSoftResetStartHandler, IPauseHandler, ILevelFinishHandler,
                                        ILevelRestartHandler, ILevelMenuLoadHandler, ILevelLoadNextHandler

{
    [SerializeField] private int _maxClones;
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private GameCanvas _gameCanvas;

    private LevelContext _levelContext;
    private RecordingPlayerInput _playerInput;
    private CloneSystem _cloneSystem;
    private PlayerActions _input;
    private StarCounter _stars;

    public void OnLevelLoad(LevelContext levelContext)
    {
        EventBus.Unsubscribe<ILevelLoadHandler>(this);
        _levelContext = levelContext;
    }

    private void PrepareLevel()
    {
        var playerControls = FindObjectOfType<PlayerControls>();
        var playerAnimation = playerControls.GetComponentInChildren<PlayerAnimation>();
        var playerFinisher = playerControls.GetComponent<LevelFinisher>();
        _playerInput = new RecordingPlayerInput(playerControls);
        _cloneSystem = new CloneSystem(_playerInput, _clonePrefab, playerControls.transform.position, playerAnimation.IsFacingRightDefault, _maxClones);
        _stars = new();

        _input = new PlayerActions();
        _input.Game.Clone.started += (ctx) => { _cloneSystem.AddCloneAndRestart(); };
        _input.Game.Undo.started += (ctx) => { _cloneSystem.Restart(); };
        _input.Game.Restart.started += (ctx) => { OnLevelRestart(); };
        _input.Game.Esc.started += (ctx) => { _gameCanvas.ShowPauseMenu(); };
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
        _gameCanvas.ShowAnyButtonScreen();
    }

    private void OnAnyButtonPressed(CallbackContext ctx)
    {
        _gameCanvas.HideAnyButtonScreen();
        EventBus.Invoke<ILevelStartHandler>(obj => obj.OnLevelStart());
    }

    public void OnLevelStart()
    {
        _input.Game.Move.actionMap.actionTriggered -= OnAnyButtonPressed;
        _cloneSystem.Start();
    }
    public void OnLevelRestart() { DisableInput(); LevelManager.Load(_levelContext); }
    public void OnLoadMenu() { LevelManager.LoadMenu(_levelContext); }
    public void OnLevelFinish()
    {
        LevelRepository.Get(_levelContext.Id, Save);
        void Save(LevelData prevValue)
        {
            if (prevValue.Passed && prevValue.Stars >= _stars.Count) { ShowLevelCompleteMenu(prevValue.Stars); }
            else { LevelRepository.Set(_levelContext.Id, new LevelData(_stars.Count), () => ShowLevelCompleteMenu(_stars.Count)); }
        }
        void ShowLevelCompleteMenu(int starCount) => _gameCanvas.ShowLevelCompleteMenu(_levelContext.Index + 1, starCount);
    }

    public void OnLoadNext() { if (_levelContext.IsLast) { LevelManager.LoadMenu(_levelContext); } else { LevelManager.Load(_levelContext.Next); } }

    public void OnPause() { DisableInput(); }
    public void OnResume() { EnableInput(); }

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
        EventBus.Subscribe<IPauseHandler>(this);
        EventBus.Subscribe<ILevelFinishHandler>(this);
        EventBus.Subscribe<ILevelRestartHandler>(this);
        EventBus.Subscribe<ILevelMenuLoadHandler>(this);
        EventBus.Subscribe<ILevelLoadNextHandler>(this);
    }

    private void Unsubscribe()
    {
        _input.Dispose();
        EventBus.Unsubscribe<ILevelLoadHandler>(this);
        EventBus.Unsubscribe<ILevelReadyHandler>(this);
        EventBus.Unsubscribe<ILevelStartHandler>(this);
        EventBus.Unsubscribe<ILevelSoftResetStartHandler>(this);
        EventBus.Unsubscribe<IPauseHandler>(this);
        EventBus.Unsubscribe<ILevelFinishHandler>(this);
        EventBus.Unsubscribe<ILevelRestartHandler>(this);
        EventBus.Unsubscribe<ILevelMenuLoadHandler>(this);
        EventBus.Unsubscribe<ILevelLoadNextHandler>(this);
    }

    private void Awake() => Subscribe();

#if UNITY_EDITOR
    [SerializeField] GameObject _audioControlsPrefab;
    [SerializeField] GameObject _sceneTransitionPrefab;
#endif
    private void Start()
    {
#if UNITY_EDITOR
        if (_levelContext == null)
        {
            _levelContext = new(0, Array.AsReadOnly(new string[] { SceneManager.GetActiveScene().name }), SceneManager.GetActiveScene().name);
            Debug.LogWarning($"LevelContext is null set {_levelContext.Id}");
        }
        if (AudioControl.Instance == null) { Instantiate(_audioControlsPrefab); }
        if (SceneTransition.Instance == null) { Instantiate(_sceneTransitionPrefab); }
#endif
        PrepareLevel();
    }
    private void OnDestroy()
    {
        EventBus.Invoke<IBeforeLevelUnloadHandler>(obj => obj.OnBeforeLevelUnload());
        Unsubscribe();
        Pause.Set(false);
    }
}

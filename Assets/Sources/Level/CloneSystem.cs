using System;
using System.Collections.Generic;
using UnityEngine;


public class CloneSystem : ILevelSoftResetEndHandler, IBeforeLevelUnloadHandler
{
    public enum ReadyState { Ready, Running, NotReady }
    public ReadyState State { get; private set; }
    public int CloneCount => _clones.Count;
    public int MaxClonesCount => _maxClones;

    public event Action OnUpdate;

    private readonly RecordingPlayerInput _playerInput;
    private readonly GameObject _clonePrefab;
    private readonly Vector2 _initialPosition;
    private readonly bool _isFacingRightDefault;
    private readonly List<(InputReplay input, GameObject gameObject)> _clones = new();

    private InputRecord _newRecord = null;
    private readonly int _maxClones = 0;

    public CloneSystem(RecordingPlayerInput playerInput, GameObject clonePrefab, Vector2 initialPosition, bool isFacingRightDefault, int maxClones = int.MaxValue)
    {
        _playerInput = playerInput;
        _clonePrefab = clonePrefab;
        _initialPosition = initialPosition;
        _isFacingRightDefault = isFacingRightDefault;
        _maxClones = maxClones;
        _playerInput.Reset();
        _clones.Clear();
        Subscribe();
    }

    public void Start()
    {
        if (State != ReadyState.Ready) { return; }
        State = ReadyState.Running;
        Coroutines.Run(() =>
        {
            _playerInput.Enable = true;
            _clones.ForEach(clone => clone.input.Start());
        }, new WaitForFixedUpdate());
    }

    public void AddCloneAndRestart()
    {
        if (State != ReadyState.Running || _clones.Count >= _maxClones) { return; }
        _newRecord = _playerInput.ResetAndReturn();
        Restart();
    }

    public void Restart()
    {
        State = ReadyState.NotReady;
        _playerInput.Reset();
        _clones.ForEach(clone => clone.input.Reset());
        EventBus.Invoke<ILevelSoftResetStartHandler>(obj => obj.OnSoftResetStart(1f));
    }

    private void SpawnClone(InputRecord inputRecord)
    {
        var clone = UnityEngine.Object.Instantiate(_clonePrefab, (Vector3)_initialPosition, Quaternion.identity);
        var cloneControls = clone.GetComponent<IControllable>();
        var cloneRenderer = clone.GetComponentInChildren<SpriteRenderer>();
        var cloneAnimation = clone.GetComponentInChildren<PlayerAnimation>();
        cloneAnimation.IsFacingRightDefault = _isFacingRightDefault;
        cloneRenderer.sortingOrder = CloneCount;
        _clones.Add((new InputReplay(cloneControls, inputRecord), clone));
    }

    public void OnSoftResetEnd()
    {
        if (_newRecord != null)
        {
            SpawnClone(_newRecord);
            _newRecord = null;
            OnUpdate?.Invoke();
        }
        State = ReadyState.Ready;
    }

    public void OnBeforeLevelUnload() => Unsubscribe();

    private void Subscribe()
    {
        EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
        EventBus.Subscribe<IBeforeLevelUnloadHandler>(this);
    }

    private void Unsubscribe()
    {
        EventBus.Unsubscribe<ILevelSoftResetEndHandler>(this);
        EventBus.Unsubscribe<IBeforeLevelUnloadHandler>(this);
    }
}

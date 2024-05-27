using UnityEngine;

public interface ILevelLoadHandler { void OnLevelLoad(LevelContext levelContext); }
public interface ILevelReadyHandler { void OnLevelReady(); }
public interface ILevelStartHandler { void OnLevelStart(); }
public interface IBeforeLevelUnloadHandler { void OnBeforeLevelUnload(); }
public interface ILevelReloadHandler { void OnLevelRestart(); }

public interface ILevelSoftResetStartHandler { void OnSoftResetStart(float duration); }
public interface ILevelSoftResetEndHandler { void OnSoftResetEnd(); }

public interface ILevelFinishHandler { void OnLevelFinish(); }

public interface IPauseToggleHandler { void OnPauseToggled(); }

public interface IStarCollected { void OnStarCollected(); }

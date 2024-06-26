public interface ILevelLoadHandler { void OnLevelLoad(LevelContext levelContext); }
public interface ILevelReadyHandler { void OnLevelReady(); }
public interface ILevelStartHandler { void OnLevelStart(); }
public interface IBeforeLevelUnloadHandler { void OnBeforeLevelUnload(); }
public interface ILevelRestartHandler { void OnLevelRestart(); }
public interface ILevelLoadNextHandler { void OnLoadNext(); }
public interface ILevelMenuLoadHandler { void OnLoadMenu(); }

public interface ILevelSoftResetStartHandler { void OnSoftResetStart(float duration); }
public interface ILevelSoftResetEndHandler { void OnSoftResetEnd(); }

public interface ILevelFinishHandler { void OnLevelFinish(); }
public interface IPauseHandler { void OnPause(); void OnResume(); }

public interface IStarCollectHandler { void OnStarCollected(); }

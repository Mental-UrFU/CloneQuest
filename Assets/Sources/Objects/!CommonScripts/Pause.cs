using UnityEngine;

public static class Pause
{
    public static bool IsPaused => Time.timeScale == 0f;

    public static void Set(bool isPause)
    {
        if (isPause == IsPaused) { return; }
        if (isPause)
        {
            Time.timeScale = 0f;
            EventBus.Invoke<IPauseHandler>(obj => obj.OnPause());
        }
        else
        {
            Time.timeScale = 1f;
            EventBus.Invoke<IPauseHandler>(obj => obj.OnResume());
        }
    }
}

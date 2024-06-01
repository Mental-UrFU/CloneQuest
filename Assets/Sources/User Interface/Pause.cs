using UnityEngine;

public class Pause : MonoBehaviour
{
    public void TogglePause()
    {
        EventBus.Invoke<IPauseToggleHandler>(act => act.OnPauseToggled());
    }
}

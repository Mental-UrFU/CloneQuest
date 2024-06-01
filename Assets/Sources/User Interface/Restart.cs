using UnityEngine;

public class Restart : MonoBehaviour
{
    public void DoRestart()
    {
        EventBus.Invoke<ILevelReloadHandler>(act => act.OnLevelRestart());
    }
}

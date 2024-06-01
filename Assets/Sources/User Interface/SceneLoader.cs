using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour // TODO REMOVE
{
    public static void LoadScene(string sceneName)
    {
        EventBus.Invoke<IBeforeLevelUnloadHandler>((obj) => obj.OnBeforeLevelUnload());
        SceneManager.LoadScene(sceneName);
    }
    
    public static void TryLoadingNextLevel()
    {
        LevelManager.LoadNextLevel();
    }
}

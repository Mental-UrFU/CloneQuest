using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static void Load(LevelContext levelContext)
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
        SceneTransition.Instance.Show(() => SceneManager.LoadScene(levelContext.Id));

        void OnLevelLoaded(Scene _, LoadSceneMode __)
        {
            SceneManager.sceneLoaded -= OnLevelLoaded;
            SceneTransition.Instance.Hide();
            EventBus.Invoke<ILevelLoadHandler>((obj) => obj.OnLevelLoad(levelContext));
        }
    }

    public static void LoadMenu(LevelContext levelContext)
    {
        SceneManager.sceneLoaded += OnMenuLoaded;
        SceneTransition.Instance.Show(() => SceneManager.LoadScene(levelContext.FromId));

        void OnMenuLoaded(Scene _, LoadSceneMode __)
        {
            SceneManager.sceneLoaded -= OnMenuLoaded;
            SceneTransition.Instance.Hide();
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGetter : MonoBehaviour
{   
    private void Awake()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        var levelNumber = sceneName.Split('L');
        var level = levelNumber[1];
        gameObject.GetComponent<TextMeshProUGUI>().text = $"˜˜˜˜˜˜˜ {level}";
    }
}

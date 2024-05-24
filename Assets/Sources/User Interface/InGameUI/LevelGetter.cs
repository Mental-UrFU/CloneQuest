using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGetter : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {

        var sceneName = SceneManager.GetActiveScene().name;
        var levelNubmer = sceneName.Split('L');
        var level = levelNubmer[1];
        gameObject.GetComponent<TextMeshProUGUI>().text = $"спнбемэ {level}";
    }
}

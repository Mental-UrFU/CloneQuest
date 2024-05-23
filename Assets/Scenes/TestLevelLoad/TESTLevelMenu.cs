using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TESTLevelMenu : MonoBehaviour
{
    [SerializeField] RectTransform _container;
    [SerializeField] Button _buttonTemplate;
    [HideInInspector][SerializeField] string[] _levelIds;
    
    private Button[] _buttons;

    private void CreateButtons()
    {
        _buttons = _levelIds.Select(id => Instantiate(_buttonTemplate, _container)).ToArray();
        LevelManager.levelsByName = _levelIds.ToList();
        for (var i = 0; i < _buttons.Length; i++)
        {
            if (!PersistentLevelData.LevelStars.ContainsKey(i))
            {
                PersistentLevelData.LevelStars.Add(i, -1);
            }
            var button = _buttons[i];
            var levelIndex = i;
            button.GetComponentInChildren<TMP_Text>().text = $"{levelIndex + 1}";
            button.GetComponentInChildren<StarcounterSetter>().DisplayStarsCount(PersistentLevelData.LevelStars[levelIndex]);
            if (PersistentLevelData.LevelStars[levelIndex] == -1 && i != 0)
            {
                button.GetComponentInChildren<StarcounterSetter>().DisplayLock();
            }
            else
            {
                button.onClick.AddListener(() => LoadLevel(levelIndex));
            }            
        }
    }

    private void LoadLevel(int index)
    {
        var context = new LevelContext(index, Array.AsReadOnly(_levelIds), SceneManager.GetActiveScene().name);
        PersistentLevelData.CurrentLevel = index;
        LevelManager.Load(context);
    }

    private void LoadLevelMetadata()
    {

    }

    private void Awake()
    {
        CreateButtons();
        LoadLevelMetadata();
    }

#if UNITY_EDITOR
    [SerializeField] SceneAsset[] _levelAssets;
    private void OnValidate()
    {
        _levelIds = _levelAssets
            .Select(scene => scene.name)
            .ToArray();        
    }
#endif
}

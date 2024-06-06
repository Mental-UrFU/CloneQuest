using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] RectTransform _container;
    [SerializeField] Button _buttonTemplate;
    [SerializeField] bool _unlockAllLevels;
    [HideInInspector][SerializeField] string[] _levelIds;

    
    private Button[] _buttons;

    private void CreateButtons()
    {
        _buttons = _levelIds.Select(id => Instantiate(_buttonTemplate, _container)).ToArray();
        Dictionary<string, int> levelStars = new Dictionary<string, int>();
        List<LevelData> _levelData = new();
        LevelRepository.Get(_levelIds, SetLevelStars);
        void SetLevelStars(List<LevelData> levelData)
        {
            _levelData = levelData;
        }

        for (var i = 0; i < _buttons.Length; i++)
        {
            var button = _buttons[i];
            var levelIndex = i;
            button.GetComponentInChildren<TMP_Text>().text = $"{levelIndex + 1}";
            button.GetComponentInChildren<StarcounterSetter>().DisplayStarsCount(_levelData[levelIndex].Stars);
            if (_levelData[levelIndex].Passed == false && i != 0 && !_unlockAllLevels)
            {
                if (!_levelData[levelIndex - 1].Passed)
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

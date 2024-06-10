using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButton : MonoBehaviour
{
    [SerializeField] private GameObject disablePicture;
    [SerializeField] bool StartOnSceneLoad = false;
    
    void Start()
    {
        if (StartOnSceneLoad)
        {
            if(AudioControl.Instance.Music == 0f)
                disablePicture.SetActive(true);
        }        
    }

    public void OnMusicToggle()
    {
        if(AudioControl.Instance.Music == 0f)
            disablePicture.SetActive(true);
        else
            disablePicture.SetActive(false);
    }
}

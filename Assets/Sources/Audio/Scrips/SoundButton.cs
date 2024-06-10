using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : MonoBehaviour
{
    [SerializeField] private GameObject disablePicture;
    [SerializeField] bool StartOnSceneLoad = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (StartOnSceneLoad)
        {
            if (AudioControl.Instance.Sound == 0f)
                disablePicture.SetActive(true);
        }
    }

    public void OnSoundToggle()
    {
        if (AudioControl.Instance.Sound == 0f)
            disablePicture.SetActive(true);
        else
            disablePicture.SetActive(false);
    }
}

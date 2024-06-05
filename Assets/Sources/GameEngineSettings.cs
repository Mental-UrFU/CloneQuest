using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngineSettings : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 59;
        QualitySettings.vSyncCount = 1;
    }
}

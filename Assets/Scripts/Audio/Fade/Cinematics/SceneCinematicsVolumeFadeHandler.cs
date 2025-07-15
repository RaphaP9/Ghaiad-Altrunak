using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCinematicsVolumeFadeHandler : SceneVolumeFadeHandler
{
    private void Start()
    {
        SetVolumeFadeManager(CinematicsVolumeFadeManager.Instance);
    }
}


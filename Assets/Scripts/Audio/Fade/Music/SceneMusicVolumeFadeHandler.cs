using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicVolumeFadeHandler : SceneVolumeFadeHandler
{
    private void Start()
    {
        SetVolumeFadeManager(MusicVolumeFadeManager.Instance);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeListener : MonoBehaviour
{
    [Header("Camera Shake Objects Settings")]
    [SerializeField] private List<CameraShake> cameraShakes;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void CheckStartShake(string log)
    {
        foreach(CameraShake cameraShake in cameraShakes)
        {
            if (cameraShake.logToShake != log) continue;        
            if (!cameraShake.enabled) return;

            CameraShakeHandler.Instance.ShakeCamera(cameraShake);
            return;          
        }
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckStartShake(e.gameplayAction.log);
    }
}
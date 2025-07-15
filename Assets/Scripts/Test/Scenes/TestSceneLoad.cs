using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneLoad : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string scene;
    [SerializeField] private TransitionType transitionType;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScenesManager.Instance.TransitionLoadTargetScene(scene, transitionType);
        }
    }
}

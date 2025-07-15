using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUIScaleToCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Mode mode;

    private enum Mode {ConstantScale}

    private GameObject player;

    private const string PLAYER_TAG = "Player";
    private const float PLAYER_DISTANCE_TO_UPDATE = 12.5f;

    private void Start()
    {
        InitializeVariables();
    }

    private void LateUpdate()
    {
        ScaleLogic();
    }

    private void InitializeVariables()
    {
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
    }

    private void ScaleLogic()
    {
        if (!CheckPlayerClose()) return;

        switch (mode)
        {
            case Mode.ConstantScale:
                transform.localScale = CameraOrthoSizeWorldSpaceUISizeHandler.Instance.WorldSpaceUIScaleFactor;
                break;
        }
    }

    private bool CheckPlayerClose()
    {
        if (!player) return true;
        if (Vector3.Distance(transform.position, player.transform.position) <= PLAYER_DISTANCE_TO_UPDATE) return true;

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTeleporterManager : MonoBehaviour
{
    public static PlayerTeleporterManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static EventHandler<OnPlayerTeleportEventArgs> OnPlayerTeleported;

    public class OnPlayerTeleportEventArgs: EventArgs
    {
        public Vector2 teleportPosition;
        public bool cameraInstantPosition;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one PlayerTeleporterManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    public void TeleportPlayerToPosition(Vector2 position, bool cameraInstantPosition)
    {
        if(PlayerTransformRegister.Instance == null || PlayerTransformRegister.Instance.PlayerTransform == null)
        {
            if (debug) Debug.Log("Can not Teleport player. Either PlayerTransformRegister or PlayerTransform is null");
            return;
        }

        PlayerTransformRegister.Instance.PlayerTransform.position = GeneralUtilities.Vector2ToVector3(position);

        OnPlayerTeleported?.Invoke(this, new OnPlayerTeleportEventArgs { teleportPosition = position, cameraInstantPosition = cameraInstantPosition });
    }
}

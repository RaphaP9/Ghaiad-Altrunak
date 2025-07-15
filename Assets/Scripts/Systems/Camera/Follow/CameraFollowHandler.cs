using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera CMVCAM;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const string CAMERA_FOLLOW_POINT = "CameraFollowPoint";

    public static event EventHandler<OnCameraFollowPointEventArgs> OnCameraFollowPointSet;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 originalDamping;

    public class OnCameraFollowPointEventArgs : EventArgs
    {
        public Transform cameraFollowPoint;
    }

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
        PlayerTeleporterManager.OnPlayerTeleported += PlayerTeleporterManager_OnPlayerTeleported;

        CameraTransitionHandler.OnCameraTransitionPositionDeterminedPreFollow += CameraTransitionHandler_OnCameraTransitionPositionDeterminedPreFollow;
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedPostFollow += CameraTransitionHandler_OnCameraTransitionPositionDeterminedPostFollow;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
        PlayerTeleporterManager.OnPlayerTeleported -= PlayerTeleporterManager_OnPlayerTeleported;

        CameraTransitionHandler.OnCameraTransitionPositionDeterminedPreFollow -= CameraTransitionHandler_OnCameraTransitionPositionDeterminedPreFollow;
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedPostFollow -= CameraTransitionHandler_OnCameraTransitionPositionDeterminedPostFollow;
    }

    private void Awake()
    {
        cinemachineTransposer = CMVCAM.GetCinemachineComponent<CinemachineTransposer>();
        InitializeDamping();
    }

    private void InitializeDamping()
    {
        originalDamping = new Vector3(cinemachineTransposer.m_XDamping, cinemachineTransposer.m_YDamping, cinemachineTransposer.m_ZDamping);
    }

    private Transform SeekCameraFollowPoint(Transform playerTransform)
    {
        Transform cameraFollowPoint = playerTransform.Find(CAMERA_FOLLOW_POINT);

        if(cameraFollowPoint == null)
        {
            if (debug) Debug.Log("Could not find Camera Follow Point. Returning Null.");
        }
        return cameraFollowPoint;
    }

    private void SetCameraFollowPoint(Transform cameraFollowPoint)
    {
        if (cameraFollowPoint == null) return;
        CMVCAM.Follow = cameraFollowPoint;

        OnCameraFollowPointSet?.Invoke(this, new OnCameraFollowPointEventArgs { cameraFollowPoint = cameraFollowPoint });
    }

    private IEnumerator MoveInstanltyToNextPosition()
    {
        DisableDamping();

        yield return null;
        yield return null;

        EnableDamping();
    }
    
    private void DisableDamping()
    {
        cinemachineTransposer.m_XDamping = 0f;
        cinemachineTransposer.m_YDamping = 0f;
        cinemachineTransposer.m_ZDamping = 0f;
    }

    private void EnableDamping()
    {
        cinemachineTransposer.m_XDamping = originalDamping.x;
        cinemachineTransposer.m_YDamping = originalDamping.y;
        cinemachineTransposer.m_ZDamping = originalDamping.z;
    }

    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        Transform cameraFollowPoint = SeekCameraFollowPoint(e.playerTransform);
        SetCameraFollowPoint(cameraFollowPoint);
    }

    private void PlayerTeleporterManager_OnPlayerTeleported(object sender, PlayerTeleporterManager.OnPlayerTeleportEventArgs e)
    {
        if (!e.cameraInstantPosition) return;

        StartCoroutine(MoveInstanltyToNextPosition());
    }

    private void CameraTransitionHandler_OnCameraTransitionPositionDeterminedPreFollow(object sender, CameraTransitionHandler.OnCameraTransitionEventArgs e)
    {
        DisableDamping();
    }

    private void CameraTransitionHandler_OnCameraTransitionPositionDeterminedPostFollow(object sender, CameraTransitionHandler.OnCameraTransitionEventArgs e)
    {
        EnableDamping();
    }
}

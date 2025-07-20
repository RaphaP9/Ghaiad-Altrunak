using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<CinemachineVirtualCamera> CMVCAMs;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const string CAMERA_FOLLOW_POINT = "CameraFollowPoint";

    public class OnCameraFollowPointEventArgs : EventArgs
    {
        public Transform cameraFollowPoint;
    }

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
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

    private void SetCameraFollowPoint(CinemachineVirtualCamera CMVCAM, Transform cameraFollowPoint)
    {
        if (cameraFollowPoint == null) return;
        CMVCAM.Follow = cameraFollowPoint;
    }

    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        Transform cameraFollowPoint = SeekCameraFollowPoint(e.playerTransform);

        foreach(CinemachineVirtualCamera CMVCAM in CMVCAMs)
        {
            SetCameraFollowPoint(CMVCAM, cameraFollowPoint);
        }
    }
}

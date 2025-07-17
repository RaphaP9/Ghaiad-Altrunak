using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfinerHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedPreFollow += CameraTransitionHandler_OnCameraTransitionPositionDeterminedPreFollow;
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedEnd += CameraTransitionHandler_OnCameraTransitionPositionDeterminedEnd;
    }

    private void OnDisable()
    {
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedPreFollow -= CameraTransitionHandler_OnCameraTransitionPositionDeterminedPreFollow;
        CameraTransitionHandler.OnCameraTransitionPositionDeterminedEnd -= CameraTransitionHandler_OnCameraTransitionPositionDeterminedEnd;
    }

    private void SwitchConfiner(PolygonCollider2D confiner)
    {
        cinemachineConfiner2D.m_BoundingShape2D = confiner;
        cinemachineConfiner2D.enabled = false; //Force Reinitialization 
        cinemachineConfiner2D.enabled = true;
    }

    private void EnableConfiner()
    {
        cinemachineConfiner2D.enabled = true;
    }

    private void DisableConfiner()
    {
        cinemachineConfiner2D.enabled = false;
    }

    private void CameraTransitionHandler_OnCameraTransitionPositionDeterminedPreFollow(object sender, CameraTransitionHandler.OnCameraTransitionEventArgs e)
    {
        DisableConfiner();
    }

    private void CameraTransitionHandler_OnCameraTransitionPositionDeterminedEnd(object sender, CameraTransitionHandler.OnCameraTransitionEventArgs e)
    {
        EnableConfiner();
    }

}

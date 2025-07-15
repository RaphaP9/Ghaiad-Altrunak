using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrthoSizeWorldSpaceUISizeHandler : MonoBehaviour
{
    public static CameraOrthoSizeWorldSpaceUISizeHandler Instance;

    [Header("Components")]
    [SerializeField] private CameraOrthoSizeHandler cameraOrthoSizeHandler;
    [SerializeField] private Camera mainCamera;

    [Header("Settings")]
    [SerializeField, Range(1f, 2f)] private float scaleFactorMin;
    [SerializeField, Range(1f, 2f)] private float scaleFactorMax;
    [SerializeField, Range(0.01f, 150f)] private float smoothScaleFactor;

    private Vector3 desiredFactor;
    public Vector3 WorldSpaceUIScaleFactor { get; private set; }

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
            Debug.LogWarning("There is more than one CameraScrollWorldSpaceUIHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CalculateWorldSpaceUIScaleFactor();
        SmoothFactor();
    }

    private void CalculateWorldSpaceUIScaleFactor()
    {
        desiredFactor = CalculateUIFixedFactor() * cameraOrthoSizeHandler.OrthoSizeFactor * Vector3.one;
    }

    private float CalculateUIFixedFactor()
    {
        float fixedFactor = Mathf.Lerp(scaleFactorMin, scaleFactorMax, cameraOrthoSizeHandler.OrthoSizeFactor);
        return fixedFactor;
    }

    private void SmoothFactor()
    {
        WorldSpaceUIScaleFactor = Vector3.Lerp(WorldSpaceUIScaleFactor, desiredFactor, smoothScaleFactor * Time.deltaTime);
    }
}

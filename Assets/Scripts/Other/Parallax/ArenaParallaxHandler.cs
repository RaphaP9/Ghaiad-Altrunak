using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaParallaxHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform arenaCenterRefference;
    [SerializeField] private Transform refferenceCameraTransform;

    [Header("Settings")]
    [SerializeField] private Vector2 displacementMultipliers;

    private const float DISTANCE_THRESHOLD_TO_UPDATE = 50f;

    private void Update()
    {
        HandleParallax();
    }

    private void HandleParallax()
    {
        if (refferenceCameraTransform == null) return;

        Vector2 cameraOffsetFromCenter = GeneralUtilities.Vector3ToVector2(refferenceCameraTransform.position - arenaCenterRefference.position);

        if (cameraOffsetFromCenter.magnitude > DISTANCE_THRESHOLD_TO_UPDATE) return;
       
        transform.localPosition = new Vector3(cameraOffsetFromCenter.x * displacementMultipliers.x, cameraOffsetFromCenter.y * displacementMultipliers.y, transform.position.z);
    }
}

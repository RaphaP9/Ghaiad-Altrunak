using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDirectionHandler : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private Vector2 normalizedMouseDirection;

    public Vector2 NormalizedMouseDirection => normalizedMouseDirection;

    public Vector2 Input => ScreenInput.Instance.GetWorldMousePosition();

    private void Update()
    {
        HandleMouseDirection();
    }

    private void HandleMouseDirection()
    {
        Vector2 rawDirection = Input - GeneralUtilities.TransformPositionVector2(transform);
        normalizedMouseDirection = rawDirection.normalized;
    }
}

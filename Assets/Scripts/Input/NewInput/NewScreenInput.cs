using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScreenInput : ScreenInput
{
    [Header("Components")]
    [SerializeField] private Camera _camera;

    public override bool CanProcessInput()
    {
        if(PauseManager.Instance.GamePaused) return false;
        return true;
    }

    public override Vector2 GetWorldMousePosition()
    {
        if(!CanProcessInput()) return lastValidWorldMousePosition;

        Vector3 rawPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 screenPosition = GeneralUtilities.SupressZComponent(rawPosition);

        lastValidWorldMousePosition = screenPosition;

        return screenPosition;
    }
}

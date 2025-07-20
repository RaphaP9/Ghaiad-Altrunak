using System;
using UnityEngine;

public class NewMovementInput : MovementInput
{
    private PlayerInputActions playerInputActions;

    private Vector2 LastNonZeroMovementInput = new Vector2(1f, 0f); //Default Value Assigned

    private void OnDisable()
    {
        playerInputActions?.Movement.Disable();
    }

    private void Start()
    {
        InitializePlayerInputActions(CentralizedInputSystemManager.Instance.PlayerInputActions);
    }

    private void InitializePlayerInputActions(PlayerInputActions playerInputActions)
    {
        this.playerInputActions = playerInputActions;   
        playerInputActions.Movement.Enable();
    }


    private void Update()
    {
        CalculateLastNonZeroInput();
    }

    private void CalculateLastNonZeroInput()
    {
        if (GetMovementInputNormalized() == Vector2.zero) return;
        
        LastNonZeroMovementInput = GetMovementInputNormalized();
        
    }

    public override bool CanProcessInput()
    {
        if(playerInputActions == null) return false;

        //if (GameManager.Instance.GameState != GameManager.State.OnWave) return false;
        if (PauseManager.Instance.GamePaused) return false;

        return true;
    }

    public override Vector2 GetMovementInputNormalized()
    {
        if (!CanProcessInput()) return Vector2.zero;

        Vector2 input = playerInputActions.Movement.Move.ReadValue<Vector2>();

        return input;
    }

    public override Vector2 GetLastNonZeroMovementInputNormalized() => LastNonZeroMovementInput;
}

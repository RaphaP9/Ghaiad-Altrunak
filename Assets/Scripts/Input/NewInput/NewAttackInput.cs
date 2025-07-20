using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackInput : AttackInput
{
    private PlayerInputActions playerInputActions;

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
        playerInputActions.Attack.Enable();
    }

    public override bool CanProcessInput()
    {
        if (playerInputActions == null) return false;

        //if (GameManager.Instance.GameState != GameManager.State.OnWave) return false;
        if (PauseManager.Instance.GamePaused) return false;

        return true;
    }

    public override bool GetAttackDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Attack.Attack.WasPerformedThisFrame();

        return input;
    }

    public override bool GetAttackHold()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Attack.Attack.IsPressed();

        return input;
    }
}

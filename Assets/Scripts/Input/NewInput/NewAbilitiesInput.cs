using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAbilitiesInput : AbilitiesInput
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
        playerInputActions.Abilities.Enable();
    }

    public override bool CanProcessInput()
    {
        if (playerInputActions == null) return false;

        //if (GameManager.Instance.GameState != GameManager.State.OnWave) return false;
        if (PauseManager.Instance.GamePaused) return false;

        return true;
    }

    public override bool GetAbilityADown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityA.WasPerformedThisFrame();

        return input;
    }

    public override bool GetAbilityAHold()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityA.IsPressed();

        return input;
    }

    public override bool GetAbilityBDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityB.WasPerformedThisFrame();

        return input;
    }

    public override bool GetAbilityBHold()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityB.IsPressed();

        return input;
    }

    public override bool GetAbilityCDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityC.WasPerformedThisFrame();

        return input;
    }

    public override bool GetAbilityCHold()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityC.IsPressed();

        return input;
    }
}

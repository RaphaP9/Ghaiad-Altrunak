using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUIInput : UIInput
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
        playerInputActions.UI.Enable();
    }

    public override bool CanProcessInput()
    {
        if (playerInputActions == null) return false;

        if (ScenesManager.Instance.SceneState != ScenesManager.State.Idle) return false;
        return true;
    }

    public override bool GetPauseDown()
    {
        if (!CanProcessInput()) return false;
        if (InputOnCooldown()) return false;

        bool pauseInput = playerInputActions.UI.Pause.WasPerformedThisFrame();
        return pauseInput;
    }

    public override bool GetStatsDown()
    {
        if (!CanProcessInput()) return false;
        if (InputOnCooldown()) return false;

        bool statsInput = playerInputActions.UI.Stats.WasPerformedThisFrame();
        return statsInput;
    }

    public override bool GetDevMenuDown()
    {
        if (!CanProcessInput()) return false;
        if (InputOnCooldown()) return false;

        bool devMenuInput = playerInputActions.UI.DevMenu.WasPerformedThisFrame();
        return devMenuInput;
    }
}

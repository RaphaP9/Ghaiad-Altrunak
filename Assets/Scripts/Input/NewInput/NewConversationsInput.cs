using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewConversationsInput : ConversationsInput
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
        playerInputActions.Conversations.Enable();
    }

    public override bool CanProcessInput()
    {
        if (playerInputActions == null) return false;

        if (PauseManager.Instance.GamePaused) return false;
        return true;
    }

    public override bool GetSkipDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Conversations.Skip.WasPerformedThisFrame();

        return input;
    }
}

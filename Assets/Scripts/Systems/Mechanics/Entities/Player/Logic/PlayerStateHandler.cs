using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerAbilitiesCastingHandler playerAbilitiesCastingHandler;
    [Space]
    [SerializeField] private PlayerFacingDirectionHandler playerFacingDirectionHandler;
    [SerializeField] private PlayerWeaponAimHandler playerWeaponAimHandler;
    [SerializeField] private PlayerAimDirectionHandler playerAimDirectionerHandler;

    [Header("Settings")]
    [SerializeField] private PlayerState startingState;

    [Header("Runtime Filled")]
    [SerializeField] private PlayerState playerState;

    public PlayerState State => playerState;
    public enum PlayerState {ZeroActions, Combat, NoCombat, Rest, Dead}

    private void Awake()
    {
        //NOTE: If this method is on Start(), must coincide with the first state Initialized By Game Manager (Zero Actions). Line can also be removed
        SetPlayerState(startingState);
    }

    private void Update()
    {
        HandleStateUpdate();
    }

    private void FixedUpdate()
    {
        HandleStateFixedUpdate();
    }

    private void LateUpdate()
    {
        HandleStateLateUpdate();
    }

    private void HandleStateUpdate()
    {
        switch (playerState)
        {
            case PlayerState.ZeroActions:
                ZeroActionsLogicUpdate();
                break;
            case PlayerState.Combat:
            default:
                CombatLogicUpdate();
                break;
            case PlayerState.NoCombat:
                NoCombatLogicUpdate();
                break;
            case PlayerState.Rest:
                RestLogicUpdate();
                break;
        }
    }

    private void HandleStateFixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.ZeroActions:
                ZeroActionsLogicFixedUpdate();
                break;
            case PlayerState.Combat:
            default:
                CombatLogicFixedUpdate();
                break;
            case PlayerState.NoCombat:
                NoCombatLogicFixedUpdate();
                break;
            case PlayerState.Rest:
                RestLogicFixedUpdate();
                break;
        }
    }

    private void HandleStateLateUpdate()
    {
        switch (playerState)
        {
            case PlayerState.ZeroActions:
                ZeroActionsLogicLateUpdate();
                break;
            case PlayerState.Combat:
            default:
                CombatLogicLateUpdate();
                break;
            case PlayerState.NoCombat:
                NoCombatLogicLateUpdate();
                break;
            case PlayerState.Rest:
                RestLogicLateUpdate();
                break;
        }
    }

    #region ZeroActions Logics
    private void ZeroActionsLogicUpdate()
    {

    }

    private void ZeroActionsLogicFixedUpdate()
    {
        playerMovement.Stop();
    }

    private void ZeroActionsLogicLateUpdate()
    {

    }

    #endregion

    #region Combat Logics
    private void CombatLogicUpdate()
    {
        playerMovement.HandleMovement();
        playerAttack.HandleAttack();
        playerAbilitiesCastingHandler.HandleAbilitiesCasting();

        playerAimDirectionerHandler.HandleAim();
        playerFacingDirectionHandler.HandleFacing();
    }

    private void CombatLogicFixedUpdate()
    {
        playerMovement.ApplyMovement();
    }

    private void CombatLogicLateUpdate()
    {
        playerWeaponAimHandler.HandlePivotRotation();
    }
    #endregion

    #region NoCombat Logics
    private void NoCombatLogicUpdate()
    {
        playerMovement.HandleMovement();

        playerFacingDirectionHandler.HandleFacing();
        playerAimDirectionerHandler.HandleAim();
    }

    private void NoCombatLogicFixedUpdate()
    {
        playerMovement.ApplyMovement();
    }

    private void NoCombatLogicLateUpdate()
    {
        playerWeaponAimHandler.HandlePivotRotation();
    }
    #endregion

    #region Rest Logics
    private void RestLogicUpdate()
    {
        
    }

    private void RestLogicFixedUpdate()
    {
        playerMovement.Stop();
    }

    private void RestLogicLateUpdate()
    {

    }
    #endregion

    private void SetPlayerState(PlayerState state) => playerState = state;
}

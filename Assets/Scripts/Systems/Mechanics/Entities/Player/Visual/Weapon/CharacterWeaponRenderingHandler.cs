using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponRenderingHandler : EntityWeaponRenderingHandler
{
    [Header("Player Components")]
    [SerializeField] private PlayerStateHandler playerStateHandler;

    protected override bool CanRenderWeapon()
    {
        if (!base.CanRenderWeapon()) return false;
        if (!CanRenderWeaponDueToPlayerState()) return false;

        return true;
    }

    private bool CanRenderWeaponDueToPlayerState()
    {
        if (playerStateHandler.State == PlayerStateHandler.PlayerState.Combat) return true;
        if (playerStateHandler.State == PlayerStateHandler.PlayerState.NoCombat) return true;
        if (playerStateHandler.State == PlayerStateHandler.PlayerState.Rest) return true;

        return false;
    }
}

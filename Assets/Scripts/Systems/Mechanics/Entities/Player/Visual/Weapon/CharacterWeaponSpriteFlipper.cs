using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSpriteFlipper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerAimDirectionHandler aimDirectionerHandler;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool facingRight = true;

    private void Update()
    {
        HandleFacingDueToAim();
    }

    private void HandleFacingDueToAim()
    {
        if (aimDirectionerHandler.IsAimingRight())
        {
            CheckFlipRight();
        }

        if (!aimDirectionerHandler.IsAimingRight())
        {
            CheckFlipLeft();
        }
    }

    private void CheckFlipRight()
    {
        if (facingRight) return;

        FlipRight();

        facingRight = true;
    }

    private void CheckFlipLeft()
    {
        if (!facingRight) return;

        FlipLeft();

        facingRight = false;
    }

    private void FlipRight()
    {
        spriteRenderer.flipY = false;
    }

    private void FlipLeft()
    {
        spriteRenderer.flipY = true;
    }
}

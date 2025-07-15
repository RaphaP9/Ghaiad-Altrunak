using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponScaleFlipper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerAimDirectionHandler aimDirectionerHandler;
    [SerializeField] private Transform transformToFlip;

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
        transformToFlip.localScale = new Vector3(1f,1f,1f);
    }

    private void FlipLeft()
    {
        transformToFlip.localScale = new Vector3(1f, -1f, 1f);
    }
}

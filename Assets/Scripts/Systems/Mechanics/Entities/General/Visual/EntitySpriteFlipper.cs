using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpriteFlipper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private EntityFacingDirectionHandler facingDirectionHandler;

    private bool facingRight = true;

    private void Update()
    {
        HandleFlip();
    }

    private void HandleFlip()
    {
        if (facingDirectionHandler.IsFacingRight())
        {
            CheckFlipRight();
        }

        if (!facingDirectionHandler.IsFacingRight())
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
        spriteRenderer.flipX = false;
    }

    private void FlipLeft()
    {
        spriteRenderer.flipX = true;
    }
}

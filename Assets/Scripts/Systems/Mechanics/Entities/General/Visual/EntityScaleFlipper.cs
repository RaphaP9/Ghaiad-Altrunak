using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityScaleFlipper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform transformToFlip;
    [SerializeField] private EntityFacingDirectionHandler facingDirectionHandler;

    private bool facingRight = true;

    private void Update()
    {
        HandleFlipDueToFacing();
    }

    private void HandleFlipDueToFacing()
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
        transformToFlip.localScale = new Vector3(1f, 1f, 1f);
    }

    private void FlipLeft()
    {
        transformToFlip.localScale = new Vector3(-1f, 1f, 1f);
    }

}

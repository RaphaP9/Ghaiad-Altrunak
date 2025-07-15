using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CheckWall : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("General Settings")]
    [SerializeField] private LayerMask wallLayer;

    [Header("Wall Detection Settings")]
    [SerializeField, Range(-0.2f, 1f)] private float rayLength = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool drawRaycasts;

    private Vector2 MoveDirection => MovementInput.Instance.GetLastNonZeroMovementInputNormalized();

    public bool HitWall; //{ get; private set; }

    private void FixedUpdate()
    {
        HitWall = CheckIfWall();
    }

    private bool CheckIfWall()
    {
        bool hitWall = false;

        if (MoveDirection == Vector2.zero) return hitWall;

        Vector2 origin = GeneralUtilities.TransformPositionVector2(transform);

        RaycastHit2D hit = Physics2D.Raycast(origin, MoveDirection, rayLength + circleCollider2D.radius, wallLayer);
        hitWall = hit.collider != null;

        if (drawRaycasts) Debug.DrawRay(origin, MoveDirection, Color.blue);

        return hitWall;
    }
}

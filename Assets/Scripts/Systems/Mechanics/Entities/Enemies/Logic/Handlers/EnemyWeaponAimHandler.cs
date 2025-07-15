using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponAimHandler : EntityWeaponAimHandler
{
    [Header("Player Components")]
    [SerializeField] private PlayerRelativeHandler playerRelativeHandler;

    protected override Vector2 GetTargetPosition() => playerRelativeHandler.PlayerPosition;
}

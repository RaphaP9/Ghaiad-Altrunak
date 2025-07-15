using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAimHandler : EntityWeaponAimHandler
{
    [Header("Player Components")]
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;

    protected override Vector2 GetTargetPosition() => mouseDirectionHandler.Input;
}

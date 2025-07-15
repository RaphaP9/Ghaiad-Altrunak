using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimDirectionHandler : EntityAimDirectionHandler
{
    [Header("Player Components")]
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;

    protected override Vector2 CalculateAimDirection() => mouseDirectionHandler.NormalizedMouseDirection;
    protected override float CalculateAimAngle() => GeneralUtilities.GetVector2AngleDegrees(CalculateAimDirection());
}

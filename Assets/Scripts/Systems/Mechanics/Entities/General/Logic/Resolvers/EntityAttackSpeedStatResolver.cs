using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackSpeedStatResolver : EntityFloatStatResolver
{
    protected override float CalculateStat() => entityIdentifier.EntitySO.baseAttackSpeed;
    protected override float CalculateBaseValue() => entityIdentifier.EntitySO.baseAttackSpeed;
}


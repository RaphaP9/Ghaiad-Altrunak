using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackCritDamageMultiplierStatResolver : EntityFloatStatResolver
{
    protected override float CalculateStat() => entityIdentifier.EntitySO.baseAttackCritDamageMultiplier;
    protected override float CalculateBaseValue() => entityIdentifier.EntitySO.baseAttackCritDamageMultiplier;
}



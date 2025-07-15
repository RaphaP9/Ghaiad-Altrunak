using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackCritChanceStatResolver : EntityFloatStatResolver
{
    protected override float CalculateStat() => entityIdentifier.EntitySO.baseAttackCritChance;
    protected override float CalculateBaseValue() => entityIdentifier.EntitySO.baseAttackCritChance;
}



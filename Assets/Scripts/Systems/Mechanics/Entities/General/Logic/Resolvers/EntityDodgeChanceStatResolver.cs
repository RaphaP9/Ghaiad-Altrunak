using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDodgeChanceStatResolver : EntityFloatStatResolver
{
    protected override float CalculateStat() => entityIdentifier.EntitySO.baseDodgeChance; 
    protected override float CalculateBaseValue() => entityIdentifier.EntitySO.baseDodgeChance;
}


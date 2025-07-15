using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackDamageStatResolver : EntityIntStatResolver
{
    protected override int CalculateStat() => entityIdentifier.EntitySO.baseAttackDamage;
    protected override int CalculateBaseValue() => entityIdentifier.EntitySO.baseAttackDamage;
}


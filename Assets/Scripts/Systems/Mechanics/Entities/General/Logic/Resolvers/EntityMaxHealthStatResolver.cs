using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMaxHealthStatResolver : EntityIntStatResolver
{
    protected override int CalculateStat() => entityIdentifier.EntitySO.baseHealth;
    protected override int CalculateBaseValue() => entityIdentifier.EntitySO.baseHealth;
}

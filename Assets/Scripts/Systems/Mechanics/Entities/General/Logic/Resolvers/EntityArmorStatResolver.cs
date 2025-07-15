using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityArmorStatResolver : EntityIntStatResolver
{
    protected override int CalculateStat() => entityIdentifier.EntitySO.baseArmor;
    protected override int CalculateBaseValue() => entityIdentifier.EntitySO.baseArmor;
}


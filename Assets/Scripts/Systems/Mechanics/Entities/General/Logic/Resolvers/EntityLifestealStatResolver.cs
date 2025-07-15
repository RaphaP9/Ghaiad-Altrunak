using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLifestealStatResolver : EntityFloatStatResolver
{
    protected override float CalculateStat() => entityIdentifier.EntitySO.baseLifesteal; 
    protected override float CalculateBaseValue() => entityIdentifier.EntitySO.baseLifesteal;
}



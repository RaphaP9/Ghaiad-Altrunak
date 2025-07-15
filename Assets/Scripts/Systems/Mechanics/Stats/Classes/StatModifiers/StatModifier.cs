using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatModifier 
{
    public string originGUID;
    public abstract StatValueType GetStatValueType();
}

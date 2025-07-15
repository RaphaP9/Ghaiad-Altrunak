using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoundSO : ScriptableObject
{
    [Header("Settings")]
    public int roundID;
    public DifficultyTier RoundTier;

    public abstract RoundType GetRoundType();
}

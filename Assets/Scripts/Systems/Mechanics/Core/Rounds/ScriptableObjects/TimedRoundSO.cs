using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTimedRoundSO", menuName = "ScriptableObjects/Core/Rounds/TimedRound")]
public class TimedRoundSO : RoundSO
{
    [Header("Timed Round Settings")]
    [Range(10, 120), Tooltip("In Seconds")] public int duration;
    [Range(1f, 10f)] public float baseSpawnInterval;
    public List<ProceduralRoundEnemyGroup> proceduralRoundEnemyGroups;

    public override RoundType GetRoundType() => RoundType.Timed;
}

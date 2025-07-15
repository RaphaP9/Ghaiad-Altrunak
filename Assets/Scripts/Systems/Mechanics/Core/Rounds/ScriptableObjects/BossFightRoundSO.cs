using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossFightRoundSO", menuName = "ScriptableObjects/Core/Rounds/BossFightRound")]
public class BossFightRoundSO : RoundSO
{
    [Header("Boss Fight Round Settings")]
    public EnemySO enemyBoss;
    public override RoundType GetRoundType() => RoundType.BossFight;
}

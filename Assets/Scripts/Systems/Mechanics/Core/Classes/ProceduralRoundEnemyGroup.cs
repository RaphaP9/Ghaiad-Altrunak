using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProceduralRoundEnemyGroup
{
    public EnemySO enemySO;
    [Range(1, 1000)] public int weight;
}

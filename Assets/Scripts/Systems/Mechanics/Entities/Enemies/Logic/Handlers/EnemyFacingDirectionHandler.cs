using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFacingDirectionHandler : EntityFacingDirectionHandler
{
    [Header("Enemy Components")]
    [SerializeField] private EnemySpawnHandler enemySpawnHandler;

    protected override bool CanFace()
    {
        if (!base.CanFace()) return false;
        if(enemySpawnHandler.IsSpawning) return false;

        return true;
    }
}

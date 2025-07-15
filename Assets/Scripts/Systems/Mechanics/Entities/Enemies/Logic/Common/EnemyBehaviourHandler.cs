using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EnemyIdentifier enemyIdentifier;
    [SerializeField] protected EnemySpawnHandler enemySpawnHandler;
    [SerializeField] protected EnemyHealth enemyHealth;
    [SerializeField] protected EnemyMovement enemyMovement;
    [SerializeField] protected EnemyCleanupHandler enemyCleanup;
    [Space]
    [SerializeField] protected EnemyAimDirectionHandler enemyAimDirectionerHandler;
    [SerializeField] protected PlayerRelativeHandler playerRelativeHandler;
    [SerializeField] protected EnemyFacingDirectionHandler enemyFacingDirectionHandler;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform originalStagePoint;
    [SerializeField] private StageSpawnPointsHandler stageSpawnPointsHandler;
    [SerializeField] private PolygonCollider2D stageConfiner;

    public Transform OriginalStagePoint => originalStagePoint;
    public StageSpawnPointsHandler StageSpawnPointsHandler => stageSpawnPointsHandler;
    public PolygonCollider2D StageConfiner => stageConfiner;
}

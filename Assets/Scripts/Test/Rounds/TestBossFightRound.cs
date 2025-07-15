using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossFightRound : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StageSpawnPointsHandler stageSpawnPointsHandler;
    [SerializeField] private BossFightRoundSO bossFightRoundSO;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            BossFightRoundHandler.Instance.StartBossFightRound(bossFightRoundSO, stageSpawnPointsHandler);
        }
    }
}

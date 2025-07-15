using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWavesRound : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StageSpawnPointsHandler stageSpawnPointsHandler;
    [SerializeField] private WavesRoundSO wavesRoundSO;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            WavesRoundHandler.Instance.StartWavesRound(wavesRoundSO, stageSpawnPointsHandler);
        }
    }
}

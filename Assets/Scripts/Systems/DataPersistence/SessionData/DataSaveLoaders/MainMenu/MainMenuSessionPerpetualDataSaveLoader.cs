using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class MainMenuSessionPerpetualDataSaveLoader : MonoBehaviour
{
    public static Func<Task> OnTriggerDataSaveOnMenuStart;

    private static bool hasEnteredGame = false;

    private void Start()
    {
        HandleEnteringGame();
    }

    private void HandleEnteringGame()
    {
        if (hasEnteredGame) return;

        hasEnteredGame = true;

        SessionPerpetualDataContainer.Instance.IncreaseTimesEnteredGame();
        OnTriggerDataSaveOnMenuStart?.Invoke();    
    }
}

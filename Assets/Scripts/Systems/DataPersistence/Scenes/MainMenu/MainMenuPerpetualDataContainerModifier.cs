using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPerpetualDataContainerModifier : MonoBehaviour
{
    private static bool hasEnteredGame = false;

    private void Start()
    {
        HandleEnteringGameDataModification();
    }

    private void HandleEnteringGameDataModification()
    {
        if (hasEnteredGame) return;

        hasEnteredGame = true;

        PerpetualDataContainer.Instance.IncreaseTimesEnteredGame();
        GeneralDataManager.Instance.SavePerpetualJSONDataAsyncWrapper();
    }
}

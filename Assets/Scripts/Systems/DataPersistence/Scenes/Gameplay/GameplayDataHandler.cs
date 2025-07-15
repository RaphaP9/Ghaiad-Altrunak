using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameplayDataHandler : SceneDataHandler
{
    public static GameplayDataHandler Instance {  get; private set; }

    private void OnEnable()
    {
        GameplayRunDataContainerInjectorExtractor.OnTriggerDataSaveOnRoundCompleted += GameplaySessionRunDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;

        GameplayPerpetualDataContainerInjectorExtractor.OnTriggerDataSaveOnRoundCompleted += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;
        GameplayPerpetualDataContainerInjectorExtractor.OnTriggerDataSaveOnTutorialCompleted += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnTutorialCompleted;
        GameplayPerpetualDataContainerInjectorExtractor.OnTriggerDataSaveOnRunCompleted += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunCompleted;
        GameplayPerpetualDataContainerInjectorExtractor.OnTriggerDataSaveOnRunLost += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunLost;

        DevMenuUIButtonsHandler.OnTriggerForceJSONSave += DevMenuUIButtonsHandler_OnTriggerForceJSONSave; 
    }

    private void OnDisable()
    {
        GameplayRunDataContainerInjectorExtractor.OnTriggerDataSaveOnRoundCompleted -= GameplaySessionRunDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;

        GameplayPerpetualDataContainerInjectorExtractor.OnTriggerDataSaveOnRoundCompleted -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;
        GameplayPerpetualDataContainerInjectorExtractor.OnTriggerDataSaveOnTutorialCompleted -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnTutorialCompleted;
        GameplayPerpetualDataContainerInjectorExtractor.OnTriggerDataSaveOnRunCompleted -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunCompleted;
        GameplayPerpetualDataContainerInjectorExtractor.OnTriggerDataSaveOnRunLost -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunLost;

        DevMenuUIButtonsHandler.OnTriggerForceJSONSave -= DevMenuUIButtonsHandler_OnTriggerForceJSONSave;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Run Subscriptions
    private async Task GameplaySessionRunDataSaveLoader_OnTriggerDataSaveOnRoundCompleted()
    {
        await HandleRunDataSave();
    }
    #endregion

    #region Perpetual Subscriptions
    private async Task GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRoundCompleted()
    {
        await HandlePerpetualDataSave();
    }

    private async Task GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnTutorialCompleted()
    {
        await HandlePerpetualDataSave();
    }

    private async Task GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunCompleted()
    {
        await HandlePerpetualDataSave();
    }

    private async Task GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunLost()
    {
        await HandlePerpetualDataSave();
    }
    #endregion

    #region DevMenu Subscriptions
    private async Task DevMenuUIButtonsHandler_OnTriggerForceJSONSave()
    {
        await HandleRunDataSave();
    }
    #endregion
}

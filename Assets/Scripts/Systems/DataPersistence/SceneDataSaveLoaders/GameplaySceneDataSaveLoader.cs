using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameplaySceneDataSaveLoader : SceneDataSaveLoader
{
    public static GameplaySceneDataSaveLoader Instance {  get; private set; }

    private void OnEnable()
    {
        GameplaySessionRunDataSaveLoader.OnTriggerDataSaveOnRoundCompleted += GameplaySessionRunDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;

        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRoundCompleted += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnTutorialCompleted += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnTutorialCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRunCompleted += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRunLost += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunLost;

        DevMenuUIButtonsHandler.OnTriggerForceJSONSave += DevMenuUIButtonsHandler_OnTriggerForceJSONSave; 
    }

    private void OnDisable()
    {
        GameplaySessionRunDataSaveLoader.OnTriggerDataSaveOnRoundCompleted -= GameplaySessionRunDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;

        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRoundCompleted -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnTutorialCompleted -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnTutorialCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRunCompleted -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRunLost -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunLost;

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

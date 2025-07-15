using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MainMenuSceneDataSaveLoader : SceneDataSaveLoader
{
    public static MainMenuSceneDataSaveLoader Instance { get; private set; }

    private void OnEnable()
    {
        MainMenuSessionPerpetualDataSaveLoader.OnTriggerDataSaveOnMenuStart += MainMenuSessionPerpetualDataSaveLoader_OnTriggerDataSaveOnMenuStart;
    }

    private void OnDisable()
    {
        MainMenuSessionPerpetualDataSaveLoader.OnTriggerDataSaveOnMenuStart -= MainMenuSessionPerpetualDataSaveLoader_OnTriggerDataSaveOnMenuStart;
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

    #region Susbscriptions
    private async Task MainMenuSessionPerpetualDataSaveLoader_OnTriggerDataSaveOnMenuStart()
    {
        await HandlePerpetualDataSave();
    }
    #endregion
}
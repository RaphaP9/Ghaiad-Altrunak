using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayPerpetualDataContainerInjectorExtractor : DataContainerInjectorExtractor
{
    [Header("Data Scripts - Already On Scene")]
    [SerializeField] private DialogueTriggerHandler dialogueTriggerHandler;

    //Runtime Filled
    private Transform playerTransform;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    #region Abstract Methods
    public override void InjectAllDataFromDataContainers()
    {
        InjectCharactersPlayedDialogues();
    }

    public override void ExtractAllDataToDataContainers()
    {
        ExtractCharactersPlayedDialogues();
    }
    #endregion

    #region InjectionMethods
    private void InjectCharactersPlayedDialogues()
    {
        if (dialogueTriggerHandler == null) return;
        dialogueTriggerHandler.SetDialoguesPlayed(PerpetualDataContainer.Instance.PerpetualData.dataModeledCharacterDataList);
    }
    #endregion

    #region Extraction Methods
    private void ExtractCharactersPlayedDialogues()
    {
        if (dialogueTriggerHandler == null) return;
        PerpetualDataContainer.Instance.AddCharactersDialoguesPlayed(dialogueTriggerHandler.GetPlayedPrimitiveDialogueGroups());
    }
    #endregion


    #region PlayerSubscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerTransform = e.playerTransform;
    }
    #endregion

    #region Data Update Subscriptions
    private void GameManager_OnDataUpdateOnTutorialCompleted(object sender, System.EventArgs e)
    {
        PerpetualDataContainer.Instance.SetHasCompletedTutorial(true);

        GeneralDataManager.Instance.SavePerpetualJSONDataAsyncWrapper();
    }
    #endregion
}

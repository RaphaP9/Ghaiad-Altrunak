using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GameplayPerpetualDataContainerInjectorExtractor : DataContainerInjectorExtractor
{
    [Header("Data Scripts - Already On Scene")]
    [SerializeField] private DialogueTriggerHandler dialogueTriggerHandler;

    //Runtime Filled
    private Transform playerTransform;

    public static Func<Task> OnTriggerDataSaveOnTutorialCompleted;
    public static Func<Task> OnTriggerDataSaveOnRoundCompleted;
    public static Func<Task> OnTriggerDataSaveOnRunLost;
    public static Func<Task> OnTriggerDataSaveOnRunCompleted;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;

        GameManager.OnDataUpdateOnTutorialCompleted += GameManager_OnDataUpdateOnTutorialCompleted;
        GameManager.OnDataUpdateOnRoundCompleted += GameManager_OnDataUpdateOnRoundCompleted;
        WinManager.OnDataUpdateOnRunCompleted += WinManager_OnDataUpdateOnRunCompleted;
        LoseManager.OnDataUpdateOnRunLost += LoseManager_OnDataUpdateOnRunLost;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;

        GameManager.OnDataUpdateOnTutorialCompleted -= GameManager_OnDataUpdateOnTutorialCompleted;
        GameManager.OnDataUpdateOnRoundCompleted -= GameManager_OnDataUpdateOnRoundCompleted;
        WinManager.OnDataUpdateOnRunCompleted -= WinManager_OnDataUpdateOnRunCompleted;
        LoseManager.OnDataUpdateOnRunLost -= LoseManager_OnDataUpdateOnRunLost;
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
        OnTriggerDataSaveOnTutorialCompleted?.Invoke();
    }

    private void GameManager_OnDataUpdateOnRoundCompleted(object sender, GameManager.OnRoundCompletedEventArgs e)
    {
        ExtractCharactersPlayedDialogues();
        OnTriggerDataSaveOnRoundCompleted?.Invoke();
    }

    private void WinManager_OnDataUpdateOnRunCompleted(object sender, WinManager.OnRunCompletedEventArgs e)
    {
        PerpetualDataContainer.Instance.IncreaseCharacterRunsPlayed(PlayerCharacterManager.Instance.CharacterSO);
        PerpetualDataContainer.Instance.IncreaseCharacterRunsWon(PlayerCharacterManager.Instance.CharacterSO);

        PerpetualDataContainer.Instance.AddUnlockedCharacterIDs(GeneralGameSettings.Instance.GetRunCompletedUnlockedCharacterIDsByCharacterSO(e.characterSO));

        OnTriggerDataSaveOnRunCompleted?.Invoke();
    }

    private void LoseManager_OnDataUpdateOnRunLost(object sender, LoseManager.OnRunLostEventArgs e)
    {
        PerpetualDataContainer.Instance.IncreaseCharacterRunsPlayed(PlayerCharacterManager.Instance.CharacterSO);
        PerpetualDataContainer.Instance.IncreaseCharacterRunsLost(PlayerCharacterManager.Instance.CharacterSO);

        OnTriggerDataSaveOnRunLost?.Invoke();
    }
    #endregion
}

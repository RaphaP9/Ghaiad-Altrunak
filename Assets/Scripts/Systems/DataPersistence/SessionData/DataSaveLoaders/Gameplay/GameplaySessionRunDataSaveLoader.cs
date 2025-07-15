using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GameplaySessionRunDataSaveLoader : SessionDataSaveLoader
{
    [Header("Data Scripts - Already On Scene")]
    [SerializeField] private GameManager gameManager;
    [Space]
    [SerializeField] private PlayerCharacterManager playerCharacterManager;
    [Space]
    [SerializeField] private GeneralStagesManager generalStagesManager;
    [Space]
    [SerializeField] private GoldManager goldManager;   
    [Space]
    [SerializeField] private ObjectsInventoryManager objectsInventoryManager;
    [Space]
    [SerializeField] private TreatsInventoryManager treatsInventoryManager;
    [Space]
    [SerializeField] private RunNumericStatModifierManager runNumericStatModifierManager;

    //Runtime Filled
    private Transform playerTransform;

    public static Func<Task> OnTriggerDataSaveOnRoundCompleted;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;

        GameManager.OnDataUpdateOnRoundCompleted += GameManager_OnDataUpdateOnRoundCompleted;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;

        GameManager.OnDataUpdateOnRoundCompleted -= GameManager_OnDataUpdateOnRoundCompleted;
    }

    #region Abstract Methods

    public override void InjectAllDataFromDataContainers()
    {
        InjectTutorializedRunBoolean();

        InjectCurrentCharacter();

        InjectCurrentStageNumber();
        InjectCurrentRoundNumber();

        InjectCurrentGold();

        InjectPlayerCurrentHealth();
        InjectPlayerCurrentShield();

        InjectObjects();
        InjectTreats();

        InjectRunNumericStats();

        InjectCharacterAbilityLevels();
        InjectCharacterSlotsAbilityVariants();
    }

    public override void ExtractAllDataToDataContainers()
    {
        ExtractTutorializedRunBoolean();

        ExtractPlayerCurrentCharacter();

        ExtractCurrentStageNumber();
        ExtractCurrentRoundNumber();

        ExtractCurrentGold();

        ExtractPlayerCurrentHealth();
        ExtractPlayerCurrentShield();

        ExtractObjects();
        ExtractTreats();

        ExtractRunNumericStats();

        ExtractCharacterAbilityLevels();
        ExtractCharacterSlotsAbilityVariants();
    }

    #endregion

    private void ExtractAllCurrentRoundDataToDataContainers()
    {
        ExtractTutorializedRunBoolean();

        ExtractPlayerCurrentCharacter();

        ExtractCurrentStageNumber();
        ExtractCurrentRoundNumber();

        ExtractCurrentGold();

        ExtractPlayerCurrentHealth();
        ExtractPlayerCurrentShield();

        ExtractObjects();
        ExtractTreats();

        ExtractRunNumericStats();

        ExtractCharacterAbilityLevels();
        ExtractCharacterSlotsAbilityVariants();
    }

    #region Injection Methods
    private void InjectTutorializedRunBoolean()
    {
        if (gameManager == null) return;
        gameManager.SetTutorializedRun(SessionRunDataContainer.Instance.RunData.tutorializedRun);
    }

    private void InjectCurrentStageNumber()
    {
        if (generalStagesManager == null) return;
        generalStagesManager.SetStartingStageNumber(SessionRunDataContainer.Instance.RunData.currentStageNumber);
    }

    private void InjectCurrentRoundNumber()
    {
        if (generalStagesManager == null) return;
        generalStagesManager.SetStartingRoundNumber(SessionRunDataContainer.Instance.RunData.currentRoundNumber);
    }

    private void InjectCurrentGold()
    {
        if (goldManager == null) return;
        goldManager.SetCurrentGold(SessionRunDataContainer.Instance.RunData.currentGold);
    }

    private void InjectCurrentCharacter()
    {
        if (playerCharacterManager == null) return;
        playerCharacterManager.SetCharacterSO(DataUtilities.TranslateCharacterIDToCharacterSO(SessionRunDataContainer.Instance.RunData.currentCharacterID));
    }

    private void InjectObjects()
    {
        if (objectsInventoryManager == null) return;
        objectsInventoryManager.SetObjectsInventory(DataUtilities.TranslateDataModeledObjectsToObjectsIdentified(SessionRunDataContainer.Instance.RunData.objects));
    }

    private void InjectTreats()
    {
        if (treatsInventoryManager == null) return;
        treatsInventoryManager.SetTreatsInventory(DataUtilities.TranslateDataModeledTreatsToTreatsIdentified(SessionRunDataContainer.Instance.RunData.treats));
    }

    private void InjectRunNumericStats()
    {
        if(runNumericStatModifierManager == null) return;
        runNumericStatModifierManager.SetStatList(DataUtilities.TranslateDataModeledNumericStatsToNumericStatModifiers(SessionRunDataContainer.Instance.RunData.numericStats));
    }

    private void InjectPlayerCurrentHealth()
    {
        if(playerTransform == null) return;

        PlayerHealth playerHealth =  playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;   

        playerHealth.SetCurrentHealth(SessionRunDataContainer.Instance.RunData.currentHealth);
    }

    private void InjectPlayerCurrentShield()
    {
        if (playerTransform == null) return;

        PlayerHealth playerHealth = playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;

        playerHealth.SetCurrentShield(SessionRunDataContainer.Instance.RunData.currentShield);
    }

    private void InjectCharacterAbilityLevels()
    {
        if (playerTransform == null) return;

        PlayerAbilitiesLevelsHandler playerAbilityLevelsHandler = playerTransform.GetComponentInChildren<PlayerAbilitiesLevelsHandler>();

        if(playerAbilityLevelsHandler == null) return;

        playerAbilityLevelsHandler.SetStartingAbilityLevels(DataUtilities.TranslateDataModeledAbilityLevelGroupsToPrimitiveAbilityLevelGroups(SessionRunDataContainer.Instance.RunData.abilityLevelGroups));
    }

    private void InjectCharacterSlotsAbilityVariants()
    {
        if (playerTransform == null) return;

        PlayerAbilitySlotsVariantsHandler playerAbilitySlotsVariantsHandler = playerTransform.GetComponentInChildren<PlayerAbilitySlotsVariantsHandler>();

        if(playerAbilitySlotsVariantsHandler == null) return;

        playerAbilitySlotsVariantsHandler.SetStartingAbilityVariants(DataUtilities.TranslateDataModeledAbilitySlotGroupsToPrimitiveAbilitySlotGroups(SessionRunDataContainer.Instance.RunData.abilitySlotGroups));
    }
    #endregion

    #region Extraction Methods
    private void ExtractTutorializedRunBoolean()
    {
        if (gameManager == null) return;
        SessionRunDataContainer.Instance.SetTutorializedRunBoolean(gameManager.TutorializedRun);
    }
    private void ExtractCurrentStageNumber()
    {
        if (generalStagesManager == null) return;
        SessionRunDataContainer.Instance.SetCurrentStageNumber(generalStagesManager.CurrentStageNumber);
    }

    private void ExtractCurrentRoundNumber()
    {
        if (generalStagesManager == null) return;
        SessionRunDataContainer.Instance.SetCurrentRoundNumber(generalStagesManager.CurrentRoundNumber);
    }

    private void ExtractCurrentGold()
    {
        if(goldManager == null) return;
        SessionRunDataContainer.Instance.SetCurrentGold(goldManager.CurrentGold);
    }

    private void ExtractPlayerCurrentCharacter()
    {
        if (playerCharacterManager == null) return;
        SessionRunDataContainer.Instance.SetCurrentCharacterID(playerCharacterManager.CharacterSO.id);
    }

    private void ExtractObjects()
    {
        if (objectsInventoryManager == null) return;
        SessionRunDataContainer.Instance.SetObjects(DataUtilities.TranslateObjectsIdentifiedToDataModeledObjects(objectsInventoryManager.ObjectsInventory));
    }

    private void ExtractTreats()
    {
        if (treatsInventoryManager == null) return;
        SessionRunDataContainer.Instance.SetTreats(DataUtilities.TranslateTreatsIdentifiedToDataModeledTreats(treatsInventoryManager.TreatsInventory));
    }

    private void ExtractRunNumericStats()
    {
        if (runNumericStatModifierManager == null) return;
        SessionRunDataContainer.Instance.SetNumericStats(DataUtilities.TranslateNumericStatModifiersToDataModeledNumericStats(runNumericStatModifierManager.NumericStatModifiers));
    }

    private void ExtractPlayerCurrentHealth()
    {
        if (playerTransform == null) return;

        PlayerHealth playerHealth = playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;

        SessionRunDataContainer.Instance.SetCurrentHealth(playerHealth.CurrentHealth);
    }

    private void ExtractPlayerCurrentShield()
    {
        if (playerTransform == null) return;

        PlayerHealth playerHealth = playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;

        SessionRunDataContainer.Instance.SetCurrentShield(playerHealth.CurrentShield);
    }

    private void ExtractCharacterAbilityLevels()
    {
        if (playerTransform == null) return;

        PlayerAbilitiesLevelsHandler playerAbilityLevelsHandler = playerTransform.GetComponentInChildren<PlayerAbilitiesLevelsHandler>();

        if (playerAbilityLevelsHandler == null) return;

        SessionRunDataContainer.Instance.SetAbilityLevels(DataUtilities.TranslatePrimitiveAbilityLevelGroupsToDataModeledAbilityLevelGroups(playerAbilityLevelsHandler.GetPrimitiveAbilityLevelGroups()));
    }

    private void ExtractCharacterSlotsAbilityVariants()
    {
        if (playerTransform == null) return;

        PlayerAbilitySlotsVariantsHandler playerAbilitySlotsVariantsHandler = playerTransform.GetComponentInChildren<PlayerAbilitySlotsVariantsHandler>();

        if(playerAbilitySlotsVariantsHandler == null) return;

        SessionRunDataContainer.Instance.SetAbilitySlotsVariants(DataUtilities.TranslatePrimitiveAbilitySlotGroupsToDataModeledAbilitySlotGroups(playerAbilitySlotsVariantsHandler.GetPrimitiveAbilitySlotGroups()));
    }
    #endregion


    #region PlayerSubscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerTransform = e.playerTransform;

        InjectPlayerCurrentHealth();
        InjectPlayerCurrentShield();

        InjectCharacterAbilityLevels();
        InjectCharacterSlotsAbilityVariants();
    }
    #endregion

    #region Data Update Subscriptions
    private void GameManager_OnDataUpdateOnRoundCompleted(object sender, GameManager.OnRoundCompletedEventArgs e)
    {
        ExtractAllCurrentRoundDataToDataContainers();
        OnTriggerDataSaveOnRoundCompleted?.Invoke();
    }

    #endregion
}

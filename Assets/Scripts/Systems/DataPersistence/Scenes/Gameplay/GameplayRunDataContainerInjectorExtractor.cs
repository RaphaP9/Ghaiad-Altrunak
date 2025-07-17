using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayRunDataContainerInjectorExtractor : DataContainerInjectorExtractor
{
    [Header("Data Scripts - Already On Scene")]
    [SerializeField] private SeedManager seedManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerCharacterManager playerCharacterManager;
    [SerializeField] private GoldManager goldManager;   
    [SerializeField] private ObjectsInventoryManager objectsInventoryManager;
    [SerializeField] private TreatsInventoryManager treatsInventoryManager;
    [SerializeField] private RunNumericStatModifierManager runNumericStatModifierManager;

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
        InjectRunSeed();

        InjectCurrentLevel();

        InjectCurrentCharacter();

        InjectCurrentGold();

        InjectPlayerCurrentHealth();

        InjectObjects();
        InjectTreats();

        InjectRunNumericStats();

        InjectCharacterAbilityLevels();
        InjectCharacterSlotsAbilityVariants();
    }

    public override void ExtractAllDataToDataContainers()
    {
        ExtractRunSeed();

        ExtractCurrentLevel();

        ExtractPlayerCurrentCharacter();

        ExtractCurrentGold();

        ExtractPlayerCurrentHealth();

        ExtractObjects();
        ExtractTreats();

        ExtractRunNumericStats();

        ExtractCharacterAbilityLevels();
        ExtractCharacterSlotsAbilityVariants();
    }

    #endregion

    #region Injection 
    private void InjectRunSeed()
    {
        if (seedManager == null) return;
        seedManager.SetSeed(RunDataContainer.Instance.RunData.runSeed);
    }

    private void InjectCurrentLevel()
    {
        if (levelManager == null) return;
        levelManager.SetCurrentLevel(RunDataContainer.Instance.RunData.currentLevel);
    }

    private void InjectCurrentCharacter()
    {
        if (playerCharacterManager == null) return;
        playerCharacterManager.SetCharacterSO(DataUtilities.TranslateCharacterIDToCharacterSO(RunDataContainer.Instance.RunData.currentCharacterID));
    }
    private void InjectCurrentGold()
    {
        if (goldManager == null) return;
        goldManager.SetCurrentGold(RunDataContainer.Instance.RunData.currentGold);
    }


    private void InjectObjects()
    {
        if (objectsInventoryManager == null) return;
        objectsInventoryManager.SetObjectsInventory(DataUtilities.TranslateDataModeledObjectsToObjectsIdentified(RunDataContainer.Instance.RunData.objects));
    }

    private void InjectTreats()
    {
        if (treatsInventoryManager == null) return;
        treatsInventoryManager.SetTreatsInventory(DataUtilities.TranslateDataModeledTreatsToTreatsIdentified(RunDataContainer.Instance.RunData.treats));
    }

    private void InjectRunNumericStats()
    {
        if(runNumericStatModifierManager == null) return;
        runNumericStatModifierManager.SetStatList(DataUtilities.TranslateDataModeledNumericStatsToNumericStatModifiers(RunDataContainer.Instance.RunData.numericStats));
    }

    private void InjectPlayerCurrentHealth()
    {
        if(playerTransform == null) return;

        PlayerHealth playerHealth =  playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;   

        playerHealth.SetCurrentHealth(RunDataContainer.Instance.RunData.currentHealth);
    }

    private void InjectCharacterAbilityLevels()
    {
        if (playerTransform == null) return;

        PlayerAbilitiesLevelsHandler playerAbilityLevelsHandler = playerTransform.GetComponentInChildren<PlayerAbilitiesLevelsHandler>();

        if(playerAbilityLevelsHandler == null) return;

        playerAbilityLevelsHandler.SetStartingAbilityLevels(DataUtilities.TranslateDataModeledAbilityLevelGroupsToPrimitiveAbilityLevelGroups(RunDataContainer.Instance.RunData.abilityLevelGroups));
    }

    private void InjectCharacterSlotsAbilityVariants()
    {
        if (playerTransform == null) return;

        PlayerAbilitySlotsVariantsHandler playerAbilitySlotsVariantsHandler = playerTransform.GetComponentInChildren<PlayerAbilitySlotsVariantsHandler>();

        if(playerAbilitySlotsVariantsHandler == null) return;

        playerAbilitySlotsVariantsHandler.SetStartingAbilityVariants(DataUtilities.TranslateDataModeledAbilitySlotGroupsToPrimitiveAbilitySlotGroups(RunDataContainer.Instance.RunData.abilitySlotGroups));
    }
    #endregion

    #region Extraction Methods

    private void ExtractRunSeed()
    {
        if(seedManager == null) return;
        RunDataContainer.Instance.SetRunSeed(seedManager.Seed);
    }

    private void ExtractCurrentLevel()
    {
        if(levelManager == null) return;
        RunDataContainer.Instance.SetCurrentLevel(levelManager.CurrentLevel);
    }

    private void ExtractCurrentGold()
    {
        if(goldManager == null) return;
        RunDataContainer.Instance.SetCurrentGold(goldManager.CurrentGold);
    }

    private void ExtractPlayerCurrentCharacter()
    {
        if (playerCharacterManager == null) return;
        RunDataContainer.Instance.SetCurrentCharacterID(playerCharacterManager.CharacterSO.id);
    }

    private void ExtractObjects()
    {
        if (objectsInventoryManager == null) return;
        RunDataContainer.Instance.SetObjects(DataUtilities.TranslateObjectsIdentifiedToDataModeledObjects(objectsInventoryManager.ObjectsInventory));
    }

    private void ExtractTreats()
    {
        if (treatsInventoryManager == null) return;
        RunDataContainer.Instance.SetTreats(DataUtilities.TranslateTreatsIdentifiedToDataModeledTreats(treatsInventoryManager.TreatsInventory));
    }

    private void ExtractRunNumericStats()
    {
        if (runNumericStatModifierManager == null) return;
        RunDataContainer.Instance.SetNumericStats(DataUtilities.TranslateNumericStatModifiersToDataModeledNumericStats(runNumericStatModifierManager.NumericStatModifiers));
    }

    private void ExtractPlayerCurrentHealth()
    {
        if (playerTransform == null) return;

        PlayerHealth playerHealth = playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;

        RunDataContainer.Instance.SetCurrentHealth(playerHealth.CurrentHealth);
    }

    private void ExtractCharacterAbilityLevels()
    {
        if (playerTransform == null) return;

        PlayerAbilitiesLevelsHandler playerAbilityLevelsHandler = playerTransform.GetComponentInChildren<PlayerAbilitiesLevelsHandler>();

        if (playerAbilityLevelsHandler == null) return;

        RunDataContainer.Instance.SetAbilityLevels(DataUtilities.TranslatePrimitiveAbilityLevelGroupsToDataModeledAbilityLevelGroups(playerAbilityLevelsHandler.GetPrimitiveAbilityLevelGroups()));
    }

    private void ExtractCharacterSlotsAbilityVariants()
    {
        if (playerTransform == null) return;

        PlayerAbilitySlotsVariantsHandler playerAbilitySlotsVariantsHandler = playerTransform.GetComponentInChildren<PlayerAbilitySlotsVariantsHandler>();

        if(playerAbilitySlotsVariantsHandler == null) return;

        RunDataContainer.Instance.SetAbilitySlotsVariants(DataUtilities.TranslatePrimitiveAbilitySlotGroupsToDataModeledAbilitySlotGroups(playerAbilitySlotsVariantsHandler.GetPrimitiveAbilitySlotGroups()));
    }
    #endregion


    #region PlayerSubscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerTransform = e.playerTransform;

        InjectPlayerCurrentHealth();

        InjectCharacterAbilityLevels();
        InjectCharacterSlotsAbilityVariants();
    }
    #endregion
}

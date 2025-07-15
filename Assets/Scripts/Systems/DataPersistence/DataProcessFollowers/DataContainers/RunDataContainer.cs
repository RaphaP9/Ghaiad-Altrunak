using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDataContainer : MonoBehaviour
{
    public static RunDataContainer Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private RunData runData;

    public RunData RunData => runData;

    #region Initialization & Settings
    private void Awake() //Remember this Awake Happens before all JSON awakes, initialization of container happens before JSON Load
    {
        SetSingleton();
        InitializeDataContainer();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void InitializeDataContainer()
    {
        runData = new RunData();
        runData.Initialize();
    }  

    public void SetRunData(RunData runData) => this.runData = runData; 

    public void ResetRunData()
    {
        InitializeDataContainer();
    }
    #endregion

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetTutorializedRunBoolean(bool tutorializedRun) => runData.tutorializedRun = tutorializedRun;

    public void SetCurrentStageNumber(int stageNumber) => runData.currentStageNumber = stageNumber;
    public void SetCurrentRoundNumber(int roundNumber) => runData.currentRoundNumber = roundNumber;

    public void SetCurrentGold(int gold) => runData.currentGold = gold;

    public void SetCurrentCharacterID(int characterID) => runData.currentCharacterID = characterID;

    public void SetCurrentHealth(int currentHealth) => runData.currentHealth = currentHealth;
    public void SetCurrentShield(int currentShield) => runData.currentShield = currentShield;

    public void SetObjects(List<DataModeledObject> dataModeledObjects) => runData.objects = dataModeledObjects;
    public void SetTreats(List<DataModeledTreat> dataModeledTreats) => runData.treats = dataModeledTreats;

    public void SetNumericStats(List<DataModeledNumericStat> dataModeledNumericStats) => runData.numericStats = dataModeledNumericStats;

    public void SetAbilityLevels(List<DataModeledAbilityLevelGroup> dataModeledAbilityLevelGroups) => runData.abilityLevelGroups = dataModeledAbilityLevelGroups;
    public void SetAbilitySlotsVariants(List<DataModeledAbilitySlotGroup> dataModeledAbilitySlotGroups) => runData.abilitySlotGroups = dataModeledAbilitySlotGroups;
}

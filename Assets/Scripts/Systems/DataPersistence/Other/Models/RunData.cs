using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData : DataModel
{
    public int currentCharacterID;
    [Space]
    public int currentGold;
    [Space]
    public int currentHealth;
    [Space]
    public List<DataModeledObject> objects;
    [Space]
    public List<DataModeledTreat> treats;
    [Space]
    public List<DataModeledNumericStat> numericStats;
    [Space]
    public List<DataModeledAbilityLevelGroup> abilityLevelGroups;
    public List<DataModeledAbilitySlotGroup> abilitySlotGroups;

    public RunData()
    {
        currentGold = 0;

        currentCharacterID = 1;

        currentHealth = 0;

        objects = new List<DataModeledObject>();

        treats = new List<DataModeledTreat>();

        numericStats = new List<DataModeledNumericStat>();

        abilityLevelGroups = new List<DataModeledAbilityLevelGroup>();
        abilitySlotGroups = new List<DataModeledAbilitySlotGroup>();
    }

    public override void Initialize()
    {
        if(GeneralGameSettings.Instance == null)
        {
            Debug.Log("GeneralGameSettings Instance is null. Can not Initialize DataModel.");
            return;
        }

        //JSON Perpetual Data Loads Before Run Data (Loaded From JSON into Data Containers)
        //PerpetualData is Ready before RunData, it is safe to call the following line
        currentGold = GeneralGameSettings.Instance.GetStartingGoldQuantity();

        currentCharacterID = GeneralGameSettings.Instance.GetDefaultCharacterID();
    }
}

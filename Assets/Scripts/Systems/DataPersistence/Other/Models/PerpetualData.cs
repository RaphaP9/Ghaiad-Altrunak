using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerpetualData : DataModel
{
    public bool hasCompletedTutorial;
    public int timesEnteredGame;
    public List<int> unlockedCharacterIDs;

    public List<DataModeledCharacterData> dataModeledCharacterDataList;

    public PerpetualData()
    {
        hasCompletedTutorial = false;
        timesEnteredGame = 0;
        unlockedCharacterIDs = new List<int>();
    }

    public override void Initialize()
    {
        if (GeneralGameSettings.Instance == null)
        {
            Debug.Log("GeneralGameSettings Instance is null. Can not Initialize DataModel.");
            return;
        }

        unlockedCharacterIDs = GeneralGameSettings.Instance.GetStartingUnlockedCharacterIDs();
        dataModeledCharacterDataList = InitializeDataModeledCharacterDataList();
    }

    private List<DataModeledCharacterData> InitializeDataModeledCharacterDataList()
    {
        List<DataModeledCharacterData> dataModeledCharacterDataList = new List<DataModeledCharacterData>();

        if (CharacterAssetLibrary.Instance == null) return dataModeledCharacterDataList;

        foreach (CharacterSO characterSO in CharacterAssetLibrary.Instance.Characters)
        {
            DataModeledCharacterData dataModeledCharacterData = new DataModeledCharacterData(characterSO.id); 
            dataModeledCharacterDataList.Add(dataModeledCharacterData);
        }

        return dataModeledCharacterDataList;
    }
}

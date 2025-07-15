using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SessionPerpetualDataContainer : MonoBehaviour
{
    public static SessionPerpetualDataContainer Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private PerpetualData perpetualData;

    public PerpetualData PerpetualData => perpetualData;

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
        perpetualData = new PerpetualData();
        perpetualData.Initialize();
    }

    public void SetPerpetualData(PerpetualData perpetualData) => this.perpetualData = perpetualData;
    
    public void ResetPerpetualData()
    {
        InitializeDataContainer();
    }
    #endregion

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Utility Methods
    public bool HasUnlockedCharacters()
    {
        if (GeneralUtilities.ListsHaveSameContents(perpetualData.unlockedCharacterIDs, GeneralGameSettings.Instance.GetStartingUnlockedCharacterIDs())) return false; 
        return true;
    }

    public DataModeledCharacterData GetDataModeledCharacterDataByCharacterID(int characterID)
    {
        foreach(DataModeledCharacterData dataModeledCharacterData in perpetualData.dataModeledCharacterDataList)
        {
            if(dataModeledCharacterData.characterID == characterID) return dataModeledCharacterData;
        }

        Debug.Log($"Could not find DataModeledCharacterData for CharacterID: {characterID}");
        return null;
    }

    #endregion
    public void SetHasCompletedTutorial(bool hasCompletedTutorial) => perpetualData.hasCompletedTutorial = hasCompletedTutorial;
    public void IncreaseTimesEnteredGame() => perpetualData.timesEnteredGame +=1;

    public void AddUnlockedCharacterIDs(List<int> characterIDs)
    {
        perpetualData.unlockedCharacterIDs.AddRange(characterIDs);
        perpetualData.unlockedCharacterIDs = perpetualData.unlockedCharacterIDs.Distinct().ToList();
    }

    public void IncreaseCharacterRunsPlayed(CharacterSO characterSO)
    {
        DataModeledCharacterData dataModeledCharacterData = GetDataModeledCharacterDataByCharacterID(characterSO.id);
        if (dataModeledCharacterData == null) return;
        dataModeledCharacterData.runsPlayed += 1;
    }

    public void IncreaseCharacterRunsWon(CharacterSO characterSO)
    {
        DataModeledCharacterData dataModeledCharacterData = GetDataModeledCharacterDataByCharacterID(characterSO.id);
        if (dataModeledCharacterData == null) return;
        dataModeledCharacterData.runsWon += 1;
    }

    public void IncreaseCharacterRunsLost(CharacterSO characterSO)
    {
        DataModeledCharacterData dataModeledCharacterData = GetDataModeledCharacterDataByCharacterID(characterSO.id);
        if (dataModeledCharacterData == null) return;
        dataModeledCharacterData.runsLost += 1;
    }

    public void AddCharactersDialoguesPlayed(List<PrimitiveDialogueGroup> primitiveDialogueGroups)
    {
        foreach(PrimitiveDialogueGroup primitiveDialogueGroup in primitiveDialogueGroups)
        {
            AddCharacterDialoguesPlayed(primitiveDialogueGroup);
        }
    }

    private void AddCharacterDialoguesPlayed(PrimitiveDialogueGroup primitiveDialogueGroup)
    {
        DataModeledCharacterData dataModeledCharacterData = GetDataModeledCharacterDataByCharacterID(primitiveDialogueGroup.characterSO.id);
        if (dataModeledCharacterData == null) return;

        if (dataModeledCharacterData.dialoguesPlayedIDs.Contains(primitiveDialogueGroup.id)) return;
        dataModeledCharacterData.dialoguesPlayedIDs.Add(primitiveDialogueGroup.id);
    }

}

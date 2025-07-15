using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralGameSettings : MonoBehaviour
{
    public static GeneralGameSettings Instance { get; private set; }

    [Header("Base Game Settings")]
    [SerializeField] private int startingStage;
    [SerializeField] private int startingRound;
    [Space]
    [SerializeField] private int startingGoldQuantity;
    [Space]
    [SerializeField] private CharacterSO defaultCharacter;

    [Header("Character Unlock Settings")]
    [SerializeField] private List<CharacterSO> startingUnlockedCharacters;
    [SerializeField] private List<RunCompletedCharactersUnlocked> runCompletedCharactersUnlockedList;

    #region Initialization
    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one GeneralGameSettings instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    #endregion

    public int GetStartingGoldQuantity() => startingGoldQuantity;
    public int GetStartingStage() => startingStage;
    public int GetStartingRound() => startingRound;
    public int GetDefaultCharacterID() => defaultCharacter.id;

    public List<int> GetStartingUnlockedCharacterIDs()
    {
        List<int> IDs = new List<int>();

        foreach (CharacterSO character in startingUnlockedCharacters)
        {
            IDs.Add(character.id);
        }

        return IDs;
    }

    public List<int> GetRunCompletedUnlockedCharacterIDsByCharacterSO(CharacterSO characterSO)
    {
        List<CharacterSO> unlockedCharacters = GetRunCompletedUnlockedCharactersByCharacterSO(characterSO);

        List<int> unlockedCharacterIDs = new List<int>();

        foreach(CharacterSO character in unlockedCharacters)
        {
            unlockedCharacterIDs.Add(character.id);
        }

        return unlockedCharacterIDs;
    }

    private List<CharacterSO> GetRunCompletedUnlockedCharactersByCharacterSO(CharacterSO characterSO)
    {
        foreach (RunCompletedCharactersUnlocked element in runCompletedCharactersUnlockedList)
        {
            if (element.characterSO == characterSO) return element.unlockedCharacters;
        }

        return new List<CharacterSO>();
    }
}

[System.Serializable]
public class RunCompletedCharactersUnlocked
{
    public CharacterSO characterSO;
    public List<CharacterSO> unlockedCharacters;
}

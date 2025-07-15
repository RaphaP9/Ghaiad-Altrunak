using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerHandler : MonoBehaviour
{
    public static DialogueTriggerHandler Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<DialogueGroup> dialogueGroups;

    [Header("Debug")]
    [SerializeField] private bool debug;

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
            Debug.LogWarning("There is more than one GameManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }


    #region Public Methods
    public bool ExistDialogueWithConditions(CharacterSO characterSO, int stageNumber, int roundNumber, DialogueChronology dialogueChronology)
    {
        foreach(DialogueGroup dialogueGroup in dialogueGroups)
        {
            if (!dialogueGroup.enabled) continue;
            if (dialogueGroup.hasBeenPlayed && !dialogueGroup.playEvenIfAlreadyPlayed) continue;
            if (dialogueGroup.onlyTutorializedRun && !GameManager.Instance.TutorializedRun) continue;

            if (dialogueGroup.characterSO != characterSO) continue;
            if (dialogueGroup.stageNumber != stageNumber) continue;
            if (dialogueGroup.roundNumber != roundNumber) continue;
            if (dialogueGroup.dialogueChronology != dialogueChronology) continue;

            return true;
        }

        return false;
    }

    public void PlayDialogueWithConditions(CharacterSO characterSO, int stageNumber, int roundNumber, DialogueChronology dialogueChronology)
    {
        DialogueGroup dialogueGroup = FindDialogueGroupWithConditions(characterSO, stageNumber, roundNumber,dialogueChronology);

        if(dialogueGroup == null)
        {
            if (debug) Debug.Log("DialogueGroup was not found. Dialogue Play will be ignored.");
            return;
        }

        if(dialogueGroup.dialogueSO == null)
        {
            if (debug) Debug.Log("DialogueSO is null. Dialogue Play will be ignored.");
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogueGroup.dialogueSO);
        dialogueGroup.hasBeenPlayed = true;
    }

    private DialogueGroup FindDialogueGroupWithConditions(CharacterSO characterSO, int stageNumber, int roundNumber, DialogueChronology dialogueChronology)
    {
        foreach (DialogueGroup dialogueGroup in dialogueGroups)
        {
            if (!dialogueGroup.enabled) continue;

            if (dialogueGroup.characterSO != characterSO) continue;
            if (dialogueGroup.stageNumber != stageNumber) continue;
            if (dialogueGroup.roundNumber != roundNumber) continue;
            if (dialogueGroup.dialogueChronology != dialogueChronology) continue;

            return dialogueGroup;
        }

        return null;
    }
    #endregion

    #region Save/Load Related Methods
    public void SetDialoguesPlayed(List<DataModeledCharacterData> dataModeledCharacterDataList)
    {
        foreach(DataModeledCharacterData dataModeledCharacterData in dataModeledCharacterDataList)
        {
            InjectDialoguesPlayedOfCharacter(dataModeledCharacterData);
        }
    }

    private void InjectDialoguesPlayedOfCharacter(DataModeledCharacterData dataModeledCharacterData)
    {
        if (CharacterAssetLibrary.Instance == null) return;

        CharacterSO characterSO = CharacterAssetLibrary.Instance.GetCharacterSOByID(dataModeledCharacterData.characterID);

        if(characterSO == null)
        {
            if (debug) Debug.Log($"Could not find Character with ID: {dataModeledCharacterData.characterID}. Dialogues Played injection will be ignored.");
            return;
        }

        foreach(DialogueGroup dialogueGroup in dialogueGroups)
        {
            if (characterSO != dialogueGroup.characterSO) continue;
            if (!dataModeledCharacterData.dialoguesPlayedIDs.Contains(dialogueGroup.id)) continue;

            dialogueGroup.hasBeenPlayed = true;
        }
    }

    public List<PrimitiveDialogueGroup> GetPlayedPrimitiveDialogueGroups()
    {
        List<PrimitiveDialogueGroup> primitiveDialogueGroups = new List<PrimitiveDialogueGroup>();

        foreach(DialogueGroup dialogueGroup in dialogueGroups)
        {
            if(!dialogueGroup.hasBeenPlayed) continue;

            PrimitiveDialogueGroup primitiveDialogueGroup = new PrimitiveDialogueGroup { characterSO = dialogueGroup.characterSO, id = dialogueGroup.id, hasBeenPlayed = dialogueGroup.hasBeenPlayed };
            primitiveDialogueGroups.Add(primitiveDialogueGroup);
        }

        return primitiveDialogueGroups;
    }

    #endregion
}
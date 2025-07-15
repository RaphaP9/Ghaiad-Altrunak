using UnityEngine;

[System.Serializable]
public class DialogueGroup
{
    public int id;
    public CharacterSO characterSO;
    public int stageNumber;
    public int roundNumber;
    public DialogueChronology dialogueChronology;
    public DialogueSO dialogueSO;
    [Space]
    public bool enabled;
    public bool onlyTutorializedRun;
    public bool playEvenIfAlreadyPlayed;

    [Header("Runtime Filled")]
    public bool hasBeenPlayed;
}
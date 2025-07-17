using UnityEngine;

[System.Serializable]
public class DialogueGroup
{
    public int id;
    public CharacterSO characterSO;
    public DialogueSO dialogueSO;
    [Space]
    public bool enabled;
    public bool playEvenIfAlreadyPlayed;

    [Header("Runtime Filled")]
    public bool hasBeenPlayed;
}
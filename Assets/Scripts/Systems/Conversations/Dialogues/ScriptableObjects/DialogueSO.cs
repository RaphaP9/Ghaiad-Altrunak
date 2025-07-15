using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueSO", menuName = "ScriptableObjects/Conversations/Dialogues/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public int id;
    public List<DialogueSentence> dialogueSentences;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewDialogueSpeakerSO", menuName = "ScriptableObjects/Conversations/Dialogues/DialogueSpeaker")]
public class DialogueSpeakerSO : ScriptableObject
{
    public int id;
    public string speakerName;
    public Sprite speakerImage;
    public Color nameColor;
}

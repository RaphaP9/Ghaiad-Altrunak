using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewMonologueSpeakerSO", menuName = "ScriptableObjects/Conversations/Monologues/MonologueSpeaker")]
public class MonologueSpeakerSO : ScriptableObject
{
    public int id;
    public string speakerName;
    public Color textColor;
}

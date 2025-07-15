using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonologueSentence
{
    public int localID;
    public MonologueSpeakerSO monologueSpeakerSO;
    [TextArea(3, 10)] public string sentenceText;
    [Space]
    [Range(2f, 10f)] public float time;
    [Space]
    public AudioClip audioClip;
}
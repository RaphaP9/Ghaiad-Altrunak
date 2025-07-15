using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonologueSO", menuName = "ScriptableObjects/Conversations/Monologues/Monologue")]
public class MonologueSO : ScriptableObject
{
    public int id;
    public List<MonologueSentence> monologueSentences;
}
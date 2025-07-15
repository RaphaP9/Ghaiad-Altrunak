using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatInfoSO", menuName = "ScriptableObjects/Stats/StatInfo")]
public class StatInfoSO : ScriptableObject
{
    [Header("Settings")]
    public string statName;
    public Sprite sprite;
    [TextArea(3, 10)] public string description;
}

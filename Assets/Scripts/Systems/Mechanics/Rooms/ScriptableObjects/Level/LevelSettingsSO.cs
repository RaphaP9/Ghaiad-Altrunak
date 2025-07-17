using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettingsSO", menuName = "ScriptableObjects/Game/LevelSettings")]
public class LevelSettingsSO : ScriptableObject
{
    [Header("Identifiers")]
    public int level;

    [Header("Rooms")]
    [Range(1,20)] public int roomsQuantity;
    [Space]
    public List<Transform> roomsPool;

    [Header("Generation")]
    public RoomGenerationStrategySO roomGenerationStrategy;

}

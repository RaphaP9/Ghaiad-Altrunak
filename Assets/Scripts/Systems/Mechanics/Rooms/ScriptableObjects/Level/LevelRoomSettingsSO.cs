using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelRoomSettingsSO", menuName = "ScriptableObjects/Rooms/LevelRoomSettings")]
public class LevelRoomSettingsSO : ScriptableObject
{
    [Header("Identifiers")]
    public int level;

    [Header("Rooms")]
    [Range(10,50)] public int roomsQuantity;
    [Space]
    public Vector2Int roomsGridSize;
    [Space]
    public List<Transform> roomsPool;

    [Header("Generation")]
    public RoomGenerationStrategySO roomGenerationStrategy;
}

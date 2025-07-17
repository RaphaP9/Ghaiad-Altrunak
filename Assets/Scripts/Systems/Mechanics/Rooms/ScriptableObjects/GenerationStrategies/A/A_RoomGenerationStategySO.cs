using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "A_RoomGenerationStategySO", menuName = "ScriptableObjects/Game/RoomGeneration/A_RoomGenerationStategy")]
public class A_RoomGenerationStategySO : RoomGenerationStrategySO
{
    [Header("Settings")]
    [Range(1, 3)] public int bossRooms = 1;
    [Range(1,3)] public int shopRooms = 1;
    [Range(1, 3)] public int treasureRooms = 2;
    [Range(1, 7)] public int eventRooms = 5;
    [Range(1, 5)] public int narrativeRooms = 3;

    [Header("Settings")]
    [Range(0, 2)] public int minRoomDistanceBetweenNonEmptyRooms = 1;
    [Range(0, 3)] public int maxRoomDistanceBetweenNonEmptyRooms = 1;
}

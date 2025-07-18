using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomGenerationStrategySO : ScriptableObject
{
    [Header("Settings")]
    [Range(1, 3)] public int bossRooms = 1;
    [Range(1, 3)] public int shopRooms = 1;
    [Range(1, 3)] public int treasureRooms = 2;
    [Range(1, 7)] public int eventRooms = 5;
    [Range(1, 5)] public int narrativeRooms = 3;

    [Header("Branching")]
    [Range(1, 4)] public int startRoomBranching = 2;
    [Range(1, 4)] public int nonStartRoomBranching = 2;

    [Header("Start Room Positioning")]
    [Range(0f, 1f)] public float startRoomCenteringMinBias;
    [Range(0f, 1f)] public float startRoomCenteringMaxBias;

    [Header("Empty Room Distances")]
    [Range(0, 2)] public int minRoomDistanceBetweenNonEmptyRooms = 1;
    [Range(0, 3)] public int maxRoomDistanceBetweenNonEmptyRooms = 1;

    public float GetStartRoomCenteringBias(System.Random random)
    {
        float startRoomCenteringBias = startRoomCenteringMinBias + (float)random.NextDouble() * (startRoomCenteringMaxBias - startRoomCenteringMinBias);
        return startRoomCenteringBias;
    }

    public int GetRoomDistanceBetweenNonEmptyRooms(System.Random random)
    {
        int roomDistanceBetweenNonEmptyRooms = random.Next(minRoomDistanceBetweenNonEmptyRooms, maxRoomDistanceBetweenNonEmptyRooms);
        return roomDistanceBetweenNonEmptyRooms;
    }
}

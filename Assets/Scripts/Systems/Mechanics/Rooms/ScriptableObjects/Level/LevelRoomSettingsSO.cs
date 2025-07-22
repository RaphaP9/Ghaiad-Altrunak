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
    [Space]
    public List<Transform> doorsPool;

    [Header("Generation")]
    [Range(0, 3)] public int shopRooms = 1;
    [Range(0, 3)] public int treasureRooms = 2;
    [Range(0, 7)] public int eventRooms = 5;
    [Range(0, 5)] public int narrativeRooms = 3;

    [Header("Start Room Positioning")]
    [Range(0f, 1f)] public float startRoomCenteringMinBias;
    [Range(0f, 1f)] public float startRoomCenteringMaxBias;

    [Header("Non 1x1 Candidates")]
    public List<RoomShapeCandidates> roomShapeCandidates;

    public float GetStartRoomCenteringBias(System.Random random)
    {
        float startRoomCenteringBias = startRoomCenteringMinBias + (float)random.NextDouble() * (startRoomCenteringMaxBias - startRoomCenteringMinBias);
        return startRoomCenteringBias;
    }
}

[System.Serializable]
public class RoomShapeCandidates
{
    public RoomShape roomShape;
    [Range(0, 5)] public int targetReplacements;
}

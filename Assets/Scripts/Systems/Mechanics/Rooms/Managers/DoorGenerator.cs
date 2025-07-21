using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGenerator : MonoBehaviour
{
    public static DoorGenerator Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<Transform> doorPool;

    private Dictionary<DoorKey, DoorPositionInfo> doorPositionsByKey = new();
    private Dictionary<DoorKey, DoorAppearanceInfo> doorAppearancesByKey = new();

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one DoorGenerator instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void GenerateDoors(List<RoomInstance> roomInstances)
    {
        PopulateDoorDictionaries(roomInstances);
        LinkDoorPositionAppearances();
    }

    public void PopulateDoorDictionaries(List<RoomInstance> roomInstances)
    {
        doorPositionsByKey.Clear();
        doorAppearancesByKey.Clear();

        foreach (RoomInstance roomInstance in roomInstances)
        {
            RoomData roomData = roomInstance.GetRoomDataComponent();

            foreach (DoorPosition doorPosition in roomData.DoorPositionList)
            {
                Vector2Int worldCell = roomInstance.anchorCell + doorPosition.LeadingLocalCell;
                Direction direction = doorPosition.LeadingDirection;

                DoorKey key = new DoorKey(worldCell, direction);
                doorPositionsByKey[key] = new DoorPositionInfo(roomData, doorPosition);
            }

            foreach (DoorAppearance doorAppearance in roomData.DoorAppearanceList)
            {
                Vector2Int worldCell = roomInstance.anchorCell + doorAppearance.IncomingLocalCell;
                Direction direction = doorAppearance.IncomingDirection;

                DoorKey key = new DoorKey(worldCell, direction);
                doorAppearancesByKey[key] = new DoorAppearanceInfo(roomData, doorAppearance);
            }
        }
    }

    private void LinkDoorPositionAppearances()
    {
        int linkCounter = 0;

        foreach (KeyValuePair<DoorKey, DoorPositionInfo> positionKeyValuePair in doorPositionsByKey)
        {
            DoorKey doorKey = positionKeyValuePair.Key;
            DoorPositionInfo doorPositionInfo = positionKeyValuePair.Value;

            if (doorAppearancesByKey.TryGetValue(doorKey, out DoorAppearanceInfo doorAppearanceInfo))
            {
                linkCounter++;
            }
        }

        Debug.Log($"Found {linkCounter} door links");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGenerator : MonoBehaviour
{
    public static DoorGenerator Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Transform doorsHolder;

    [Header("Runtime Filled")]
    [SerializeField] private List<DoorInstance> doorInstances;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private Dictionary<DoorKey, DoorPositionInfo> doorPositionsByKey = new();
    private Dictionary<DoorKey, DoorAppearanceInfo> doorAppearancesByKey = new();

    private List<DoorInstance> preliminarDoorInstances = new();

    public static event EventHandler<OnDoorsIntantiatedEventArgs> OnDoorsInstantiated;

    public class OnDoorsIntantiatedEventArgs : EventArgs
    {
        public List<DoorInstance> doorInstances;
    }

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

    public void GenerateDoors(List<RoomInstance> roomInstances, LevelRoomSettingsSO levelRoomSettings)
    {
        #region Clear Lists
        doorInstances.Clear();
        preliminarDoorInstances.Clear();
        #endregion

        PopulateDoorDictionaries(roomInstances);
        if(!FillPreliminaryDoorInstances(levelRoomSettings)) return;

        InstantiateDoors();
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
                Vector2Int worldCell = roomInstance.AnchorCell + doorPosition.LeadingLocalCell;
                Direction direction = doorPosition.LeadingDirection;

                DoorKey key = new DoorKey(worldCell, direction);
                doorPositionsByKey[key] = new DoorPositionInfo(roomData, doorPosition);
            }

            foreach (DoorAppearance doorAppearance in roomData.DoorAppearanceList)
            {
                Vector2Int worldCell = roomInstance.AnchorCell + doorAppearance.IncomingLocalCell;
                Direction direction = doorAppearance.IncomingDirection;

                DoorKey key = new DoorKey(worldCell, direction);
                doorAppearancesByKey[key] = new DoorAppearanceInfo(roomData, doorAppearance);
            }
        }
    }

    private bool FillPreliminaryDoorInstances(LevelRoomSettingsSO levelRoomSettingsSO)
    {
        List<DoorInstance> preliminarDoorInstances = new();

        foreach (KeyValuePair<DoorKey, DoorPositionInfo> positionKeyValuePair in doorPositionsByKey)
        {
            DoorKey doorKey = positionKeyValuePair.Key;
            DoorPositionInfo doorPositionInfo = positionKeyValuePair.Value;

            if (doorAppearancesByKey.TryGetValue(doorKey, out DoorAppearanceInfo doorAppearanceInfo))
            {
                Transform foundDoorTransform = RoomUtilities.GetDoorTransformByRoomType(levelRoomSettingsSO.doorsPool, doorAppearanceInfo.IncomingRoomData.RoomType); //DoorTransform is Determined by Incoming Room Type 

                if (foundDoorTransform == null)
                {
                    preliminarDoorInstances.Clear();

                    if (debug) Debug.Log("No door transform was found. Can not generate doors");
                    return false;
                }

                DoorInstance preliminarDoorInstance = new(foundDoorTransform, doorPositionInfo, doorAppearanceInfo, doorKey.Direction);

                preliminarDoorInstances.Add(preliminarDoorInstance);
            }         
        }

        this.preliminarDoorInstances = preliminarDoorInstances;

        return true;
    }

    private void InstantiateDoors()
    {
        foreach (DoorInstance preliminarDoorInstance in preliminarDoorInstances)
        {
            Transform doorInstanceTransform = Instantiate(preliminarDoorInstance.DoorTransform, preliminarDoorInstance.DoorPositionInfo.DoorPosition.DoorPositionTransform.position, Quaternion.identity, doorsHolder);

            if (doorInstanceTransform.TryGetComponent(out DoorData doorData))
            {
                doorData.SetDoorData(preliminarDoorInstance.LeadingDirection, preliminarDoorInstance.DoorPositionInfo.OriginRoomData, preliminarDoorInstance.DoorAppearanceInfo.IncomingRoomData, preliminarDoorInstance.DoorAppearanceInfo.DoorAppearance.AppearanceTransform);
            }

            DoorInstance doorInstance = new(doorInstanceTransform, preliminarDoorInstance.DoorPositionInfo, preliminarDoorInstance.DoorAppearanceInfo, preliminarDoorInstance.LeadingDirection);
            doorInstances.Add(doorInstance);
        }

        OnDoorsInstantiated?.Invoke(this, new OnDoorsIntantiatedEventArgs { doorInstances = doorInstances });
    }
}

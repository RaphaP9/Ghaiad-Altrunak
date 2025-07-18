using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Transform roomsHolder;
    [SerializeField] private GeneralRoomsSettingsSO generalRoomsSettings;

    [Header("Testing")]
    [SerializeField] private Transform randomWalkVisualizationPrefab;

    private Dictionary<Vector2Int, RoomHandler> roomMap = new();

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
            Debug.LogWarning("There is more than one RoomGenerator instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void GenerateRooms()
    {
        LevelRoomSettingsSO levelRoomSettings = generalRoomsSettings.FindLevelSettingsByLevel(LevelManager.Instance.CurrentLevel);

        int roomCount = levelRoomSettings.roomsQuantity;
        Vector2Int roomsGridSize = levelRoomSettings.roomsGridSize;

        HashSet<Vector2Int> randomWalk = RoomsUtilities.GenerateRandomWalk(RoomsUtilities.GetRandomWalkStartingCell(), roomCount, roomsGridSize, SeedManager.Instance.SeededRandom);
        VisualizeGeneratedRandomWalk(randomWalk);
    }

    private void VisualizeGeneratedRandomWalk(HashSet<Vector2Int> randomWalk)
    {
        float XrealRoomSpacing = RoomsUtilities.GetRoomRealSize().x + RoomsUtilities.GetRoomRealSpacing().x;
        float YrealRoomSpacing = RoomsUtilities.GetRoomRealSize().y + RoomsUtilities.GetRoomRealSpacing().y;

        foreach (Vector2Int cell in randomWalk)
        {
            Vector3 worldPos = new Vector3(cell.x * XrealRoomSpacing, cell.y * YrealRoomSpacing, 0f);
            Instantiate(randomWalkVisualizationPrefab, worldPos, Quaternion.identity, roomsHolder);
        }
    }
}

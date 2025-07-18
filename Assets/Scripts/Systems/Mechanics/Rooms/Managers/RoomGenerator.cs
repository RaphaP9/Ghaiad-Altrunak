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
    [SerializeField] private Transform randomWalkVisualizationStartPrefab;

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

    public void GenerateRooms(System.Random random)
    {
        LevelRoomSettingsSO levelRoomSettings = generalRoomsSettings.FindLevelSettingsByLevel(LevelManager.Instance.CurrentLevel);

        int roomCount = levelRoomSettings.roomsQuantity;
        Vector2Int roomsGridSize = levelRoomSettings.roomsGridSize;
        float startRoomCenterBias = levelRoomSettings.roomGenerationStrategy.GetStartRoomCenteringBias(random);

        HashSet<Vector2Int> randomWalk = RoomsUtilities.GenerateRandomWalk(RoomsUtilities.GetRandomWalkStartingCell(), roomCount, roomsGridSize, random);
        Vector2Int startCell = RoomsUtilities.GetBiasedCenteredCell(randomWalk, startRoomCenterBias);

        VisualizeGeneratedRandomWalk(randomWalk, startCell);
    }

    private void VisualizeGeneratedRandomWalk(HashSet<Vector2Int> randomWalk, Vector2Int startCell)
    {
        float XrealRoomSpacing = RoomsUtilities.GetRoomRealSize().x + RoomsUtilities.GetRoomRealSpacing().x;
        float YrealRoomSpacing = RoomsUtilities.GetRoomRealSize().y + RoomsUtilities.GetRoomRealSpacing().y;

        #region StartCell
        Vector3 startCellWorldPos = new Vector3(startCell.x * XrealRoomSpacing, startCell.y * YrealRoomSpacing, 0f);
        Instantiate(randomWalkVisualizationStartPrefab, startCellWorldPos, Quaternion.identity, roomsHolder);
        #endregion

        foreach (Vector2Int cell in randomWalk)
        {
            if(cell == startCell) continue; 

            Vector3 worldPos = new Vector3(cell.x * XrealRoomSpacing, cell.y * YrealRoomSpacing, 0f);
            Instantiate(randomWalkVisualizationPrefab, worldPos, Quaternion.identity, roomsHolder);
        }


    }
}

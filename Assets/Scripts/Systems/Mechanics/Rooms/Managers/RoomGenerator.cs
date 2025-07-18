using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Transform roomsHolder;
    [SerializeField] private GeneralRoomsSettingsSO generalRoomsSettings;

    [Header("Testing")]
    [SerializeField] private Transform testRoomAny;
    [SerializeField] private Transform testRoomStart;
    [SerializeField] private Transform testRoomBoss;

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

        HashSet<Vector2Int> randomWalkCells = RoomUtilities.GenerateRandomWalk(RoomUtilities.GetRandomWalkStartingCell(), roomCount, roomsGridSize, random);

        #region StartCell
        Vector2Int startCell = RoomUtilities.GetBiasedCenteredCell(randomWalkCells, startRoomCenterBias);
        #endregion

        #region DeadEnds
        HashSet<Vector2Int> deadEnds = RoomUtilities.GetDeadEndCells(randomWalkCells);

        //If no real Dead Ends, consider Two Neighbour Cells as Dead Ends
        if(deadEnds.Count <= 0) deadEnds.AddRange(RoomUtilities.GetTwoNeigboursCells(randomWalkCells));

        deadEnds.Remove(startCell); //Remove Start Cell if included in dead ends
        #endregion

        #region BossCell
        Vector2Int bossCell = RoomUtilities.GetFurthestCell(startCell, deadEnds);
        #endregion

        VisualizeGeneratedRandomWalk(randomWalkCells, startCell, bossCell);
    }

    private void VisualizeGeneratedRandomWalk(HashSet<Vector2Int> randomWalk, Vector2Int startCell, Vector2Int bossCell)
    {
        float XrealRoomSpacing = RoomUtilities.GetRoomRealSize().x + RoomUtilities.GetRoomRealSpacing().x;
        float YrealRoomSpacing = RoomUtilities.GetRoomRealSize().y + RoomUtilities.GetRoomRealSpacing().y;

        #region StartCell
        Vector3 startCellWorldPos = new Vector3(startCell.x * XrealRoomSpacing, startCell.y * YrealRoomSpacing, 0f);
        Instantiate(testRoomStart, startCellWorldPos, Quaternion.identity, roomsHolder);
        #endregion

        #region BossCell
        Vector3 bossCellWorldPos = new Vector3(bossCell.x * XrealRoomSpacing, bossCell.y * YrealRoomSpacing, 0f);
        Instantiate(testRoomBoss, bossCellWorldPos, Quaternion.identity, roomsHolder);
        #endregion

        foreach (Vector2Int cell in randomWalk)
        {
            if(cell == startCell) continue; 
            if(cell == bossCell) continue;

            Vector3 worldPos = new Vector3(cell.x * XrealRoomSpacing, cell.y * YrealRoomSpacing, 0f);
            Instantiate(testRoomAny, worldPos, Quaternion.identity, roomsHolder);
        }
    }
}

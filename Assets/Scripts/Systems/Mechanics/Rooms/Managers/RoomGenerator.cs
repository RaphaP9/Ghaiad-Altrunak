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
    [SerializeField] private Transform testRoomEnd;
    [SerializeField] private Transform testRoomShop;

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

        int shopRooms = levelRoomSettings.roomGenerationStrategy.shopRooms;

        HashSet<Vector2Int> totalCells = RoomUtilities.GenerateRandomWalk(RoomUtilities.GetRandomWalkStartingCell(), roomCount, roomsGridSize, random);
        HashSet<Vector2Int> nonAsignedCells = new HashSet<Vector2Int>(totalCells); //Can not assign because HashSet is Refference Type!

        #region StartCell
        Vector2Int startCell = RoomUtilities.GetBiasedCenteredCell(totalCells, startRoomCenterBias);
        nonAsignedCells.Remove(startCell);
        #endregion

        #region EndCell
        HashSet<Vector2Int> deadEndsForEndCell = RoomUtilities.GetProcessedDeadEndCells(nonAsignedCells, totalCells, random);
        Vector2Int endCell = RoomUtilities.GetFurthestCell(deadEndsForEndCell, startCell);
        nonAsignedCells.Remove(endCell);
        #endregion

        #region ShopCells
        //Shop is not necessarily a Dead End, Only the furthest room from boss out of non assigned cells

        HashSet<Vector2Int> shopCells = new();
        HashSet<Vector2Int> shopCellsGenerationRefferences = new HashSet<Vector2Int>{ startCell, endCell };

        for (int i = 0; i < shopRooms; i++)
        {
            Vector2Int shopCell = RoomUtilities.GetFurthestCell(nonAsignedCells, shopCellsGenerationRefferences);

            shopCells.Add(shopCell);
            shopCellsGenerationRefferences.Add(shopCell);
            nonAsignedCells.Remove(shopCell);
        }
        #endregion

        VisualizeGeneratedRooms(nonAsignedCells, startCell, endCell, shopCells);
    }

    private void VisualizeGeneratedRooms(HashSet<Vector2Int> nonAssignedCells, Vector2Int startCell, Vector2Int endCell, HashSet<Vector2Int> shopCells)
    {
        float XrealRoomSpacing = RoomUtilities.GetRoomRealSize().x + RoomUtilities.GetRoomRealSpacing().x;
        float YrealRoomSpacing = RoomUtilities.GetRoomRealSize().y + RoomUtilities.GetRoomRealSpacing().y;

        #region StartCell
        Vector3 startCellWorldPos = new Vector3(startCell.x * XrealRoomSpacing, startCell.y * YrealRoomSpacing, 0f);
        Instantiate(testRoomStart, startCellWorldPos, Quaternion.identity, roomsHolder);
        #endregion

        #region EndCell
        Vector3 endCellWorldPos = new Vector3(endCell.x * XrealRoomSpacing, endCell.y * YrealRoomSpacing, 0f);
        Instantiate(testRoomEnd, endCellWorldPos, Quaternion.identity, roomsHolder);
        #endregion

        #region ShopCells
        foreach (Vector2Int cell in shopCells)
        {
            Vector3 worldPos = new Vector3(cell.x * XrealRoomSpacing, cell.y * YrealRoomSpacing, 0f);
            Instantiate(testRoomShop, worldPos, Quaternion.identity, roomsHolder);
        }
        #endregion

        #region NonAssignedCells
        foreach (Vector2Int cell in nonAssignedCells)
        {
            Vector3 worldPos = new Vector3(cell.x * XrealRoomSpacing, cell.y * YrealRoomSpacing, 0f);
            Instantiate(testRoomAny, worldPos, Quaternion.identity, roomsHolder);
        }
        #endregion
    }
}

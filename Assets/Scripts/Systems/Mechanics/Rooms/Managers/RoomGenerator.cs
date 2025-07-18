using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Transform testRoomTreasure;
    [SerializeField] private Transform testRoomNarrative;
    [SerializeField] private Transform testRoomEvent;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const int MAX_NUMBER_OF_GENERATION_ITERATIONS = 5;

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

    public void GenerateRooms(System.Random seededRandom)
    {
        #region Initialize Variables & HashSets
        bool generationSucceeded = false;
        int generationIterationsCount = 0;

        HashSet<Vector2Int> totalCells = new();
        HashSet<Vector2Int> nonAssignedCells = new();

        Vector2Int startCell = Vector2Int.zero;
        Vector2Int endCell = Vector2Int.zero;

        HashSet<Vector2Int> shopCells = new();
        HashSet<Vector2Int> treasureCells = new();
        HashSet<Vector2Int> narrativeCells = new();
        HashSet<Vector2Int> eventCells = new();
        #endregion

        while (!generationSucceeded)
        {
            if(generationIterationsCount > MAX_NUMBER_OF_GENERATION_ITERATIONS)
            {
                Debug.LogWarning("Could not find a valid cell Layout. Can not generate Rooms");
                return;
            }

            generationIterationsCount++;

            #region Clear Cell HashSets
            totalCells.Clear();
            nonAssignedCells.Clear();
            startCell = Vector2Int.zero;
            endCell = Vector2Int.zero;
            shopCells.Clear();
            treasureCells.Clear();
            narrativeCells.Clear();
            eventCells.Clear();
            #endregion

            #region Cell Assignation Logic
            LevelRoomSettingsSO levelRoomSettings = generalRoomsSettings.FindLevelSettingsByLevel(LevelManager.Instance.CurrentLevel);

            int roomCount = levelRoomSettings.roomsQuantity;
            Vector2Int roomsGridSize = levelRoomSettings.roomsGridSize;
            float startRoomCenterBias = levelRoomSettings.GetStartRoomCenteringBias(seededRandom);

            int shopRooms = levelRoomSettings.shopRooms;
            int treasureRooms = levelRoomSettings.treasureRooms;
            int narrativeRooms = levelRoomSettings.narrativeRooms;
            int eventRooms = levelRoomSettings.eventRooms;

            totalCells = RoomUtilities.GenerateRandomWalk(RoomUtilities.GetRandomWalkStartingCell(), roomCount, roomsGridSize, seededRandom);
            nonAssignedCells = new(totalCells); //Can not assign because HashSet is Refference Type!

            #region StartCell
            startCell = RoomUtilities.GetBiasedCenteredCell(totalCells, startRoomCenterBias);
            nonAssignedCells.Remove(startCell);
            #endregion

            #region EndCell
            HashSet<Vector2Int> deadEndsForEndCell = RoomUtilities.GetProcessedDeadEndCells(nonAssignedCells, totalCells, seededRandom);
            endCell = RoomUtilities.GetFurthestCell(deadEndsForEndCell, startCell);
            nonAssignedCells.Remove(endCell);
            #endregion

            #region ShopCells
            //Shop is not necessarily a Dead End, Only the furthest room from boss out of non assigned cells
            HashSet<Vector2Int> shopCellsGenerationRefferences = new HashSet<Vector2Int> { startCell, endCell };

            for (int i = 0; i < shopRooms; i++)
            {
                Vector2Int shopCell = RoomUtilities.GetFurthestCell(nonAssignedCells, shopCellsGenerationRefferences);
                //Vector2Int shopCell = RoomUtilities.GetFurthestCell(RoomUtilities.GetProcessedDeadEndCells(nonAsignedCells, totalCells, random), shopCellsGenerationRefferences);

                shopCells.Add(shopCell);
                shopCellsGenerationRefferences.Add(shopCell);
                nonAssignedCells.Remove(shopCell);
            }
            #endregion

            #region TreasureCells
            HashSet<Vector2Int> treasureCellsGenerationRefferences = new HashSet<Vector2Int> { startCell, endCell };
            treasureCellsGenerationRefferences.AddRange(shopCells);

            for (int i = 0; i < treasureRooms; i++)
            {
                Vector2Int treasureCell = RoomUtilities.GetFurthestCell(nonAssignedCells, treasureCellsGenerationRefferences);

                treasureCells.Add(treasureCell);
                treasureCellsGenerationRefferences.Add(treasureCell);
                nonAssignedCells.Remove(treasureCell);
            }
            #endregion

            #region Narrative Cells
            HashSet<Vector2Int> forbiddenNarrativeRooms = new() { startCell, endCell };
            HashSet<Vector2Int> forbiddenNarrativeNeighbors = RoomUtilities.Get4DirectionalCellsNeighbors(forbiddenNarrativeRooms);

            forbiddenNarrativeRooms.AddRange(forbiddenNarrativeNeighbors);

            HashSet<Vector2Int> narrativeCellsPool = new(nonAssignedCells);
            narrativeCellsPool.ExceptWith(forbiddenNarrativeRooms);

            for (int i = 0; i < narrativeRooms; i++)
            {
                if (narrativeCellsPool.Count <= 0) break; //Break if no cells in pool (will not generate desired narrativeRooms count)

                Vector2Int narrativeCell = RoomUtilities.GetRandomCellFromPool(narrativeCellsPool, seededRandom);

                narrativeCells.Add(narrativeCell);
                narrativeCellsPool.Remove(narrativeCell);
                nonAssignedCells.Remove(narrativeCell);

                //Narrative Rooms Can not be next to each other
                HashSet<Vector2Int> newNarrativeForbiddenNeighbors = RoomUtilities.Get4DirectionalCellNeighbors(narrativeCell);
                narrativeCellsPool.ExceptWith(newNarrativeForbiddenNeighbors);
            }

            #endregion

            #region Event Cells
            HashSet<Vector2Int> forbiddenEventRooms = new() { startCell, endCell };
            HashSet<Vector2Int> forbiddenEventNeighbors = RoomUtilities.Get4DirectionalCellsNeighbors(forbiddenEventRooms);

            forbiddenEventRooms.AddRange(forbiddenEventNeighbors);

            HashSet<Vector2Int> eventCellsPool = new(nonAssignedCells);
            eventCellsPool.ExceptWith(forbiddenEventRooms);

            for (int i = 0; i < eventRooms; i++)
            {
                if (eventCellsPool.Count <= 0) break; //Break if no cells in pool (will not generate desired narrativeRooms count)

                Vector2Int eventCell = RoomUtilities.GetRandomCellFromPool(eventCellsPool, seededRandom);

                eventCells.Add(eventCell);
                eventCellsPool.Remove(eventCell);
                nonAssignedCells.Remove(eventCell);

                HashSet<Vector2Int> newEventForbiddenNeighbors = RoomUtilities.Get4DirectionalCellNeighbors(eventCell);
                eventCellsPool.ExceptWith(newEventForbiddenNeighbors);
            }

            #endregion

            #endregion

            #region Cell Layout Validation

            #region AllCellsUniqueValidation

            List<Vector2Int> allCellsList = new();

            allCellsList.Add(startCell);
            allCellsList.Add(endCell);
            allCellsList.AddRange(shopCells);
            allCellsList.AddRange(treasureCells);
            allCellsList.AddRange(narrativeCells);
            allCellsList.AddRange(eventCells);
            allCellsList.AddRange(nonAssignedCells);

            if(!RoomUtilities.IsEveryCellUnique(allCellsList))
            {
                GeneralUtilities.RandomizeRandom(seededRandom);
                continue;
            }

            #endregion

            generationSucceeded = true;

            #endregion
        }

        if (debug) Debug.Log($"Found a Valid Cell Layout in {generationIterationsCount} Iteration(s)");

        VisualizeGeneratedRooms(nonAssignedCells, startCell, endCell, shopCells, treasureCells, narrativeCells, eventCells);
    }

    private void VisualizeGeneratedRooms(HashSet<Vector2Int> nonAssignedCells, Vector2Int startCell, Vector2Int endCell, HashSet<Vector2Int> shopCells, HashSet<Vector2Int> treasureCells, HashSet<Vector2Int> narrativeCells, HashSet<Vector2Int> eventCells)
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

        #region TreasureCells
        foreach (Vector2Int cell in treasureCells)
        {
            Vector3 worldPos = new Vector3(cell.x * XrealRoomSpacing, cell.y * YrealRoomSpacing, 0f);
            Instantiate(testRoomTreasure, worldPos, Quaternion.identity, roomsHolder);
        }
        #endregion

        #region NarrativeCells
        foreach (Vector2Int cell in narrativeCells)
        {
            Vector3 worldPos = new Vector3(cell.x * XrealRoomSpacing, cell.y * YrealRoomSpacing, 0f);
            Instantiate(testRoomNarrative, worldPos, Quaternion.identity, roomsHolder);
        }
        #endregion

        #region EventCells
        foreach (Vector2Int cell in eventCells)
        {
            Vector3 worldPos = new Vector3(cell.x * XrealRoomSpacing, cell.y * YrealRoomSpacing, 0f);
            Instantiate(testRoomEvent, worldPos, Quaternion.identity, roomsHolder);
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

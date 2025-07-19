using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Transform roomsHolder;
    [SerializeField] private GeneralRoomsSettingsSO generalRoomsSettings;

    [Header("Runtime Filled")]
    [SerializeField] private List<PlacedRoom> placedRooms;

    [Header("Testing")]
    [SerializeField] private Transform testRoomUnassigned;
    [SerializeField] private Transform testRoomStart;
    [SerializeField] private Transform testRoomEnd;
    [SerializeField] private Transform testRoomShop;
    [SerializeField] private Transform testRoomTreasure;
    [SerializeField] private Transform testRoomNarrative;
    [SerializeField] private Transform testRoomEvent;
    [Space]
    [SerializeField] private Transform testRoomHorizontal2x1;
    [SerializeField] private Transform testRoomVertical1x2;
    [SerializeField] private Transform testRoomSquare2x2;
    [SerializeField] private Transform testRoomLShapedA;
    [SerializeField] private Transform testRoomLShapedB;
    [SerializeField] private Transform testRoomLShapedC;
    [SerializeField] private Transform testRoomLShapedD;

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
        placedRooms.Clear();

        #region Initialize Variables & HashSets
        System.Random localRandom = GeneralUtilities.RandomizeRandomByRandom(seededRandom); //Can not clone the OG random but whatever

        LevelRoomSettingsSO levelRoomSettings = generalRoomsSettings.FindLevelSettingsByLevel(LevelManager.Instance.CurrentLevel);

        int roomCount = levelRoomSettings.roomsQuantity;
        Vector2Int roomsGridSize = levelRoomSettings.roomsGridSize;
        float startRoomCenterBias = levelRoomSettings.GetStartRoomCenteringBias(localRandom);

        int shopRooms = levelRoomSettings.shopRooms;
        int treasureRooms = levelRoomSettings.treasureRooms;
        int narrativeRooms = levelRoomSettings.narrativeRooms;
        int eventRooms = levelRoomSettings.eventRooms;

        List<RoomShapeCandidates> roomShapeCandidatesList = levelRoomSettings.roomShapeCandidates;

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

        #region Preliminar Cell Mapping

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
            totalCells = RoomUtilities.GenerateRandomWalk(RoomUtilities.GetRandomWalkStartingCell(), roomCount, roomsGridSize, localRandom);
            nonAssignedCells = new(totalCells); //Can not assign because HashSet is Refference Type!

            #region StartCell
            startCell = RoomUtilities.GetBiasedCenteredCell(totalCells, startRoomCenterBias);
            nonAssignedCells.Remove(startCell);
            #endregion

            #region EndCell
            HashSet<Vector2Int> deadEndsForEndCell = RoomUtilities.GetProcessedDeadEndCells(nonAssignedCells, totalCells, localRandom);
            endCell = RoomUtilities.GetFurthestCell(deadEndsForEndCell, startCell);
            nonAssignedCells.Remove(endCell);
            #endregion

            #region ShopCells
            //Shop is not necessarily a Dead End, Only the furthest room from boss out of non assigned cells
            HashSet<Vector2Int> shopCellsGenerationRefferences = new HashSet<Vector2Int> { startCell, endCell };

            for (int i = 0; i < shopRooms; i++)
            {
                if(nonAssignedCells.Count <= 0) break; //Break if no cells remaining in nonAssignedCells (will not generate desired narrativeRooms count)

                Vector2Int shopCell = RoomUtilities.GetFurthestCell(nonAssignedCells, shopCellsGenerationRefferences);
                //Vector2Int shopCell = RoomUtilities.GetFurthestCell(RoomUtilities.GetProcessedDeadEndCells(nonAsignedCells, totalCells, random), shopCellsGenerationRefferences);

                shopCells.Add(shopCell);
                shopCellsGenerationRefferences.Add(shopCell);
                nonAssignedCells.Remove(shopCell);
            }
            #endregion

            #region TreasureCells
            HashSet<Vector2Int> treasureCellsGenerationRefferences = new HashSet<Vector2Int> { startCell, endCell };
            treasureCellsGenerationRefferences.UnionWith(shopCells);

            for (int i = 0; i < treasureRooms; i++)
            {
                if (nonAssignedCells.Count <= 0) break; //Break if no cells remaining in nonAssignedCells (will not generate desired narrativeRooms count)

                Vector2Int treasureCell = RoomUtilities.GetFurthestCell(nonAssignedCells, treasureCellsGenerationRefferences);

                treasureCells.Add(treasureCell);
                treasureCellsGenerationRefferences.Add(treasureCell);
                nonAssignedCells.Remove(treasureCell);
            }
            #endregion

            #region Narrative Cells
            HashSet<Vector2Int> forbiddenNarrativeRooms = new() { startCell, endCell };
            HashSet<Vector2Int> forbiddenNarrativeNeighbors = RoomUtilities.Get4DirectionalCellsNeighbors(forbiddenNarrativeRooms);

            forbiddenNarrativeRooms.UnionWith(forbiddenNarrativeNeighbors);

            HashSet<Vector2Int> narrativeCellsPool = new(nonAssignedCells);
            narrativeCellsPool.ExceptWith(forbiddenNarrativeRooms);

            for (int i = 0; i < narrativeRooms; i++)
            {
                if (narrativeCellsPool.Count <= 0) break; //Break if no cells in pool (will not generate desired narrativeRooms count)

                Vector2Int narrativeCell = RoomUtilities.GetRandomCellFromPool(narrativeCellsPool, localRandom);

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

            forbiddenEventRooms.UnionWith(forbiddenEventNeighbors);

            HashSet<Vector2Int> eventCellsPool = new(nonAssignedCells);
            eventCellsPool.ExceptWith(forbiddenEventRooms);

            for (int i = 0; i < eventRooms; i++)
            {
                if (eventCellsPool.Count <= 0) break; //Break if no cells in pool (will not generate desired narrativeRooms count)

                Vector2Int eventCell = RoomUtilities.GetRandomCellFromPool(eventCellsPool, localRandom);

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
                GeneralUtilities.RandomizeRandomByRandom(localRandom);
                continue;
            }

            #endregion

            generationSucceeded = true;

            #endregion
        }

        if (debug) Debug.Log($"Found a Valid Cell Layout in {generationIterationsCount} Iteration(s)");

        #endregion

        #region First Placed Room List Population

        //All Special Rooms are 1x1 Single Cell

        placedRooms.Add(new PlacedRoom(startCell, RoomType.Start, RoomShape.SingleCell));
        placedRooms.Add(new PlacedRoom(endCell, RoomType.End, RoomShape.SingleCell));

        foreach (Vector2Int shopCell in shopCells)
        {
            placedRooms.Add(new PlacedRoom(shopCell, RoomType.Shop, RoomShape.SingleCell));
        }

        foreach (Vector2Int treasureCell in treasureCells)
        {
            placedRooms.Add(new PlacedRoom(treasureCell, RoomType.Treasure, RoomShape.SingleCell));
        }

        foreach (Vector2Int narrativeCell in narrativeCells)
        {
            placedRooms.Add(new PlacedRoom(narrativeCell, RoomType.Narrative, RoomShape.SingleCell));
        }

        foreach (Vector2Int eventCell in eventCells)
        {
            placedRooms.Add(new PlacedRoom(eventCell, RoomType.Event, RoomShape.SingleCell));
        }
        #endregion

        #region Non 1x1 Dimensional Cell Candidate Replacement
        List<PlacedRoomPrimitive> replacementPlacedRooms = new List<PlacedRoomPrimitive>();

        HashSet<Vector2Int> untouchableCells = new HashSet<Vector2Int>();
        untouchableCells.Add(startCell);
        untouchableCells.Add(endCell);
        untouchableCells.UnionWith(shopCells);
        untouchableCells.UnionWith(treasureCells);
        untouchableCells.UnionWith(narrativeCells);
        untouchableCells.UnionWith(eventCells);

        HashSet<Vector2Int> replacementCandidatesPool = new HashSet<Vector2Int>(nonAssignedCells);

        foreach(RoomShapeCandidates roomShapeCandidates in roomShapeCandidatesList)
        {
            replacementCandidatesPool = RoomUtilities.ShuffleCells(replacementCandidatesPool, localRandom);

            RoomShape roomShape = roomShapeCandidates.roomShape;
            int targetCount = roomShapeCandidates.targetReplacements;
            int placedCount = 0;

            foreach (Vector2Int anchorCell in replacementCandidatesPool)
            {
                if (placedCount >= targetCount) break;

                HashSet<Vector2Int> desiredOccupiedCells = RoomUtilities.GetShapeOccupiedCells(anchorCell, roomShape);

                bool hasConflict = desiredOccupiedCells.Any(cell => untouchableCells.Contains(cell));

                if (hasConflict) continue;

                untouchableCells.UnionWith(desiredOccupiedCells);
                nonAssignedCells.ExceptWith(desiredOccupiedCells);

                replacementPlacedRooms.Add(new PlacedRoomPrimitive(anchorCell, roomShape));
                placedRooms.Add(new PlacedRoom(anchorCell, RoomType.Transition, roomShape)); //Populate Placed Rooms List on Iteration, All are of Transition Room Type
                placedCount++;
            }
        }
        #endregion

        #region Second Placed Room List Population

        //All Remaining Non Assigned Cells are 1x1 Single Cell Transition Type Rooms

        foreach (Vector2Int nonAssignedCell in nonAssignedCells)
        {
            placedRooms.Add(new PlacedRoom(nonAssignedCell, RoomType.Transition, RoomShape.SingleCell));
        }
        #endregion

        VisualizeGeneratedRooms(nonAssignedCells, startCell, endCell, shopCells, treasureCells, narrativeCells, eventCells, replacementPlacedRooms);
    }

    private void VisualizeGeneratedRooms(HashSet<Vector2Int> nonAssignedCells, Vector2Int startCell, Vector2Int endCell, HashSet<Vector2Int> shopCells, HashSet<Vector2Int> treasureCells, HashSet<Vector2Int> narrativeCells, HashSet<Vector2Int> eventCells, List<PlacedRoomPrimitive> replacementPlacedRooms)
    {
        float XrealRoomSpacing = RoomUtilities.GetRoomRealSize().x;
        float YrealRoomSpacing = RoomUtilities.GetRoomRealSize().y;

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

        #region ReplacementNonAssignedCells
        foreach (PlacedRoomPrimitive replacementRoom in replacementPlacedRooms)
        {
            Transform prefab = replacementRoom.roomShape switch
            {
                RoomShape.Horizontal2x1 => testRoomHorizontal2x1,
                RoomShape.Vertical1x2 => testRoomVertical1x2,
                RoomShape.Square2x2 => testRoomSquare2x2,
                RoomShape.LShapedA => testRoomLShapedA,
                RoomShape.LShapedB => testRoomLShapedB,
                RoomShape.LShapedC => testRoomLShapedC,
                RoomShape.LShapedD => testRoomLShapedD,
                _ => testRoomUnassigned,
            };

            Vector3 worldPos = new Vector3(replacementRoom.anchorCell.x * XrealRoomSpacing, replacementRoom.anchorCell.y * YrealRoomSpacing, 0f);
            Instantiate(prefab, worldPos, Quaternion.identity, roomsHolder);
        }
        #endregion

        #region NonAssignedCells
        foreach (Vector2Int cell in nonAssignedCells)
        {
            Vector3 worldPos = new Vector3(cell.x * XrealRoomSpacing, cell.y * YrealRoomSpacing, 0f);
            Instantiate(testRoomUnassigned, worldPos, Quaternion.identity, roomsHolder);
        }
        #endregion
    }
}

[System.Serializable]
public class PlacedRoom
{
    public Vector2Int anchorCell;
    public RoomType roomType;
    public RoomShape roomShape;

    public PlacedRoom(Vector2Int anchorCell,RoomType roomType, RoomShape roomShape)
    {
        this.anchorCell = anchorCell;
        this.roomType = roomType;
        this.roomShape = roomShape;
    }
}

public class PlacedRoomPrimitive
{
    public Vector2Int anchorCell;
    public RoomShape roomShape;

    public PlacedRoomPrimitive(Vector2Int anchorCell, RoomShape roomShape)
    {
        this.anchorCell = anchorCell;
        this.roomShape = roomShape;
    }
}


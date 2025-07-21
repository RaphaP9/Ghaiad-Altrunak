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

    [Header("Doors Pool")]
    [SerializeField] private List<Transform> doorsPool;

    [Header("Runtime Filled")]
    [SerializeField] private List<PreliminarRoom> preliminarRooms;
    [SerializeField] private List<RoomInstance> roomInstances;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const int MAX_NUMBER_OF_GENERATION_ITERATIONS = 5;

    private List<RoomInstance> preliminaryRoomInstances;

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
        #region Clear Lists & Containers
        LevelRoomSettingsSO levelRoomSettings = FindLevelRoomSettings();

        preliminarRooms.Clear();
        roomInstances.Clear();

        #endregion

        if (!FillPreliminarRooms(seededRandom, levelRoomSettings)) return;
        if(!FillPreliminaryRoomInstances(seededRandom,levelRoomSettings)) return;

        InstantiateRooms();
        InstantiateDoors();
    }

    private LevelRoomSettingsSO FindLevelRoomSettings() => generalRoomsSettings.FindLevelSettingsByLevel(LevelManager.Instance.CurrentLevel);

    private bool FillPreliminarRooms(System.Random seededRandom, LevelRoomSettingsSO levelRoomSettings)
    {
        #region Initialize Variables & HashSets
        System.Random localRandom = GeneralUtilities.RandomizeRandomByRandom(seededRandom); //Can not clone the OG random but whatever

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
                return false;
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
            RoomUtilities.GenerateRandomWalkNonAlloc(RoomUtilities.GetRandomWalkStartingCell(), roomCount, roomsGridSize, localRandom, totalCells);
            nonAssignedCells = new(totalCells); //Can not assign because HashSet is Refference Type!

            #region StartCell
            RoomUtilities.GetBiasedCenteredCellNonAlloc(totalCells, startRoomCenterBias, ref startCell);
            nonAssignedCells.Remove(startCell);
            #endregion

            #region EndCell
            HashSet<Vector2Int> deadEndsForEndCell = RoomUtilities.GetProcessedDeadEndCells(nonAssignedCells, totalCells, localRandom);
            RoomUtilities.GetFurthestCellNonAlloc(deadEndsForEndCell, startCell, ref endCell);
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
                HashSet<Vector2Int> newNarrativeForbiddenNeighbors = RoomUtilities.Get4DirectionalCellNeighbors(narrativeCell); //No need to create or use a NonAlloc Method (for loop does very few iterations)
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

                HashSet<Vector2Int> newEventForbiddenNeighbors = RoomUtilities.Get4DirectionalCellNeighbors(eventCell); //No need to create or use a NonAlloc Method (for loop does very few iterations)
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

        #region First Preliminar Room List Population

        //All Special Rooms are 1x1 Single Cell

        preliminarRooms.Add(new PreliminarRoom(startCell, RoomType.Start, RoomShape.SingleCell, new List<Vector2Int> {startCell}));
        preliminarRooms.Add(new PreliminarRoom(endCell, RoomType.End, RoomShape.SingleCell, new List<Vector2Int> { endCell }));

        foreach (Vector2Int shopCell in shopCells)
        {
            preliminarRooms.Add(new PreliminarRoom(shopCell, RoomType.Shop, RoomShape.SingleCell, new List<Vector2Int> { shopCell }));
        }

        foreach (Vector2Int treasureCell in treasureCells)
        {
            preliminarRooms.Add(new PreliminarRoom(treasureCell, RoomType.Treasure, RoomShape.SingleCell, new List<Vector2Int> { treasureCell }));
        }

        foreach (Vector2Int narrativeCell in narrativeCells)
        {
            preliminarRooms.Add(new PreliminarRoom(narrativeCell, RoomType.Narrative, RoomShape.SingleCell, new List<Vector2Int> { narrativeCell }));
        }

        foreach (Vector2Int eventCell in eventCells)
        {
            preliminarRooms.Add(new PreliminarRoom(eventCell, RoomType.Event, RoomShape.SingleCell, new List<Vector2Int> { eventCell }));
        }
        #endregion

        #region Non 1x1 Dimensional Cell Candidate Replacement
        HashSet<Vector2Int> untouchableCells = new HashSet<Vector2Int>();
        untouchableCells.Add(startCell);
        untouchableCells.Add(endCell);
        untouchableCells.UnionWith(shopCells);
        untouchableCells.UnionWith(treasureCells);
        untouchableCells.UnionWith(narrativeCells);
        untouchableCells.UnionWith(eventCells);

        HashSet<Vector2Int> replacementCandidatesPool = new HashSet<Vector2Int>(nonAssignedCells);

        HashSet<Vector2Int> desiredOccupiedCellsBuffer = new();

        foreach (RoomShapeCandidates roomShapeCandidates in roomShapeCandidatesList)
        {
            replacementCandidatesPool = RoomUtilities.ShuffleCellsHashSet(replacementCandidatesPool, localRandom);

            RoomShape roomShape = roomShapeCandidates.roomShape;
            int targetCount = roomShapeCandidates.targetReplacements;
            int replacedCount = 0;

            foreach (Vector2Int anchorCell in replacementCandidatesPool)
            {
                if (replacedCount >= targetCount) break;
                if (untouchableCells.Contains(anchorCell)) continue; //Better to check here if we are adding cells to untouchableCells

                RoomUtilities.GetShapeOccupiedCellsNonAlloc(anchorCell, roomShape, desiredOccupiedCellsBuffer);

                if (desiredOccupiedCellsBuffer.Overlaps(untouchableCells)) continue;

                untouchableCells.UnionWith(desiredOccupiedCellsBuffer);
                nonAssignedCells.ExceptWith(desiredOccupiedCellsBuffer);

                List<Vector2Int> occupiedCells = new(desiredOccupiedCellsBuffer);

                preliminarRooms.Add(new PreliminarRoom(anchorCell, RoomType.Regular, roomShape, occupiedCells)); //Populate Preliminar Rooms List on Iteration, All are of Transition Room Type
                replacedCount++;
            }
        }
        #endregion

        #region Second Preliminar Room List Population

        //All Remaining Non Assigned Cells are 1x1 Single Cell Transition Type Rooms

        foreach (Vector2Int nonAssignedCell in nonAssignedCells)
        {
            preliminarRooms.Add(new PreliminarRoom(nonAssignedCell, RoomType.Regular, RoomShape.SingleCell, new List<Vector2Int> { nonAssignedCell }));
        }
        #endregion

        return true;
    }
    private bool FillPreliminaryRoomInstances(System.Random seededRandom, LevelRoomSettingsSO levelRoomSettingsSO)
    {
        List<Transform> totalRoomsTransformPool = new(levelRoomSettingsSO.roomsPool); //Create another list - we are going to make some shuffles
        List<Transform> remainingUniqueRoomsPool = new(levelRoomSettingsSO.roomsPool);

        List<RoomInstance> preliminaryRoomInstances = new();

        foreach (PreliminarRoom preliminarRoom in preliminarRooms)
        {
            totalRoomsTransformPool = RoomUtilities.ShuffleTransformsList(totalRoomsTransformPool, seededRandom);
            remainingUniqueRoomsPool = RoomUtilities.ShuffleTransformsList(remainingUniqueRoomsPool, seededRandom);

            Transform foundRoomTransform = RoomUtilities.GetDifferentRoomTransformFromPoolByPreliminarRoom(totalRoomsTransformPool, remainingUniqueRoomsPool, preliminarRoom);

            if(foundRoomTransform == null)
            {
                preliminaryRoomInstances.Clear();

                if (debug) Debug.Log("No room transform was found. Can not generate rooms");
                return false;
            }

            remainingUniqueRoomsPool.Remove(foundRoomTransform);

            RoomInstance preliminaryRoomInstance = new(foundRoomTransform, preliminarRoom.anchorCell, preliminarRoom.occupiedCells );
            preliminaryRoomInstances.Add(preliminaryRoomInstance);
        }

        this.preliminaryRoomInstances = preliminaryRoomInstances;

        return true;
    }

    private void InstantiateRooms()
    {
        float XrealRoomSpacing = RoomUtilities.GetRoomRealSize().x;
        float YrealRoomSpacing = RoomUtilities.GetRoomRealSize().y;

        foreach (RoomInstance preliminarRoomInstance in preliminaryRoomInstances)
        {
            Vector3 roomWorldPos = new Vector3(preliminarRoomInstance.anchorCell.x * XrealRoomSpacing, preliminarRoomInstance.anchorCell.y * YrealRoomSpacing, 0f);
            Transform roomInstanceTransform = Instantiate(preliminarRoomInstance.roomTransform, roomWorldPos, Quaternion.identity, roomsHolder);

            if(roomInstanceTransform.TryGetComponent(out RoomData roomHandler))
            {
                roomHandler.SetAnchorCell(preliminarRoomInstance.anchorCell);
                roomHandler.SetOccupiedCells(preliminarRoomInstance.occupiedCells);
            }

            RoomInstance roomInstance = new(roomInstanceTransform, preliminarRoomInstance.anchorCell, preliminarRoomInstance .occupiedCells);
            roomInstances.Add(roomInstance);
        }
    }

    private void InstantiateDoors()
    {
        foreach (RoomInstance roomInstance in roomInstances)
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RoomUtilities
{
    private const int X_RANDOM_WALK_STARTING_CELL = 0;
    private const int Y_RANDOM_WALK_STARTING_CELL = 0;

    private const float X_ROOM_REAL_SIZE = 16f;
    private const float Y_ROOM_REAL_SIZE = 9f;

    private const int RANDOM_WALK_STUCK_COUNT_THRESHOLD = 5;


    private static readonly Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private static readonly Dictionary<RoomShape, HashSet<Vector2Int>> roomShapeLocalCellOccupations = new()
    {
        { RoomShape.SingleCell, new() { new Vector2Int(0, 0) } },
        { RoomShape.Horizontal2x1, new() { new Vector2Int(0, 0), new Vector2Int(1, 0) } },
        { RoomShape.Vertical1x2, new() { new Vector2Int(0, 0), new Vector2Int(0, 1) } },
        { RoomShape.Square2x2, new() {new Vector2Int(0, 0), new Vector2Int(1, 0),new Vector2Int(0, 1), new Vector2Int(1, 1)}},
        { RoomShape.LShapedA, new() {new Vector2Int(0, 0), new Vector2Int(1, 0),new Vector2Int(0, 1)}},
        { RoomShape.LShapedB, new() {new Vector2Int(0, 0), new Vector2Int(1, 0),new Vector2Int(1, 1)}},
        { RoomShape.LShapedC, new() {new Vector2Int(0, 0), new Vector2Int(-1, 0),new Vector2Int(0, -1)}},
        { RoomShape.LShapedD, new() {new Vector2Int(0, 0), new Vector2Int(-1, 0),new Vector2Int(-1, -1)}},
    };

    private static readonly HashSet<Vector2Int> singeCellFallback = new() { Vector2Int.zero };

    public static Vector2Int GetRandomWalkStartingCell() => new Vector2Int(X_RANDOM_WALK_STARTING_CELL, Y_RANDOM_WALK_STARTING_CELL);
    public static Vector2 GetRoomRealSize() => new Vector2(X_ROOM_REAL_SIZE, Y_ROOM_REAL_SIZE);

    #region Random
    public static Vector2Int GetRandomCellFromPool(HashSet<Vector2Int> cellPool, System.Random random)
    {
        int index = random.Next(cellPool.Count);

        foreach (Vector2Int cell in cellPool)
        {
            if (index-- == 0) return cell;
        }

        return Vector2Int.zero;
    }

    public static HashSet<Vector2Int> ShuffleCells(HashSet<Vector2Int> cells, System.Random random)
    {
        return new HashSet<Vector2Int>(cells.OrderBy(_ => random.NextDouble()));
    }
    #endregion

    #region Directions & Neighbors
    public static Vector2Int GetRandomDirection(System.Random random)
    {
        return directions[random.Next(0, directions.Length)];
    }

    public static HashSet<Vector2Int> Get4DirectionalCellNeighbors(Vector2Int cell)
    {
        HashSet<Vector2Int> neighborCells = new();

        foreach(Vector2Int direction in directions)
        {
            neighborCells.Add(cell + direction);
        }

        return neighborCells;
    }

    public static HashSet<Vector2Int> Get4DirectionalCellsNeighbors(HashSet<Vector2Int> cells)
    {
        HashSet<Vector2Int> neighborCells = new();

        foreach(Vector2Int cell in cells)
        {
            HashSet<Vector2Int> cellNeighbors = Get4DirectionalCellNeighbors(cell);
            neighborCells.UnionWith(cellNeighbors);
        }

        return neighborCells;
    }
    #endregion

    #region Random Walk
    public static HashSet<Vector2Int> GenerateRandomWalk(Vector2Int startCell, int steps, Vector2Int gridSize, System.Random random)
    {
        HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int>();
        Vector2Int currentCell = startCell;
        visitedCells.Add(currentCell);

        int maxCellsInGrid = gridSize.x * gridSize.y;
        int stepsTaken = 0;
        int stuckCount = 0;

        while (stepsTaken < steps - 1)
        {
            Vector2Int next = currentCell + GetRandomDirection(random);

            int halfWidth = gridSize.x / 2;
            int halfHeight = gridSize.y / 2;

            if (Mathf.Abs(next.x) > halfWidth || Mathf.Abs(next.y) > halfHeight || visitedCells.Contains(next)) //If out of grid or already visited
            {
                stuckCount++;

                if (stuckCount >= RANDOM_WALK_STUCK_COUNT_THRESHOLD)
                {
                    currentCell = GetRandomCellFromPool(visitedCells, random);
                    stuckCount = 0;
                }

                continue;
            }

            currentCell = next;
            visitedCells.Add(currentCell);
            stepsTaken++;

            if (visitedCells.Count >= maxCellsInGrid)
            {
                Debug.LogWarning($"Requested {steps} Steps, but the Grid can only fit {maxCellsInGrid} unique cells. Walk will be truncated to fill the grid.");
                break;
            }
        }

        return visitedCells;
    }

    public static void GenerateRandomWalkNonAlloc(Vector2Int startCell, int steps, Vector2Int gridSize, System.Random random, HashSet<Vector2Int> cellsContainer, bool clearContainer = true)
    {
        if(clearContainer) cellsContainer.Clear();

        Vector2Int currentCell = startCell;
        cellsContainer.Add(currentCell);

        int maxCellsInGrid = gridSize.x * gridSize.y;
        int stepsTaken = 0;
        int stuckCount = 0;

        while (stepsTaken < steps - 1)
        {
            Vector2Int next = currentCell + GetRandomDirection(random);

            int halfWidth = gridSize.x / 2;
            int halfHeight = gridSize.y / 2;

            if (Mathf.Abs(next.x) > halfWidth || Mathf.Abs(next.y) > halfHeight || cellsContainer.Contains(next)) //If out of grid or already visited
            {
                stuckCount++;

                if (stuckCount >= RANDOM_WALK_STUCK_COUNT_THRESHOLD)
                {
                    currentCell = GetRandomCellFromPool(cellsContainer, random);
                    stuckCount = 0;
                }

                continue;
            }

            currentCell = next;
            cellsContainer.Add(currentCell);
            stepsTaken++;

            if (cellsContainer.Count >= maxCellsInGrid)
            {
                Debug.LogWarning($"Requested {steps} Steps, but the Grid can only fit {maxCellsInGrid} unique cells. Walk will be truncated to fill the grid.");
                break;
            }
        }
    }
    #endregion

    #region RoomGeneration
    //Get the Closest Cell to Average Cells Position (bias = 0) or furthest to Average Cells Position (bias = 1)
    public static Vector2Int GetBiasedCenteredCell(HashSet<Vector2Int> cells, float bias)
    {
        bias = Mathf.Clamp01(bias);

        #region GetAverage
        Vector2 average = Vector2.zero;
        foreach (Vector2Int cell in cells)
        {
            average += (Vector2)cell;
        }

        average /= cells.Count;
        #endregion

        #region Order By Distance to Average
        var sortedCells = cells.Select(cell => new {Cell = cell, DistanceToAverageCenter = ((Vector2)cell - average).sqrMagnitude}).OrderBy(item => item.DistanceToAverageCenter).ToList();
        #endregion

        #region Interpolate to Find Desired Element Index
        int index = Mathf.FloorToInt(bias * (sortedCells.Count - 1));
        #endregion

        return sortedCells[index].Cell;
    }

    public static void GetBiasedCenteredCellNonAlloc(HashSet<Vector2Int> cells, float bias, ref Vector2Int cellContainer)
    {
        bias = Mathf.Clamp01(bias);

        #region GetAverage
        Vector2 average = Vector2.zero;
        foreach (Vector2Int cell in cells)
        {
            average += (Vector2)cell;
        }

        average /= cells.Count;
        #endregion

        #region Order By Distance to Average
        List<Vector2Int> sortedCells = cells.ToList();

        sortedCells.Sort((a, b) =>
        {
            float distanceCompareeA = ((Vector2)a - average).sqrMagnitude;
            float distanceCompareeB = ((Vector2)b - average).sqrMagnitude;
            return distanceCompareeA.CompareTo(distanceCompareeB);
        });
        #endregion

        #region Interpolate to Find Desired Element Index
        int index = Mathf.FloorToInt(bias * (sortedCells.Count - 1));
        #endregion

        cellContainer = sortedCells[index];
    }

    //Get Furthest Cell To Origin from a Group Of Cells
    public static Vector2Int GetFurthestCell(HashSet<Vector2Int> cells, Vector2Int refferenceCell)
    {
        Vector2Int furthestCell = refferenceCell;
        float maxDistanceSqr = float.MinValue;

        foreach (Vector2Int cell in cells)
        {
            float distanceSqr = (cell - refferenceCell).sqrMagnitude; //Cast as Vector2 If more precission needed (Slower)

            if (distanceSqr > maxDistanceSqr)
            {
                maxDistanceSqr = distanceSqr;
                furthestCell = cell;
            }
        }

        return furthestCell;
    }

    public static void GetFurthestCellNonAlloc(HashSet<Vector2Int> cells, Vector2Int refferenceCell, ref Vector2Int cellContainer)
    {
        cellContainer = refferenceCell;
        float maxDistanceSqr = float.MinValue;

        foreach (Vector2Int cell in cells)
        {
            float distanceSqr = (cell - refferenceCell).sqrMagnitude; //Cast as Vector2 If more precission needed (Slower)

            if (distanceSqr > maxDistanceSqr)
            {
                maxDistanceSqr = distanceSqr;
                cellContainer = cell;
            }
        }
    }

    public static Vector2Int GetFurthestCell(HashSet<Vector2Int> cellPool, HashSet<Vector2Int> refferenceCells)
    {
        Vector2Int furthestCell = refferenceCells.First();
        float bestMinDistanceToARefference = float.MinValue;

        foreach (Vector2Int cell in cellPool)
        {
            float minDistanceToRefference = float.MaxValue;

            foreach (Vector2Int referenceCell in refferenceCells)
            {
                float distanceToRefference = (cell - referenceCell).sqrMagnitude; //Cast as Vector2 If more precission needed (Slower)

                if (distanceToRefference < minDistanceToRefference) minDistanceToRefference = distanceToRefference;
            }

            if (minDistanceToRefference > bestMinDistanceToARefference)
            {
                bestMinDistanceToARefference = minDistanceToRefference;
                furthestCell = cell;
            }
        }

        return furthestCell;
    }

    public static void GetFurthestCellNonAlloc(HashSet<Vector2Int> cellPool, HashSet<Vector2Int> refferenceCells, ref Vector2Int cellContainer)
    {
        cellContainer = refferenceCells.First();
        float bestMinDistanceToARefference = float.MinValue;

        foreach (Vector2Int cell in cellPool)
        {
            float minDistanceToRefference = float.MaxValue;

            foreach (Vector2Int referenceCell in refferenceCells)
            {
                float distanceToRefference = (cell - referenceCell).sqrMagnitude; //Cast as Vector2 If more precission needed (Slower)

                if (distanceToRefference < minDistanceToRefference) minDistanceToRefference = distanceToRefference;
            }

            if (minDistanceToRefference > bestMinDistanceToARefference)
            {
                bestMinDistanceToARefference = minDistanceToRefference;
                cellContainer = cell;
            }
        }
    }

    //Get Cells With Exactly 1,2,3,4 neighbor cells (In Priority Order)
    public static HashSet<Vector2Int> GetProcessedDeadEndCells(HashSet<Vector2Int> cellsPool, HashSet<Vector2Int> checkPool, System.Random random)
    {
        HashSet<Vector2Int> oneNeighborCells = GetCellsWithNNeightbors(cellsPool, checkPool,1);

        if(oneNeighborCells.Count >= 1) return ShuffleCells(oneNeighborCells, random);

        HashSet<Vector2Int> twoNeighborCells = GetCellsWithNNeightbors(cellsPool, checkPool,2);

        if (twoNeighborCells.Count >= 1) return ShuffleCells(twoNeighborCells, random);

        HashSet<Vector2Int> threeNeighborCells = GetCellsWithNNeightbors(cellsPool, checkPool,3);

        if (threeNeighborCells.Count >= 1) return ShuffleCells(threeNeighborCells, random);

        return ShuffleCells(GetCellsWithNNeightbors(cellsPool, checkPool, 4), random);
    }

    //Get Cells with N neightbors
    public static HashSet<Vector2Int> GetCellsWithNNeightbors(HashSet<Vector2Int> cellsPool, HashSet<Vector2Int> neighborCheckCellsPool, int neightborCount)
    {
        HashSet<Vector2Int> cellsWithNeighbors = new();

        foreach (Vector2Int cell in cellsPool)
        {
            int neighborCount = 0;

            foreach (Vector2Int direction in directions)
            {
                if (neighborCheckCellsPool.Contains(cell + direction)) neighborCount++;
            }

            if (neighborCount == neightborCount) cellsWithNeighbors.Add(cell);
        }

        return cellsWithNeighbors;
    }
    #endregion

    #region Validation
    public static bool IsEveryCellUnique(List<Vector2Int> cellsList)
    {
        HashSet<Vector2Int> uniqueCells = new();

        foreach (Vector2Int cell in cellsList)
        {
            if (!uniqueCells.Add(cell)) return false; //If can not add to HashSet (Because is already contained there), return false     
        }

        return true;
    }
    #endregion

    #region RoomShape
    public static HashSet<Vector2Int> GetRoomShapeLocalCellOccupations(this RoomShape shape)
    {
        if (roomShapeLocalCellOccupations.TryGetValue(shape, out var localOccupations)) return localOccupations;

        Debug.LogWarning($"Room Shape '{shape}' not found. Returning single-cell fallback (0,0).");
        return singeCellFallback;
    }

    public static HashSet<Vector2Int> GetShapeOccupiedCells(Vector2Int anchorCell, RoomShape shape)
    {
        HashSet<Vector2Int> realCellOccupations = new();
        HashSet<Vector2Int> localCellOccupations = shape.GetRoomShapeLocalCellOccupations();

        foreach (Vector2Int localCellOccupation in localCellOccupations)
        {
            realCellOccupations.Add(anchorCell + localCellOccupation);
        }

        return realCellOccupations;
    }

    public static void GetShapeOccupiedCellsNonAlloc(Vector2Int anchorCell, RoomShape shape, HashSet<Vector2Int> occupiedCellsContainer, bool clearContainer = true)
    {
        if (clearContainer) occupiedCellsContainer.Clear();

        HashSet<Vector2Int> localCellOccupations = shape.GetRoomShapeLocalCellOccupations();

        foreach (Vector2Int localCellOccupation in localCellOccupations)
        {
            occupiedCellsContainer.Add(anchorCell + localCellOccupation);
        }
    }
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomsUtilities
{
    private const int X_RANDOM_WALK_STARTING_CELL = 0;
    private const int Y_RANDOM_WALK_STARTING_CELL = 0;

    private const float X_ROOM_REAL_SIZE = 16f;
    private const float Y_ROOM_REAL_SIZE = 9f;

    private const float X_ROOM_REAL_SPACING = 0f;
    private const float Y_ROOM_REAL_SPACING = 0f;

    private const int RANDOM_WALK_STUCK_COUNT_THRESHOLD = 5;

    public static Vector2Int GetRandomWalkStartingCell() => new Vector2Int(X_RANDOM_WALK_STARTING_CELL, Y_RANDOM_WALK_STARTING_CELL);
    public static Vector2 GetRoomRealSize() => new Vector2(X_ROOM_REAL_SIZE, Y_ROOM_REAL_SIZE);
    public static Vector2 GetRoomRealSpacing() => new Vector2(X_ROOM_REAL_SPACING, Y_ROOM_REAL_SPACING);

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
            if (visitedCells.Count >= maxCellsInGrid)
            {
                Debug.LogWarning($"Requested {steps} Steps, but the Grid can only fit {maxCellsInGrid} unique cells. Walk will be truncated to fill the grid.");
                break;
            }

            Vector2Int next = currentCell + GetRandomDirection(random);

            if (Mathf.Abs(next.x) > gridSize.x / 2 || Mathf.Abs(next.y) > gridSize.y / 2 || visitedCells.Contains(next)) //If out of grid or already visited
            {
                stuckCount++;

                if (stuckCount >= RANDOM_WALK_STUCK_COUNT_THRESHOLD)
                {
                    currentCell = GetRandomVisitedCell(visitedCells, random);
                    stuckCount = 0;
                }

                continue;
            }

            currentCell = next;
            visitedCells.Add(currentCell);
            stepsTaken++;
        }

        return visitedCells;
    }

    public static Vector2Int GetRandomDirection(System.Random random)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
        };

        return directions[random.Next(0, directions.Length)];
    }

    public static Vector2Int GetRandomVisitedCell(HashSet<Vector2Int> visitedCells, System.Random random)
    {
        int index = random.Next(visitedCells.Count);

        foreach (Vector2Int cell in visitedCells)
        {
            if (index-- == 0) return cell;
        }

        return Vector2Int.zero; 
    }
    #endregion

}

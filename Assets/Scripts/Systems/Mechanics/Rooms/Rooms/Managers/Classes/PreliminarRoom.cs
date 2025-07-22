using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreliminarRoom
{
    public Vector2Int AnchorCell { get; private set; }
    public RoomType RoomType {  get; private set; }
    public RoomShape RoomShape { get; private set; }
    public List<Vector2Int> OccupiedCells { get; private set; } //Only List to visualize on inspector, otherwise can be HashSet

    public PreliminarRoom(Vector2Int anchorCell, RoomType roomType, RoomShape roomShape, List<Vector2Int> occupiedCells)
    {
        AnchorCell = anchorCell;
        RoomType = roomType;
        RoomShape = roomShape;
        OccupiedCells = occupiedCells;
    }
}
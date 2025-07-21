using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PreliminarRoom
{
    public Vector2Int anchorCell;
    public RoomType roomType;
    public RoomShape roomShape;
    public List<Vector2Int> occupiedCells; //Only List to visualize on inspector, otherwise can be HashSet

    public PreliminarRoom(Vector2Int anchorCell, RoomType roomType, RoomShape roomShape, List<Vector2Int> occupiedCells)
    {
        this.anchorCell = anchorCell;
        this.roomType = roomType;
        this.roomShape = roomShape;
        this.occupiedCells = occupiedCells;
    }
}
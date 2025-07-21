using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInstance
{
    public Transform roomTransform;
    public Vector2Int anchorCell;
    public List<Vector2Int> occupiedCells; //Only List to visualize on inspector, otherwise can be HashSet

    public RoomInstance(Transform roomTransform, Vector2Int anchorCell, List<Vector2Int> occupiedCells)
    {
        this.roomTransform = roomTransform;
        this.anchorCell = anchorCell;
        this.occupiedCells = occupiedCells;
    }
}
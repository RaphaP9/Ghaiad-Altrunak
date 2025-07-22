using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInstance
{
    public Transform RoomTransform {  get; private set; }
    public Vector2Int AnchorCell {  get; private set; }
    public List<Vector2Int> OccupiedCells { get; private set; } //Only List to visualize on inspector, otherwise can be HashSet

    public RoomInstance(Transform roomTransform, Vector2Int anchorCell, List<Vector2Int> occupiedCells)
    {
        RoomTransform = roomTransform;
        AnchorCell = anchorCell;
        OccupiedCells = occupiedCells;
    }

    public RoomData GetRoomDataComponent()
    {
        if (RoomTransform.TryGetComponent(out RoomData roomData)) return roomData;
        return null;
    }
}
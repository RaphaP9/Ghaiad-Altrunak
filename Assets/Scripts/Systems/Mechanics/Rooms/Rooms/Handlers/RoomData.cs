using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [Header("Room Data")]
    [SerializeField] private int id;
    [SerializeField] private RoomDificulty roomDificulty;
    [SerializeField] private RoomType roomType;
    [SerializeField] private RoomShape roomShape;

    [Header("Runtime Data")]
    [SerializeField] private Vector2Int anchorCell;
    [SerializeField] private List<Vector2Int> occupiedCells;

    [Header("Components")]
    [SerializeField] private CompositeCollider2D roomConfiner;

    [Header("Spawn Positions")]
    [SerializeField] private Transform defaultSpawnPosition;

    [Header("Doors")]
    [SerializeField] private Transform doorsContainer;
    [SerializeField] private List<DoorPosition> doorPositionList;
    [SerializeField] private List<DoorAppearance> doorAppearanceList;

    public int ID => id;
    public RoomDificulty RoomDificulty => roomDificulty;
    public RoomType RoomType => roomType;
    public RoomShape RoomShape => roomShape;

    public CompositeCollider2D RoomConfiner => roomConfiner;
    public Transform DefaultSpawnPosition => defaultSpawnPosition;

    public Transform DoorsContainer => doorsContainer;
    public List<DoorPosition> DoorPositionList => doorPositionList;
    public List<DoorAppearance> DoorAppearanceList => doorAppearanceList;

    public void SetAnchorCell(Vector2Int anchorCell) => this.anchorCell = anchorCell;
    public void SetOccupiedCells(List<Vector2Int> occupiedCells) => this.occupiedCells = occupiedCells;
}

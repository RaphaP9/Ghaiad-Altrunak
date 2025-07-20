using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [Header("Room Data")]
    [SerializeField] private int id;
    [SerializeField] private RoomDificulty roomDificulty;
    [SerializeField] private RoomType roomType;
    [SerializeField] private RoomShape roomShape;

    [Header("Components")]
    [SerializeField] private CompositeCollider2D roomConfiner;

    [Header("Spawn Positions")]
    [SerializeField] private Transform defaultSpawnPosition;
    [SerializeField] private List<Transform> doorSpawnPositions;

    [Header("Doors")]
    [SerializeField] private List<DoorHandler> doorHandlers;

    public int ID => id;
    public RoomDificulty RoomDificulty => roomDificulty;
    public RoomType RoomType => roomType;
    public RoomShape RoomShape => roomShape;

    public CompositeCollider2D RoomConfiner => roomConfiner;
    public Transform DefaultSpawnPosition => defaultSpawnPosition;
    public List<Transform> DoorSpawnPositions => doorSpawnPositions;
}

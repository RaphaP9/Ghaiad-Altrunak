using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [Header("Room Data")]
    [SerializeField] private int id;
    [SerializeField] private RoomDificulty roomDificulty;
    [SerializeField] private RoomType roomType;
    [SerializeField] private Vector2Int roomDimensions = new Vector2Int (1,1);

    public int ID => id;
    public RoomDificulty RoomDificulty => roomDificulty;
    public RoomType RoomType => roomType;
    public Vector2Int RoomDimensions => roomDimensions;
}

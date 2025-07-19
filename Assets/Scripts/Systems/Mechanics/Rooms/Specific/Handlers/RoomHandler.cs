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

    public int ID => id;
    public RoomDificulty RoomDificulty => roomDificulty;
    public RoomType RoomType => roomType;
    public RoomShape RoomShape => roomShape;
}

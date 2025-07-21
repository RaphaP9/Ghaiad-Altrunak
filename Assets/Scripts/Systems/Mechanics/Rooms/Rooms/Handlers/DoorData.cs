using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorData : MonoBehaviour
{
    [Header("Origin Room Data")]
    [SerializeField] private RoomData originRoomData;

    [Header("Leading Room Data")]
    [SerializeField] private Vector2Int leadingCell;

    [Header("Runtime Filled Leading Room Data")]
    [SerializeField] private RoomData leadingRoomData;
    [SerializeField] private Transform leadingRoomTransform;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorData : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RoomType doorRoomType;

    [Header("Runtime Filled")]
    [SerializeField] private Direction doorDirection;
    [Space]
    [SerializeField] private RoomData originRoomData;
    [Space]
    [SerializeField] private RoomData leadingRoomData;
    [SerializeField] private Transform leadingRoomTransform;

    public RoomType DoorRoomType => doorRoomType;

    public event EventHandler<OnDoorDataSetEventArgs> OnDoorDataSet;

    public class OnDoorDataSetEventArgs : EventArgs
    {
        public Direction doorDirection;
        public RoomData originRoomData;
        public RoomData leadingRoomData;
        public Transform leadingRoomTransform;
    }

    public void SetDoorData(Direction doorDirection, RoomData originRoomData, RoomData leadingRoomData, Transform leadingRoomTransform)
    {
        this. doorDirection = doorDirection;
        this. originRoomData = originRoomData;
        this. leadingRoomData = leadingRoomData;
        this. leadingRoomTransform = leadingRoomTransform;

        OnDoorDataSet?.Invoke(this, new OnDoorDataSetEventArgs { doorDirection = doorDirection, originRoomData = originRoomData, leadingRoomData = leadingRoomData, leadingRoomTransform = leadingRoomTransform });
    }
}

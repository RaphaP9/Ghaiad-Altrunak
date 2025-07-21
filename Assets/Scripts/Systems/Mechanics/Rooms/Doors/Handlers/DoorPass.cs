using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPass : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DoorData doorData;

    [Header("Settings")]
    [SerializeField] private LayerMask playerLayerMask;
    
    [Header("Enablers")]
    [SerializeField] private bool doorEnabled;

    private RoomData originRoomData;
    private RoomData leadingRoomData;
    private Transform leadingRoomTransform;

    private bool doorSet = false;

    private void OnEnable()
    {
        doorData.OnDoorDataSet += DoorData_OnDoorDataSet;
    }
    private void OnDisable()
    {
        doorData.OnDoorDataSet -= DoorData_OnDoorDataSet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!doorSet) return;
        if (!doorEnabled) return;
        if (!GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, playerLayerMask)) return;

        RoomTransitionHandler.Instance.TransitionToRoom(originRoomData, leadingRoomData, leadingRoomTransform);
    }

    private void DoorData_OnDoorDataSet(object sender, DoorData.OnDoorDataSetEventArgs e)
    {
        originRoomData = e.originRoomData;
        leadingRoomData = e.leadingRoomData;
        leadingRoomTransform = e.leadingRoomTransform;

        doorSet = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitionHandler : MonoBehaviour
{
    public static RoomTransitionHandler Instance { get; private set; }

    [Header("Settings")]
    [SerializeField, Range(0.1f, 2f)] private float roomTransitionTime;

    [Header("Runtime Filled")]
    [SerializeField] private bool transitioningToRoom;

    public bool TransitioningToRoom => transitioningToRoom;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        transitioningToRoom = false;
    }

    private void SetSingleton()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToRoom(RoomHandler previousRoom, RoomHandler nextRoom, Transform targetTransformToPosition)
    {
        if (previousRoom == nextRoom) return;

        StartCoroutine(TransitionToRoomCoroutine(previousRoom, nextRoom, targetTransformToPosition));   
    }

    private IEnumerator TransitionToRoomCoroutine(RoomHandler previousRoom, RoomHandler nextRoom, Transform targetTransformToPosition)
    {
        transitioningToRoom = true;

        PlayerTeleporterManager.Instance.InstantPositionPlayer(GeneralUtilities.TransformPositionVector2(targetTransformToPosition));

        yield return StartCoroutine(CameraConfinerHandler.Instance.SmoothSwitchConfinerCoroutine(nextRoom.RoomConfiner, roomTransitionTime));

        transitioningToRoom = false;
    }
}

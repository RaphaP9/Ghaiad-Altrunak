using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoomTransition : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RoomHandler roomA;
    [SerializeField] private RoomHandler roomB;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            RoomTransitionHandler.Instance.TransitionToRoom(roomA, roomB, roomB.DefaultSpawnPosition);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            RoomTransitionHandler.Instance.TransitionToRoom(roomB, roomA, roomA.DefaultSpawnPosition);
        }
    }
}

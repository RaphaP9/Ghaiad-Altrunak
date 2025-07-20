using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInstance
{
    public Vector2Int position;
    public RoomHandler roomHandler;

    public RoomInstance(Vector2Int pos, RoomHandler handler)
    {
        position = pos;
        roomHandler = handler;
    }
}
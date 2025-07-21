using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPositionInfo
{
    public RoomData OriginRoomData { get; }
    public DoorPosition DoorPosition { get; }

    public DoorPositionInfo(RoomData originRoomData, DoorPosition doorPosition)
    {
        OriginRoomData = originRoomData;
        DoorPosition = doorPosition;
    }
}

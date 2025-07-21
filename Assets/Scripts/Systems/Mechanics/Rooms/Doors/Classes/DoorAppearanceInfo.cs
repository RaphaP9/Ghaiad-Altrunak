using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAppearanceInfo
{
    public RoomData IncomingRoomData { get; }
    public DoorAppearance DoorAppearance { get; }

    public DoorAppearanceInfo(RoomData incomingRoomData, DoorAppearance doorAppearance)
    {
        IncomingRoomData = incomingRoomData;
        DoorAppearance = doorAppearance;
    }
}
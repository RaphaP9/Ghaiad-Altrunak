using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoorInstance
{
    public Transform DoorTransform {  get; private set; }
    public Direction LeadingDirection { get; private set; }
    public DoorPositionInfo DoorPositionInfo { get; private set; }
    public DoorAppearanceInfo DoorAppearanceInfo { get; private set; }

    public DoorInstance(Transform doorTransform, DoorPositionInfo doorPositionInfo, DoorAppearanceInfo doorAppearanceInfo, Direction leadingDirection)
    {
        DoorTransform = doorTransform;
        DoorPositionInfo = doorPositionInfo;
        DoorAppearanceInfo = doorAppearanceInfo;
        LeadingDirection = leadingDirection;
    }

    public DoorData GetDoorDataComponent()
    {
        if (DoorTransform.TryGetComponent(out DoorData doorData)) return doorData;
        return null;
    }
}
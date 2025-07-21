using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVisualHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DoorData doorData;

    [Header("Lists")]
    [SerializeField] private List<DoorDirectionVisual> doorDirectionVisuals;

    [Header("Runtime Filled")]
    [SerializeField] private Transform visualTransform;

    private void OnEnable()
    {
        doorData.OnDoorDataSet += DoorData_OnDoorDataSet;
    }
    private void OnDisable()
    {
        doorData.OnDoorDataSet -= DoorData_OnDoorDataSet;
    }

    private void ChooseVisualTransform(Direction doorDirection)
    {
        foreach(DoorDirectionVisual doorDirectionVisual in doorDirectionVisuals)
        {
            if(doorDirection != doorDirectionVisual.doorDirection)
            {
                doorDirectionVisual.visualTransform.gameObject.SetActive(false);
                continue;
            }

            visualTransform = doorDirectionVisual.visualTransform;
            doorDirectionVisual.visualTransform.gameObject.SetActive(true);
        }
    }

    private void DoorData_OnDoorDataSet(object sender, DoorData.OnDoorDataSetEventArgs e)
    {
        ChooseVisualTransform(e.doorDirection);
    }
}

[System.Serializable]
public class DoorDirectionVisual
{
    public Direction doorDirection;
    public Transform visualTransform;
}

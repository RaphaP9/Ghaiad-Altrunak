using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPosition : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform doorPositionTransform;
    [SerializeField] private DoorDirection doorDirection;
    [SerializeField] private Vector2Int leadingLocalCell;

    public Transform DoorPositionTransform => doorPositionTransform;
    public DoorDirection DoorDirection => doorDirection;
    public Vector2Int LeadingLocalCell => leadingLocalCell;
}

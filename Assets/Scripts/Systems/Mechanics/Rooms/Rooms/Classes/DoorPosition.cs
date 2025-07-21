using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPosition : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform doorPositionTransform;
    [SerializeField] private Direction doorDirection;
    [SerializeField] private Vector2Int leadingLocalCell;

    public Transform DoorPositionTransform => doorPositionTransform;
    public Direction DoorDirection => doorDirection;
    public Vector2Int LeadingLocalCell => leadingLocalCell;
}

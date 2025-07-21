using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAppearance : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform appearanceTransform;
    [SerializeField] private DoorDirection incomingDoorDirection;
    [SerializeField] private Vector2Int incomingLocalCell;

    public Transform AppearanceTransform => appearanceTransform;
    public DoorDirection IncomingDoorDirection => incomingDoorDirection;
    public Vector2Int IncomingLocalCell => incomingLocalCell;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAppearance : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform appearanceTransform;
    [SerializeField] private Direction incomingDirection;
    [SerializeField] private Vector2Int incomingLocalCell;

    public Transform AppearanceTransform => appearanceTransform;
    public Direction IncomingDirection => incomingDirection;
    public Vector2Int IncomingLocalCell => incomingLocalCell;
}

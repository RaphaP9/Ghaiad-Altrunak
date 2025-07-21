using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : IEquatable<DoorKey>
{
    public Vector2Int WorldCell { get; }
    public Direction Direction { get; }

    private const int INITAL_HASH = 17;
    private const int MULTIPLIER_HASH = 31;

    public DoorKey(Vector2Int worldCell, Direction direction)
    {
        WorldCell = worldCell;
        Direction = direction;
    }

    public bool Equals(DoorKey otherDoorKey)
    {
        if (otherDoorKey == null) return false;
        return WorldCell == otherDoorKey.WorldCell && Direction == otherDoorKey.Direction;
    }

    //Needed for Dictionary Key Comparison
    public override bool Equals(object obj)
    {
        return Equals(obj as DoorKey);
    }

    //Needed for Dictionary Key Comparison
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = INITAL_HASH;
            hash = hash * MULTIPLIER_HASH + WorldCell.GetHashCode();
            hash = hash * MULTIPLIER_HASH + Direction.GetHashCode();
            return hash;
        }
    }
}
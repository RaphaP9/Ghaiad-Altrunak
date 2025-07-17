using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomsUtilities
{
    private const int X_STARTING_POSITION = 0;
    private const int Y_STARTING_POSITION = 0;

    private const float X_UNIT_SIZE = 16f;
    private const float Y_UNIT_SIZE = 9f;

    private const float ROOM_DISTANCE = 1f;

    public static Vector2Int GetRoomGenerationStartingPosition() => new Vector2Int(X_STARTING_POSITION, Y_STARTING_POSITION);   
    public static Vector2 GetRoomSquareUnit() => new Vector2(X_UNIT_SIZE, Y_UNIT_SIZE);
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserInput
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }    
    
    private enum Axis
    {
        Horizontal,
        Vertical
    }

    public static bool IsActionKeyDown(PlayerID player)
    {
        KeyCode[] codes = GetActionKeyCodesForPlayer(player);

        for (int i = 0; i < codes.Length; i++)
        {
            if (Input.GetKeyDown(codes[i]))
                return true;
        }
        
        return false;
    }

    private static KeyCode[] GetActionKeyCodesForPlayer(PlayerID player)
    {
        switch (player)
        {
            case PlayerID.Player1:
                return new[] { KeyCode.Tab, KeyCode.Joystick1Button2 };
            case PlayerID.Player2:
                return new[] { KeyCode.RightShift, KeyCode.Joystick2Button2 };
            default:
                return new KeyCode[0];
        }
    }

    private static string GetAxisForPlayer(PlayerID player, Axis axis)
    {
        if (axis == Axis.Horizontal)
            return player == PlayerID.Player1 ? "Horizontal" : "Horizontal2";
        
        return player == PlayerID.Player1 ? "Vertical" : "Vertical2";
    }

    public static Direction FindBestDirection(PlayerID player)
    {
        Direction direction = Direction.None;

        float horizontalValue = Input.GetAxis(GetAxisForPlayer(player, Axis.Horizontal));
        if (Math.Abs(horizontalValue) > .1f)
        {
            direction = horizontalValue < 0f ? Direction.Left : Direction.Right;
        }
        
        float verticalValue = Input.GetAxis(GetAxisForPlayer(player, Axis.Vertical));
        if (Math.Abs(verticalValue) > .1f && Math.Abs(verticalValue) > Math.Abs(horizontalValue))
        {
            direction = verticalValue < 0f ? Direction.Down : Direction.Up;
        }

        return direction;
    }

    private static Dictionary<PlayerID, Direction> _lastDirections = new Dictionary<PlayerID, Direction>();
    public static Direction FindBestDirectionDown(PlayerID player)
    {
        Direction bestDirection = FindBestDirection(player);

        if (_lastDirections.TryGetValue(player, out Direction lastDirection) && bestDirection == lastDirection)
            return Direction.None;

        _lastDirections[player] = bestDirection;
        return bestDirection;
    }
}

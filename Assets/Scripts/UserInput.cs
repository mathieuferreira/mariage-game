using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserInput
{
    /*public enum Key
    {
        Up,
        Down,
        Left,
        Right,
        Action
    }

    public static bool isKeyDown(PlayerID player, Key key)
    {
        KeyCode[] codes = GetKeyCodesForPlayer(player, key);

        for (int i = 0; i < codes.Length; i++)
        {
            if (Input.GetKeyDown(codes[i]))
                return true;
        }
        
        return false;
    }

    public static bool isKeyDown(int player, Key key)
    {
        switch (player)
        {
            case 0:
                return isKeyDown(PlayerID.Player1, key);
            case 1:
                return isKeyDown(PlayerID.Player2, key);
            default:
                return false;
        }
    }

    public static bool isKey(PlayerID player, Key key)
    {
        KeyCode[] codes = GetKeyCodesForPlayer(player, key);

        for (int i = 0; i < codes.Length; i++)
        {
            if (Input.GetKey(codes[i]))
                return true;
        }
        
        return false;
    }

    public static bool isKey(int player, Key key)
    {
        switch (player)
        {
            case 0:
                return isKey(PlayerID.Player1, key);
            case 1:
                return isKey(PlayerID.Player2, key);
            default:
                return false;
        }
    }

    private static KeyCode[] GetKeyCodesForPlayer(PlayerID player, Key key)
    {
        switch (player)
        {
            case PlayerID.Player1:
                return GetKeyCodesForPlayer1(key);
            case PlayerID.Player2:
                return GetKeyCodesForPlayer2(key);
            default:
                return new KeyCode[0];
        }
    }

    private static KeyCode[] GetKeyCodesForPlayer1(Key key)
    {
        switch (key)
        {
            case Key.Action:
                return new[] { KeyCode.Tab };
            case Key.Up:
                return new[] { KeyCode.Z };
            case Key.Down:
                return new[] { KeyCode.S };
            case Key.Left:
                return new[] { KeyCode.Q };
            case Key.Right:
                return new[] { KeyCode.D };
            default:
                return new KeyCode[0];
        }
    }
    private static KeyCode[] GetKeyCodesForPlayer2(Key key)
    {
        switch (key)
        {
            case Key.Action:
                return new[] { KeyCode.RightShift };
            case Key.Up:
                return new[] { KeyCode.UpArrow };
            case Key.Down:
                return new[] { KeyCode.DownArrow };
            case Key.Left:
                return new[] { KeyCode.LeftArrow };
            case Key.Right:
                return new[] { KeyCode.RightArrow };
            default:
                return new KeyCode[0];
        }
    }*/
    
    
    
    
    
    
    
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

        _lastDirections.Add(player, bestDirection);
        return bestDirection;
    }
}

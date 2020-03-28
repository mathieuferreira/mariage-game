using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserInput
{
    public enum Key
    {
        Up,
        Down,
        Left,
        Right,
        Action
    }

    public enum Player
    {
        Player1,
        Player2
    }

    public static bool isKeyDown(Player player, Key key)
    {
        switch (player)
        {
            case Player.Player1:
                return isPlayer1KeyDown(key);
            case Player.Player2:
                return isPlayer2KeyDown(key);
            default:
                return false;
        }
    }

    public static bool isKeyDown(int player, Key key)
    {
        switch (player)
        {
            case 0:
                return isPlayer1KeyDown(key);
            case 1:
                return isPlayer2KeyDown(key);
            default:
                return false;
        }
    }

    private static bool isPlayer1KeyDown(Key key)
    {
        switch (key)
        {
            case Key.Action:
                return Input.GetKeyDown(KeyCode.Tab);
            case Key.Up:
                return Input.GetKeyDown(KeyCode.Z);
            case Key.Down:
                return Input.GetKeyDown(KeyCode.S);
            case Key.Left:
                return Input.GetKeyDown(KeyCode.Q);
            case Key.Right:
                return Input.GetKeyDown(KeyCode.D);
            default:
                return false;
        }
    }

    private static bool isPlayer2KeyDown(Key key)
    {
        switch (key)
        {
            case Key.Action:
                return Input.GetKeyDown(KeyCode.RightShift);
            case Key.Up:
                return Input.GetKeyDown(KeyCode.UpArrow);
            case Key.Down:
                return Input.GetKeyDown(KeyCode.DownArrow);
            case Key.Left:
                return Input.GetKeyDown(KeyCode.LeftArrow);
            case Key.Right:
                return Input.GetKeyDown(KeyCode.RightArrow);
            default:
                return false;
        }
    }
}

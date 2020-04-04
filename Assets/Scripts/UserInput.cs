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
                return isKeyDown(Player.Player1, key);
            case 1:
                return isKeyDown(Player.Player2, key);
            default:
                return false;
        }
    }

    public static bool isKey(Player player, Key key)
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
                return isKey(Player.Player1, key);
            case 1:
                return isKey(Player.Player2, key);
            default:
                return false;
        }
    }

    private static KeyCode[] GetKeyCodesForPlayer(Player player, Key key)
    {
        switch (player)
        {
            case Player.Player1:
                return GetKeyCodesForPlayer1(key);
            case Player.Player2:
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
    }
}

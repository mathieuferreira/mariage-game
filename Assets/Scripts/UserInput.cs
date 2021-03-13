using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public static class UserInput
{
    private static readonly List<IUserInputInterceptor> Interceptors = new List<IUserInputInterceptor>();
    
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
            {
                UserActionInputEvent userInputEvent = new UserActionInputEvent(player);
                
                foreach (var interceptor in Interceptors)
                {
                    interceptor.OnUserActionInput(userInputEvent);
                }

                return !userInputEvent.IsPrevented();
            }
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

        float horizontalValue = Input.GetAxisRaw(GetAxisForPlayer(player, Axis.Horizontal));
        if (Math.Abs(horizontalValue) > .1f)
        {
            direction = horizontalValue < 0f ? Direction.Left : Direction.Right;
        }
        
        float verticalValue = Input.GetAxisRaw(GetAxisForPlayer(player, Axis.Vertical));
        if (Math.Abs(verticalValue) > .1f && Math.Abs(verticalValue) > Math.Abs(horizontalValue))
        {
            direction = verticalValue < 0f ? Direction.Down : Direction.Up;
        }

        UserDirectionInputEvent userInputEvent = new UserDirectionInputEvent(player, direction);
                
        foreach (var interceptor in Interceptors)
        {
            interceptor.OnUserDirectionInput(userInputEvent);
        }

        return userInputEvent.GetDirection();
    }

    private static readonly Dictionary<PlayerID, Direction> LastDirections = new Dictionary<PlayerID, Direction>();
    public static Direction FindBestDirectionDown(PlayerID player)
    {
        Direction bestDirection = FindBestDirection(player);

        if (LastDirections.TryGetValue(player, out Direction lastDirection) && bestDirection == lastDirection)
            return Direction.None;

        LastDirections[player] = bestDirection;
        return bestDirection;
    }

    public static void AddInterceptor(IUserInputInterceptor interceptor)
    {
        Interceptors.Add(interceptor);
    }

    public static void RemoveInterceptor(IUserInputInterceptor interceptor)
    {
        Interceptors.Remove(interceptor);
    }
}

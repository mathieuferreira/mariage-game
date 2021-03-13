using UnityEngine;

public class UserActionInputEvent
{
    private bool prevent;
    private readonly PlayerID playerID;

    public UserActionInputEvent(PlayerID playerID)
    {
        this.playerID = playerID;
    }

    public PlayerID GetPlayerID()
    {
        return playerID;
    }

    public void PreventInput(bool value)
    {
        prevent = value;
    }

    public bool IsPrevented()
    {
        return prevent;
    }
}
public class UserDirectionInputEvent
{
    private readonly PlayerID playerID;
    private UserInput.Direction direction;

    public UserDirectionInputEvent(PlayerID playerID, UserInput.Direction direction)
    {
        this.playerID = playerID;
        this.direction = direction;
    }

    public PlayerID GetPlayerID()
    {
        return playerID;
    }

    public UserInput.Direction GetDirection()
    {
        return direction;
    }

    public void SetDirection(UserInput.Direction newDirection)
    {
        direction = newDirection;
    }
}
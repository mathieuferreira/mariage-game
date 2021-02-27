using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    private static int player1Index = 0;
    private static int player2Index = 0;
    private static AvatarManager _instance;

    public static AvatarManager GetInstance()
    {
        return _instance;
    }
    
    [SerializeField] private Sprite[] player1Avatars = default;
    [SerializeField] private Sprite[] player2Avatars = default;

    private void Awake()
    {
        _instance = this;
    }

    public Sprite GetAvatar(PlayerID player)
    {
        switch (player)
        {
            case PlayerID.Player1 :
                return player1Avatars[player1Index];
            default :
                return player2Avatars[player2Index];
        }
    }

    public Sprite[] GetAvatars(PlayerID player)
    {
        switch (player)
        {
            case PlayerID.Player1 :
                return player1Avatars;
            default :
                return player2Avatars;
        }
    }

    public void SetIndex(PlayerID player, int index)
    {
        switch (player)
        {
            case PlayerID.Player1 :
                player1Index = index;
                break;
            default :
                player2Index = index;
                break;
        }
    }
}

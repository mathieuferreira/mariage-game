using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    private static AvatarManager _instance;

    public static AvatarManager GetInstance()
    {
        return _instance;
    }
    
    private const string AvatarSaveId = "avatar";
    
    [SerializeField] private Sprite[] player1Avatars;
    [SerializeField] private Sprite[] player2Avatars;

    private int player1Index = 0;
    private int player2Index = 0;

    private void Awake()
    {
        player1Index = 0;
        player2Index = 0;

        string save = PlayerPrefs.GetString(AvatarSaveId, null);

        if (!string.IsNullOrEmpty(save))
        {
            AvatarSave avatarSave = JsonUtility.FromJson<AvatarSave>(save);
            player1Index = avatarSave.player1Index;
            player2Index = avatarSave.player2Index;
        }
        
        _instance = this;
    }

    public Sprite GetAvatar(UserInput.Player player)
    {
        switch (player)
        {
            case UserInput.Player.Player1 :
                return player1Avatars[player1Index];
            default :
                return player2Avatars[player2Index];
        }
    }

    public void SetIndexes(int player1, int player2)
    {
        player1Index = player1;
        player2Index = player2;
    }
    
    public void Save()
    {
        AvatarSave save = new AvatarSave()
        {
            player1Index = player1Index,
            player2Index = player2Index
        };
        PlayerPrefs.SetString(AvatarSaveId, JsonUtility.ToJson(save));
        PlayerPrefs.Save();
    }

    private class AvatarSave
    {
        public int player1Index = 0;
        public int player2Index = 0;
    }
}

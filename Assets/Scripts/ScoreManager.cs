using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager
{
    private static ScoreManager _instance;

    public static ScoreManager GetInstance()
    {
        if (_instance == null)
            _instance = new ScoreManager();
        
        return _instance;
    }
    
    public event EventHandler OnScoreChange;
    
    private const string ScoreSaveId = "score";
    
    private int[] scores;

    private ScoreManager()
    {
        Load();
    }

    public void IncrementScore(UserInput.Player player)
    {
        IncrementScore(player, 1);
    }

    public void IncrementScore(UserInput.Player player, int amount)
    {
        scores[GetPlayerIndex(player)] += amount;
        
        if (OnScoreChange != null)
            OnScoreChange(this, EventArgs.Empty);
    }

    public int GetScore(UserInput.Player player)
    {
        return scores[GetPlayerIndex(player)];
    }

    public int GetScore()
    {
        return scores.Sum();
    }

    private int GetPlayerIndex(UserInput.Player player)
    {
        return player == UserInput.Player.Player1 ? 0 : 1;
    }
    
    public void Save()
    {
        PlayerPrefs.SetString(ScoreSaveId, JsonUtility.ToJson(scores));
        PlayerPrefs.Save();
    }

    public void Restart()
    {
        scores = new[] {0, 0};
    }

    public void Load()
    {
        scores = new[] {0, 0};
        string save = PlayerPrefs.GetString(ScoreSaveId, null);
        
        if (!string.IsNullOrEmpty(save))
            scores = JsonUtility.FromJson<int[]>(save);
    }
}

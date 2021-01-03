using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScoreManager
{
    public static event EventHandler OnScoreChange;
    
    private static int[] scores = { 0, 0 };
    private static int[] sessionScores = { 0, 0 };

    public static void IncrementScore(PlayerID player)
    {
        IncrementScore(player, 1);
    }

    public static void IncrementScore(PlayerID player, int amount)
    {
        sessionScores[GetPlayerIndex(player)] += amount;
        OnScoreChange?.Invoke(null, EventArgs.Empty);
    }

    public static int GetScore(PlayerID player)
    {
        int playerIndex = GetPlayerIndex(player);
        return scores[playerIndex] + sessionScores[playerIndex];
    }

    public static int GetGlobalScore()
    {
        return scores.Sum() + sessionScores.Sum();
    }

    private static int GetPlayerIndex(PlayerID player)
    {
        return player == PlayerID.Player1 ? 0 : 1;
    }

    public static void StartSession()
    {
        sessionScores = new [] {0, 0};
        OnScoreChange?.Invoke(null, EventArgs.Empty);
    }
    
    public static void RevertSession()
    {
        sessionScores = new [] {0, 0};
        OnScoreChange?.Invoke(null, EventArgs.Empty);
    }

    public static void CloseSession()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] += sessionScores[i];
        }

        StartSession();
    }

    public static void Initialize()
    {
        scores = new [] {0, 0};
        StartSession();
    }
}

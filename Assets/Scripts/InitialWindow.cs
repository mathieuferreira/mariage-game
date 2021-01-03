using System;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;

public class InitialWindow : MonoBehaviour
{
    private static float ERROR_MAX_TIME = 6f;
    
    [SerializeField] private Transform[] buttons = default;
    [SerializeField] private Transform[] ticks = default;
    [SerializeField] private GameObject errorText = default;
    [SerializeField] private PlayerReadyButton[] players = default;
    
    private int[] selectedButtons;
    private bool isBlinkShown;
    private float errorTimer;
    
    private void Awake()
    {
        selectedButtons = new [] {0, 0};
        isBlinkShown = true;
        errorTimer = 0f;

        for (int i = 0; i < players.Length; i++)
        {
            players[i].BeforePlayerReady += OnBeforePlayerReady;
            players[i].OnPlayerReady += OnOnPlayerReady;
        }
    }

    private void OnOnPlayerReady(object sender, EventArgs e)
    {
        if (areAllPlayersJoinedTheGame())
        {
            FunctionTimer.Create(() =>
            {
                Loader.Load(Loader.Scene.ChoosePlayer);
            }, .5f);
        }
    }

    private void OnBeforePlayerReady(object sender, PlayerReadyButton.BeforePlayerReadyEventArgs e)
    {
        PlayerReadyButton playerReadyButton = sender as PlayerReadyButton;

        int playerId = playerReadyButton.GetPlayerId() == PlayerID.Player1 ? 0 : 1;
        
        buttons[selectedButtons[playerId]].transform.GetComponent<Animator>().SetTrigger("PlayerEnterGame");
        
        String error = GetErrorText(playerId);

        if (error == null)
        {
            HideError();
        }
        else
        {
            ShowError(error);
            e.Cancel();
        }
    }

    private void Update()
    {
        HandleInputs();
        HandleError();
    }

    private void HandleError()
    {
        if (errorTimer <= 0f)
            return;

        errorTimer -= Time.deltaTime;

        if (errorTimer <= 0f)
        {
            HideError();
        }
    }

    private void HideError()
    {
        errorText.SetActive(false);
        errorTimer = 0f;
    }

    private void ShowError(String content)
    {
        errorTimer = ERROR_MAX_TIME;
        errorText.GetComponent<Text>().text = content;
        errorText.SetActive(true);
    }

    private void HandleInputs()
    {
        for (int i = 0; i < 2; i++)
        {
            if (players[i].IsPlayerReady())
                continue;

            UserInput.Direction direction = UserInput.FindBestDirectionDown(players[i].GetPlayerId());

            switch (direction)
            {
                case UserInput.Direction.Up:
                    SelectButton(i, -1);
                    break;
                case UserInput.Direction.Down:
                    SelectButton(i, +1);
                    break;
            }
        }
    }

    private bool areAllPlayersJoinedTheGame()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].IsPlayerReady())
            {
                return false;
            }
        }
        
        return true;
    }

    private String GetErrorText(int player)
    {
        if (selectedButtons[player] == 1)
            return null;

        if (selectedButtons[player] == 0)
            return "ET NON ! MAINTENANT, TU N'ES PLUS TOUT SEUL !";

        return (selectedButtons[player] + 1) + " JOUEURS ?! PAS ENCORE ! PEUT-ETRE DANS QUELQUES MOIS...";
    }

    private void SelectButton(int player, int delta)
    {
        int button = selectedButtons[player] + delta;
        
        if (button < 0)
            button = 0;

        if (button > 3)
            button = 3;
        
        selectedButtons[player] = button;

        Vector3 move = buttons[button].position - ticks[player].position;
        move.x = 0f;
        move.z = 0f;
        ticks[player].position += move;
    }
}

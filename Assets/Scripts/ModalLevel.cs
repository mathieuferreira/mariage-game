using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalLevel : MonoBehaviour
{
    public event EventHandler gameStart;
    
    [SerializeField] private Modal explanationWindow = default;
    [SerializeField] private Modal winWindow = default;
    [SerializeField] private Modal looseWindow = default;

    private void Awake()
    {
        explanationWindow.afterClose += OnExplanationClosed;
    }

    public void OnExplanationClosed(object sender, EventArgs e)
    {
        CountDown.GetInstance().StartCounter(3, () =>
        {
            if (gameStart != null)
                gameStart(this, EventArgs.Empty);
        });
    }

    public void OpenWinWindow(Action afterAction)
    {
        winWindow.beforeClose += (o, args) =>
        {
            afterAction();
        };
        winWindow.Open();
    }

    public void OpenLooseWindow(Action afterAction)
    {
        looseWindow.beforeClose += (o, args) =>
        {
            afterAction();
        };
        looseWindow.Open();
    }
}

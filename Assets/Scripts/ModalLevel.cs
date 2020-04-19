using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalLevel : MonoBehaviour
{
    public event EventHandler gameStart;
    
    [SerializeField] private Modal explanationWindow;
    [SerializeField] private Modal winWindow;

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
}

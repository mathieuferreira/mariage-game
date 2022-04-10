using System;
using UnityEngine;

public class QuitManager : MonoBehaviour
{
    private void Update()
    {
        if (
            (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftControl))
            || (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.RightControl))
            || (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.Q))
            || (Input.GetKeyDown(KeyCode.RightControl) && Input.GetKey(KeyCode.Q))
        )
        {
            Application.Quit();
        }
    }
}
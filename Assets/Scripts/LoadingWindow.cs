using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour
{
    private static float MAX_TIMER_DOT = 1f;
    
    private Text loadingText;
    private float timer;
    private int dotNumber;
    private bool nextSceneStartLoaded;

    private void Awake()
    {
        loadingText = transform.Find("Text").GetComponent<Text>();
        timer = MAX_TIMER_DOT;
        dotNumber = 2;
        nextSceneStartLoaded = false;
    }

    private void Update()
    {
        if (!nextSceneStartLoaded)
        {
            Loader.LoadTargetScene();
            nextSceneStartLoaded = true;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            dotNumber = (dotNumber + 1) % 3;
            String text = "LOADING";

            for (int i = 0; i <= dotNumber; i++)
            {
                text += ".";
            }

            loadingText.text = text;
            timer += MAX_TIMER_DOT;
        }
    }
}

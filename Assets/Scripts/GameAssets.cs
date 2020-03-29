using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public Sprite[] Player1PicturesSprites;
    public Sprite[] Player2PicturesSprites;
    
    private static GameAssets instance;

    public static GameAssets getInstance()
    {
        return instance;
    }

    public void Awake()
    {
        instance = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        Initial,
        Loading,
        ChoosePlayer,
        Adventure,
        School,
        Bar,
        Home,
        Breakout
    }

    private static Scene _targetScene;
    
    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(Scene.Loading.ToString());
        _targetScene = scene;
    }

    public static void LoadTargetScene()
    {
        SceneManager.LoadScene(_targetScene.ToString());
    }
}

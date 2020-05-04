using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeLevel : MonoBehaviour
{
    private const float Speed = 5f;
    
    [SerializeField] private HomePlayer[] players;
    
    private HomeEnvironment environment;

    private void Awake()
    {
        environment = GetComponent<HomeEnvironment>();
    }

    private void Start()
    {
        environment.SetSpeed(Speed);
    }
}

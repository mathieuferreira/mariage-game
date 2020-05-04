using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HomeLevel : MonoBehaviour
{
    private const float MinDistanceSpiderMove = 3f;
    public const float MaxYPosition = 12f;
    public const float MinYPosition = -12f;
    private const float EnemyXSpawn = 30f;
    private const float EnemyXDisappear = -30f;
    
    private const float Speed = 5f;
    
    [SerializeField] private HomePlayer[] players;
    [SerializeField] private Transform spider;
    
    private HomeEnvironment environment;
    private List<HomeEnemy> spiders;

    private void Awake()
    {
        spiders = new List<HomeEnemy>();
        environment = GetComponent<HomeEnvironment>();
    }

    private void Start()
    {
        environment.SetSpeed(Speed);
        SpawnSpider();

    }

    private void Update()
    {
        
    }

    private HomeEnemy SpawnSpider()
    {
        float minYPosition = MinYPosition + Random.Range(0f, MaxYPosition - MinYPosition - MinDistanceSpiderMove);
        float maxYPosition = minYPosition + MinDistanceSpiderMove + Random.Range(0f, MaxYPosition - minYPosition - MinDistanceSpiderMove);

        Debug.Log(new Vector3(EnemyXSpawn, minYPosition, 0f));
        
        Transform spiderTransform = Instantiate(spider, new Vector3(EnemyXSpawn, minYPosition, 0f), Quaternion.identity);
        HomeEnemy homeEnemy = spiderTransform.GetComponent<HomeEnemy>();
        homeEnemy.Setup(Vector2.left * Speed, minYPosition, maxYPosition);
        spiders.Add(homeEnemy);
        return homeEnemy;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class HomeEnvironment : MonoBehaviour
{
    private const float DISAPPEAR_X_POSITION = -60f;
    private const float SPAWN_X_POSITION = 60f;
    
    [SerializeField] private Transform floorTransform;
    [SerializeField] private Transform ceilingTransform;
    [SerializeField] private Transform wallTransform;
    [SerializeField] private Transform cloud1Transform;
    [SerializeField] private Transform cloud2Transform;
    [SerializeField] private Transform ground1Transform;
    [SerializeField] private Transform ground2Transform;

    private enum Type
    {
        Floor,
        Ceiling,
        Interior,
        Cloud1,
        Cloud2,
        Ground1,
        Ground2
    }
    
    private float speed;
    
    private Dictionary<Type, float> sizes;
    private Dictionary<Type, List<Transform>> transforms;

    private void Awake()
    {
        speed = 0f;

        sizes = new Dictionary<Type, float>();
        transforms = new Dictionary<Type, List<Transform>>();
        Type[] types = (Type[]) Enum.GetValues(typeof(Type));
        for (int i = 0; i < types.Length; i++)
        {
            sizes.Add(types[i], GetTransform(types[i]).GetComponent<Renderer>().bounds.size.x);
            transforms.Add(types[i], new List<Transform>());
        }
        
        SpawnInitialTransforms();
    }

    private void Update()
    {
        if (speed <= 0f)
            return;

        HandleMove();
    }

    private Transform GetTransform(Type type)
    {
        switch (type)
        {
            case Type.Ceiling:
                return ceilingTransform;
            case Type.Floor:
                return floorTransform;
            case Type.Cloud1:
                return cloud1Transform;
            case Type.Cloud2:
                return cloud2Transform;
            case Type.Ground1:
                return ground1Transform;
            case Type.Ground2:
                return ground2Transform;
            default:
                return wallTransform;
        }
    }

    private void HandleMove()
    {
        foreach (KeyValuePair<Type, List<Transform>> keyValue in transforms)
        {
            Type type = keyValue.Key;
            List<Transform> typeTransforms = keyValue.Value;
            
            float lastPosition = DISAPPEAR_X_POSITION;
            for (int i = 0; i < typeTransforms.Count; i++)
            {
                Transform typeTransform = typeTransforms[i];
                typeTransform.position += Vector3.left * speed * GetSpeedFactor(type) * Time.deltaTime;
    
                if (typeTransform.position.x < DISAPPEAR_X_POSITION)
                {
                    transforms[type].RemoveAt(i);
                    Destroy(typeTransform.gameObject);
                    i--;
                }
    
                if (typeTransform.position.x > lastPosition)
                    lastPosition = typeTransform.position.x;
            }
    
            while (lastPosition < SPAWN_X_POSITION)
            {
                lastPosition += sizes[type];
                SpawnTransform(type, lastPosition);
            }
        }
    }
    
    private void SpawnInitialTransforms()
    {
        foreach (Type type in transforms.Keys)
        {
            float position = DISAPPEAR_X_POSITION;
            while (position < SPAWN_X_POSITION)
            {
                SpawnTransform(type, position);
                position += sizes[type];
            }
        }
    }

    private void SpawnTransform(Type type, float positionX)
    {
        Transform initial = GetTransform(type);
        Transform newTransform = Instantiate(initial, new Vector3(positionX, initial.position.y, 0f), initial.rotation);
        transforms[type].Add(newTransform);
    }

    private float GetSpeedFactor(Type type)
    {
        switch (type)
        {
            case Type.Ground1:
                return .6f;
            case Type.Ground2:
                return .5f;
            case Type.Cloud1:
                return .25f;
            case Type.Cloud2:
                return .15f;
            default:
                return 1f;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}

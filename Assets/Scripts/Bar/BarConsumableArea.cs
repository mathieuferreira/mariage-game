using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarConsumableArea : MonoBehaviour
{
    [SerializeField] private Transform[] consumablePositions;
    [SerializeField] private BarConsumable.Type consumableType;
    [SerializeField] private Transform consumable;

    private List<GameObject> consumableList; 

    private void Awake()
    {
        consumableList = new List<GameObject>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!HasConsumableAvailable())
            return;
        
        BarPlayer player = other.GetComponent<BarPlayer>();

        if (player != null && !player.GetConsumableList().IsFull())
        {
            player.ShowAdviceButton();
            
            if (UserInput.isKeyDown(player.GetPlayerId(), UserInput.Key.Action))
            {
                BarConsumable consummable = TryConsume();
                player.GetConsumableList().Add(consummable);
                
                if (player.GetConsumableList().IsFull())
                    player.HideAdviceButton();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BarPlayer player = other.GetComponent<BarPlayer>();

        if (player != null)
        {
            player.HideAdviceButton();
        }
    }

    private BarConsumable TryConsume()
    {
        if (!HasConsumableAvailable())
            return null;

        int consumableIndex = consumableList.Count - 1;
        GameObject consumableToDelete = consumableList[consumableIndex];
        consumableList.RemoveAt(consumableIndex);
        Destroy(consumableToDelete);
        return new BarConsumable(consumableType);
    }

    private bool HasConsumableAvailable()
    {
        return consumableList.Count > 0;
    }
    
    public bool HasConsumablePlace()
    {
        return consumableList.Count < consumablePositions.Length;
    }

    public void AddConsumable()
    {
        Vector3 position = consumablePositions[consumableList.Count].position;
        position.z = 0f;
        Transform newConsumable = Instantiate(consumable, position, Quaternion.identity);
        consumableList.Add(newConsumable.gameObject);
    }
}

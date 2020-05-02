using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSystem : MonoBehaviour
{
    [SerializeField] private Sprite beerSprite;
    [SerializeField] private Sprite cakeSprite;
    [SerializeField] private Sprite talkSprite;
    
    private Transform bubble1;
    private Transform bubble2;
    private BarConsumableList consumableList;

    private void Awake()
    {
        bubble1 = transform.Find("Bubble1");
        bubble1.gameObject.SetActive(false);
        
        bubble2 = transform.Find("Bubble2");
        bubble2.gameObject.SetActive(false);
    }

    public void Setup(BarConsumableList list)
    {
        consumableList = list;
        consumableList.change += ConsumableListOnchange;
    }

    private void ConsumableListOnchange(object sender, EventArgs e)
    {
        switch (consumableList.Count())
        {
            case 0:
                bubble1.gameObject.SetActive(false);
                bubble2.gameObject.SetActive(false);
                break;
            case 1 :
                bubble1.Find("Consumable").GetComponent<SpriteRenderer>().sprite =
                    GetSpriteForType(consumableList.Get(0).GetType());
                bubble1.gameObject.SetActive(true);
                bubble2.gameObject.SetActive(false);
                break;
            default :
                bubble2.Find("Consumable1").GetComponent<SpriteRenderer>().sprite =
                    GetSpriteForType(consumableList.Get(0).GetType());
                bubble2.Find("Consumable2").GetComponent<SpriteRenderer>().sprite =
                    GetSpriteForType(consumableList.Get(1).GetType());
                bubble1.gameObject.SetActive(false);
                bubble2.gameObject.SetActive(true);
                break;
        }
    }

    private Sprite GetSpriteForType(BarConsumable.Type type)
    {
        switch (type)
        {
            case BarConsumable.Type.Beer:
                return beerSprite;
            case BarConsumable.Type.Cake:
                return cakeSprite;
            default:
                return talkSprite;
        }
    }
}

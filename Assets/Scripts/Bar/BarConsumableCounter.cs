using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarConsumableCounter : MonoBehaviour
{
    [SerializeField] private BarPlayer player;

    private Text beerText;
    private Text cakeText;

    private void Awake()
    {
        player.GetConsumableList().change += Onchange;
        beerText = transform.Find("BeerText").GetComponent<Text>();
        cakeText = transform.Find("CakeText").GetComponent<Text>();
    }

    private void Onchange(object sender, EventArgs e)
    {
        BarConsumableList consumableList = (BarConsumableList) sender;
        beerText.text = "x" + consumableList.CountType(BarConsumable.Type.Beer);
        cakeText.text = "x" + consumableList.CountType(BarConsumable.Type.Cake);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

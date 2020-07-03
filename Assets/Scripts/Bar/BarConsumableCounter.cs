﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bar
{
    public class BarConsumableCounter : MonoBehaviour
    {
        [SerializeField] private BarPlayer player;

        private Text beerText;
        private Text cakeText;

        private void Awake()
        {
            player.GetConsumableList().Change += Onchange;
            beerText = transform.Find("BeerText").GetComponent<Text>();
            cakeText = transform.Find("CakeText").GetComponent<Text>();
        }

        private void Onchange(object sender, EventArgs e)
        {
            BarConsumableList consumableList = (BarConsumableList) sender;
            beerText.text = "x" + consumableList.CountType(BarConsumable.Kind.Beer);
            cakeText.text = "x" + consumableList.CountType(BarConsumable.Kind.Cake);
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
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bar
{
    public class BarConsumableCounter : MonoBehaviour
    {
        [SerializeField] private BarPlayer player = default;
        [SerializeField] private Text beerText = default;
        [SerializeField] private Animator beerAnimator = default;
        [SerializeField] private Text cakeText = default;
        [SerializeField] private Animator cakeAnimator = default;
        private static readonly int Change = Animator.StringToHash("Change");

        private void Awake()
        {
            player.GetConsumableList().Change += Onchange;
        }

        private void Onchange(object sender, BarConsumableChangeEventArgs e)
        {
            BarConsumableList consumableList = (BarConsumableList) sender;
            beerText.text = "x" + consumableList.CountType(BarConsumable.Kind.Beer);
            cakeText.text = "x" + consumableList.CountType(BarConsumable.Kind.Cake);

            if (e.GetConsumable().GetKind() == BarConsumable.Kind.Beer)
            {
                beerAnimator.SetTrigger(Change);
            }
            else
            {
                cakeAnimator.SetTrigger(Change);
            }
        }
    }
}

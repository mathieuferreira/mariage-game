using System;
using UnityEngine;

namespace Bar
{
    public class BubbleSystem : MonoBehaviour
    {
        [SerializeField] private Sprite beerSprite = default;
        [SerializeField] private Sprite cakeSprite = default;
        [SerializeField] private Sprite talkSprite = default;
    
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
            consumableList.Change += ConsumableListOnchange;
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
                        GetSpriteForType(consumableList.Get(0).GetKind());
                    bubble1.gameObject.SetActive(true);
                    bubble2.gameObject.SetActive(false);
                    break;
                default :
                    bubble2.Find("Consumable1").GetComponent<SpriteRenderer>().sprite =
                        GetSpriteForType(consumableList.Get(0).GetKind());
                    bubble2.Find("Consumable2").GetComponent<SpriteRenderer>().sprite =
                        GetSpriteForType(consumableList.Get(1).GetKind());
                    bubble1.gameObject.SetActive(false);
                    bubble2.gameObject.SetActive(true);
                    break;
            }
        }

        private Sprite GetSpriteForType(BarConsumable.Kind kind)
        {
            switch (kind)
            {
                case BarConsumable.Kind.Beer:
                    return beerSprite;
                case BarConsumable.Kind.Cake:
                    return cakeSprite;
                default:
                    return talkSprite;
            }
        }
    }
}

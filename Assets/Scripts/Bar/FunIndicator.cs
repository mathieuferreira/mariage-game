using System;
using UnityEngine;

namespace Bar
{
    public class FunIndicator : MonoBehaviour
    {
        public event EventHandler OnFunExpired;
    
        private const int UnitByGuest = 100;
        private const float Speed = 2f;
        private const int UnitTalkSatisfied = 15;
        private const int UnitCakeSatisfied = 50;
        private const int UnitBeerSatisfied = 40;
    
        private BarGuest[] guests;
        private float currentIndex;
        private bool active;
        private Transform indicator;
        private float size;

        private void Awake()
        {
            currentIndex = 0;
            SetActive(false);
            indicator = transform.Find("Indicator");
            size = transform.Find("Bar").GetComponent<RectTransform>().sizeDelta.x;
        }

        private void FixedUpdate()
        {
            if (!active)
                return;

            float max = GetMaxIndex();
            currentIndex = Mathf.Clamp(currentIndex - Speed * GetGuestNeedsCount() * Time.fixedDeltaTime, 0f, max);
        
            indicator.localPosition = new Vector3((currentIndex / max - .5f) * size, 0f, 0f);

            if (currentIndex <= 0f)
                OnFunExpired?.Invoke(this, EventArgs.Empty);
        }

        public void Setup(BarGuest[] barGuests)
        {
            guests = barGuests;

            foreach (BarGuest guest in guests)
            {
                guest.OnNeedsComplete += OnNeedsComplete;
            }
        
            currentIndex = GetMaxIndex();
        }

        private void OnNeedsComplete(object sender, BarGuest.NeedCompleteEventArgs eventArgs)
        {
            int delta;
            switch (eventArgs.Consumable.GetKind())
            {
                case BarConsumable.Kind.Beer:
                    delta = UnitBeerSatisfied;
                    break;
                case BarConsumable.Kind.Cake:
                    delta = UnitCakeSatisfied;
                    break;
                default:
                    delta = UnitTalkSatisfied;
                    break;
            }
        
            currentIndex = Mathf.Clamp(currentIndex + delta, 0f, GetMaxIndex());
        }

        private int GetMaxIndex()
        {
            return UnitByGuest * guests.Length;
        }

        private int GetGuestNeedsCount()
        {
            int needs = 0;

            foreach (BarGuest guest in guests)
            {
                needs += guest.GetNeeds().Count();
            }
        
            return needs;
        }

        public void SetActive(bool act)
        {
            gameObject.SetActive(act);
            active = act;
        }
    }
}

using System;
using Home;
using UnityEngine;

namespace Breakout
{
    public class BreakoutBrick : MonoBehaviour
    {
        public event EventHandler OnBreak;
        
        [SerializeField] private Transform explosionEffect;
        [SerializeField] private Color color;
        [SerializeField] private GameObject armor;
        [SerializeField] private GameObject armorDamaged;
        [SerializeField] private State initialState;

        private State currentState;

        private enum State
        {
            FullArmor,
            DamagedArmor,
            NoArmor
        }

        private void Awake()
        {
            GetComponent<SpriteRenderer>().color = color;
            SetState(initialState);
        }

        private void SetState(State state)
        {
            currentState = state;

            switch (currentState)
            {
                case State.FullArmor:
                    armor.SetActive(true);
                    armorDamaged.SetActive(false);
                    break;
                case State.DamagedArmor:
                    armor.SetActive(false);
                    armorDamaged.SetActive(true);
                    break;
                default:
                    armor.SetActive(false);
                    armorDamaged.SetActive(false);
                    break;
            }
        }

        public void Damage()
        {
            switch (currentState)
            {
                case State.FullArmor:
                    SetState(State.DamagedArmor);
                    break;
                case State.DamagedArmor:
                    SetState(State.NoArmor);
                    break;
                default:
                    OnBreak?.Invoke(this, EventArgs.Empty);
                    Destroy(gameObject);
                    Transform explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    var main = explosion.GetComponent<ParticleSystem>().main;
                    main.startColor = color;
                    Destroy(explosion.gameObject, 2f);
                    break;
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}

using UnityEngine;

namespace Bar
{
    public class BarConsumableWorker : MonoBehaviour
    {
        private const float Speed = 2f;
        private const float StorageTime = .5f;
    
        [SerializeField] private BarConsumableArea storage = default;
        [SerializeField] private Transform storagePosition = default;
        [SerializeField] private Transform factoryPosition = default;
        [SerializeField] private float factoryTime = default;
        [SerializeField] private Vector2 factoryDirection = default;
        [SerializeField] private Vector2 storageDirection = default;
        [SerializeField] private GameObject cookingTransform = default;
        [SerializeField] private string cookingSound = default;
    
        private enum State
        {
            Idle,
            MoveToFactory,
            Producing,
            MoveToStorage,
            Storing
        }

        private State currentState;
        private RPGPlayerAnimator animator;
        private float timer;
        private bool active;

        private void Awake()
        {
            animator = GetComponent<RPGPlayerAnimator>();
            currentState = State.Idle;
            active = false;
        }

        private void FixedUpdate()
        {
            if (!active)
                return;
        
            switch (currentState)
            {
                case State.MoveToFactory:
                    MoveToPosition(factoryPosition.position);
                    break;
                case State.Producing:
                    ProcessProducingState();
                    break;
                case State.MoveToStorage:
                    MoveToPosition(storagePosition.position);
                    break;
                case State.Storing:
                    ProcessStoringState();
                    break;
                default:
                    ProcessIdleState();
                    break;
            }
        }

        private void StartNextStage()
        {
            switch (currentState)
            {
                case State.MoveToFactory:
                    timer = factoryTime;
                    animator.SetSpeedAndDirection(0f, factoryDirection);
                    cookingTransform.SetActive(true);
                    SoundManager.GetInstance().Play(cookingSound);
                    currentState = State.Producing;
                    break;
                case State.Producing:
                    SoundManager.GetInstance().StopPlaying(cookingSound);
                    cookingTransform.SetActive(false);
                    currentState = State.MoveToStorage;
                    break;
                case State.MoveToStorage:
                    animator.SetSpeedAndDirection(0f, storageDirection);
                    timer = StorageTime;
                    currentState = State.Storing;
                    break;
                case State.Storing:
                    currentState = State.Idle;
                    break;
                default:
                    currentState = State.MoveToFactory;
                    break;
            }
        }

        private void ProcessIdleState()
        {
            if (storage.HasConsumablePlace())
            {
                StartNextStage();
            }
        }

        private void MoveToPosition(Vector3 position)
        {
            Vector3 direction = position - transform.position;
            direction.z = 0f;
            Vector3 movement = direction.normalized * (Speed * Time.fixedDeltaTime);

            bool lastMove = false;
            if (Mathf.Approximately(direction.sqrMagnitude, 0) || Mathf.Approximately(movement.sqrMagnitude, 0) || movement.sqrMagnitude >= direction.sqrMagnitude)
            {
                movement = direction;
                lastMove = true;
            }

            transform.position += movement;

            animator.SetSpeedAndDirection(Speed, movement);

            if (lastMove)
            {
                StartNextStage();
            }
        }
    
        private void ProcessStoringState()
        {
            timer -= Time.fixedDeltaTime;
        
            if (timer < 0f)
            {
                storage.AddConsumable();
                StartNextStage();
            }
        }
    
        private void ProcessProducingState()
        {
            timer -= Time.fixedDeltaTime;
        
            if (timer < 0f)
            {
                StartNextStage();
            }
        }

        public void StartWork()
        {
            active = true;
        }

        public void StopWork()
        {
            active = false;
        }
    }
}

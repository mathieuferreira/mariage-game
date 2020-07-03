using UnityEngine;

namespace Bar
{
    public class BarConsumableWorker : MonoBehaviour
    {
        private const float Speed = 2f;
        private const float StorageTime = .5f;
    
        [SerializeField] private BarConsumableArea storage;
        [SerializeField] private Transform storagePosition;
        [SerializeField] private Transform factoryPosition;
        [SerializeField] private float factoryTime;
        [SerializeField] private Vector2 factoryDirection;
        [SerializeField] private Vector2 storageDirection;
        [SerializeField] private GameObject cookingTransform;
    
        private enum State
        {
            Idle,
            MoveToFactory,
            Producing,
            MoveToStorage,
            Storing
        }

        private State currentState;
        private Animator animator;
        private float timer;
        private bool active;
        private static readonly int AnimationVertical = Animator.StringToHash("Vertical");
        private static readonly int AnimationHorizontal = Animator.StringToHash("Horizontal");
        private static readonly int AnimationSpeed = Animator.StringToHash("Speed");

        private void Awake()
        {
            animator = GetComponent<Animator>();
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
                    animator.SetFloat(AnimationVertical, factoryDirection.y);
                    animator.SetFloat(AnimationHorizontal, factoryDirection.x);
                    animator.SetFloat(AnimationSpeed, 0f);
                    cookingTransform.SetActive(true);
                    currentState = State.Producing;
                    break;
                case State.Producing:
                    cookingTransform.SetActive(false);
                    currentState = State.MoveToStorage;
                    break;
                case State.MoveToStorage:
                    animator.SetFloat(AnimationVertical, storageDirection.y);
                    animator.SetFloat(AnimationHorizontal, storageDirection.x);
                    animator.SetFloat(AnimationSpeed, 0f);
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

            animator.SetFloat(AnimationVertical, movement.y);
            animator.SetFloat(AnimationHorizontal, movement.x);
            animator.SetFloat(AnimationSpeed, Speed);

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

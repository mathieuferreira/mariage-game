using DefaultNamespace;
using UnityEngine;

public class AfkManager : MonoBehaviour, IUserInputInterceptor
{
    [SerializeField] private float maxAfkTimer = 600;
    
    private float timer;

    private void Awake()
    {
        UserInput.AddInterceptor(this);
        InitializeTimer();
    }

    private void Update()
    {
        if (UserInput.IsStartKey(PlayerID.Player1) && UserInput.IsStartKey(PlayerID.Player2))
        {
            Loader.Load(Loader.Scene.Initial);
        }
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        
        if (timer <= 0f)
        {
            Loader.Load(Loader.Scene.Initial);
        }
    }

    private void OnDestroy()
    {
        UserInput.RemoveInterceptor(this);
    }

    private void InitializeTimer()
    {
        timer = maxAfkTimer;
    }

    public void OnUserActionInput(UserActionInputEvent userInputEvent)
    {
        InitializeTimer();
    }

    public void OnUserDirectionInput(UserDirectionInputEvent userInputEvent)
    {
        if (userInputEvent.GetDirection() != UserInput.Direction.None)
        {
            InitializeTimer();
        }
    }
}
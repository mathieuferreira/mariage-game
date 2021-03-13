namespace DefaultNamespace
{
    public interface IUserInputInterceptor
    {
        void OnUserActionInput(UserActionInputEvent userInputEvent);
        void OnUserDirectionInput(UserDirectionInputEvent userInputEvent);
    }
}
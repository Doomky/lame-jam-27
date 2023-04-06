namespace Framework.Interfaces
{
    interface IStateMachine<TState, TAction> 
    {
        TState CurrentState { get; }

        void InjectAction(TAction action);
    }
}

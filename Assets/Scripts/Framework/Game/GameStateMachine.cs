using Framework.StateMachine;

namespace Framework
{
    public abstract class GameStateMachine<TGameAction> : StateMachine<GameState, TGameAction>
        where TGameAction : System.Enum
    {
        protected GameStateMachine(GameState initialState) : base(initialState)
        {
        }
    }
}

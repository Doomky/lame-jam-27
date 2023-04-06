using System;

namespace Framework.StateMachine
{
    public interface IState
    {
        public event Action<IState> Entered;
        public event Action<IState> Exited;

        void OnEnter();

        void OnExit();
    }
}

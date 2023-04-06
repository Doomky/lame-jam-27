using System;

namespace Framework.StateMachine
{
    public abstract class State : IState
    {
        public event Action<IState> Entered;
        public event Action<IState> Exited;

        public virtual void OnEnter()
        {
            this.Entered?.Invoke(this);
        }

        public virtual void OnExit()
        {
            this.Exited?.Invoke(this);
        }
    }
}

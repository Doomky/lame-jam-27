using Framework.Interfaces;
using Sirenix.OdinInspector;
using System;

namespace Framework.StateMachine
{
    [Serializable]
    public abstract partial class StateMachine<TState, TActionEnum> : IStateMachine<TState, TActionEnum>
    where TState : class, IState
    where TActionEnum : Enum
    {
        [ShowInInspector, ReadOnly]
        protected TState _currentState;

        [ShowInInspector, ReadOnly]
        protected bool _isTransitionning = false;

        public bool IsTransitionning => this._isTransitionning;

        public TState CurrentState => this._currentState;

        public StateMachine(TState initialState)
        {
            this._currentState = initialState;
            this.Enter(initialState);
        }

        [FoldoutGroup("Actions")]
        [Button(nameof(InjectAction))]
        public void InjectAction(TActionEnum action)
        {
            if (!this._isTransitionning && this.TryGetNextState(action, out TState nextState))
            {
                this._isTransitionning = true;
                this.Exit();
                this.Enter(nextState);
                this._isTransitionning = false;
            }
        }

        protected abstract bool TryGetNextState(TActionEnum action, out TState nextState);
        protected virtual void Enter(TState nextState)
        {
            this._currentState = nextState;
            this._currentState?.OnEnter();
        }

        protected virtual void Exit()
        {
            this._currentState?.OnExit();
            this._currentState = null;
        }
    }
}

using Framework.Interfaces;
using Sirenix.OdinInspector;
using System;

namespace Framework.StateMachine
{
    [Serializable]
    public abstract partial class EnumStateMachine<TState, TActionEnum> : IStateMachine<TState, TActionEnum>
        where TState : Enum
        where TActionEnum : Enum
    {
        [ShowInInspector, ReadOnly]
        protected TState _currentState;

        [ShowInInspector, ReadOnly]
        protected bool _isTransitionning = false;

        public event Action<EnumStateMachine<TState, TActionEnum>, TState> EnterState;
        public event Action<EnumStateMachine<TState, TActionEnum>, TState> ExitState;

        public bool IsTransitionning => _isTransitionning;

        public TState CurrentState => _currentState;

        public void Iinit(TState initialState)
        {
            _currentState = initialState;
            this.OnEntering(_currentState);
        }

        [FoldoutGroup("Actions")]
        [Button(nameof(InjectAction))]
        public void InjectAction(TActionEnum action)
        {
            if (!_isTransitionning && TryGetNextState(action, out TState nextState))
            {
                _isTransitionning = true;

                {
                    this.OnExiting(_currentState);
                    ExitState?.Invoke(this, _currentState);
                }

                _currentState = nextState;

                {
                    this.OnEntering(_currentState);
                    EnterState?.Invoke(this, _currentState);
                }

                _isTransitionning = false;
            }
        }
        
        protected abstract bool TryGetNextState(TActionEnum action, out TState nextState);
        protected abstract void OnExiting(TState state);
        protected abstract void OnEntering(TState state);
    }
}

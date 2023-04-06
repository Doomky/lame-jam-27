using Framework.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI
{
    public class ShowableAnimationStateMachine : EnumStateMachine<ShowableAnimationStateMachine.State, ShowableAnimationStateMachine.Action>
    {
        public enum State
        {
            Hidden,
            Hidding,
            Shown,
            Showing,
        }

        public enum Action
        {
            Show,
            Hide,
            HideEnd,
            ShowEnd
        }

        [ShowInInspector]
        [ReadOnly]
        [PropertyRange(0,1)]
        public float VisibilityBlend => this._fadeCurve.Evaluate(Mathf.Clamp01(this._fadeCursor/this._fadeDuration));

        private float _fadeCursor = 0;

        [SerializeField]
        private AnimationCurve _fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField]
        private float _fadeDuration = 1f;
        
        private IShowable _showable = null;

        public ShowableAnimationStateMachine(IShowable showable, State initalState) : base()
        {
            this._showable = showable;
            this.Iinit(initalState);
        }

        public void Update(Animator windowAnimator)
        {
            switch (this._currentState)
            {
                case State.Showing:
                    {
                        this._fadeCursor = Mathf.Min(this._fadeDuration, this._fadeCursor + Time.deltaTime);
                        if (this._fadeCursor == this._fadeDuration)
                        {
                            this.InjectAction(Action.ShowEnd);
                        }

                        break;
                    }

                case State.Hidding:
                    {
                        this._fadeCursor = Mathf.Max(0, this._fadeCursor - Time.deltaTime);
                        if (this._fadeCursor == 0)
                        {
                            this.InjectAction(Action.HideEnd);
                        }
                        
                        break;
                    }

                default:
                    break;
            }

            windowAnimator.SetFloat(nameof(this.VisibilityBlend), this.VisibilityBlend);
        }

        protected override bool TryGetNextState(Action action, out State nextState)
        {
            switch (action)
            {
                case Action.Show:
                    {
                        if (this._currentState == State.Hidden || this._currentState == State.Hidding)
                        {
                            nextState = State.Showing;
                            return true;
                        }

                        break;
                    }

                case Action.ShowEnd:
                    {
                        nextState = State.Shown;
                        return true;
                    }

                case Action.Hide:
                    {
                        if (this._currentState == State.Shown || this._currentState == State.Showing)
                        {
                            nextState = State.Hidding;
                            return true;
                        }

                        break;
                    }

                case Action.HideEnd:
                    {
                        nextState = State.Hidden;
                        return true;
                    }

                default:
                    break;
            }

            nextState = this._currentState;
            return false;
        }

        protected override void OnEntering(State state)
        {
            switch (state)
            {
                case State.Hidden:
                    {
                        this._fadeCursor = 0;
                        this._showable.RectTransform.gameObject.SetActive(false);
                        break;
                    }

                case State.Shown:
                    {
                        this._fadeCursor = this._fadeDuration;
                        this._showable.RectTransform.gameObject.SetActive(true);
                        break;
                    }

                case State.Hidding:
                case State.Showing:
                    {
                        // Don't change blend value we can interrupt an Hidding / Showing
                        this._showable.RectTransform.gameObject.SetActive(true);
                        break;
                    }

                default:
                    break;
            }
        }

        protected override void OnExiting(State state)
        {
        }
    }
}
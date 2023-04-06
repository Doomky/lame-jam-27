using UnityEngine;
using Sirenix.OdinInspector;

namespace Framework.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Panel : Entity, IShowable
    {
        [SerializeField, Required]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private ShowableAnimationStateMachine _stateMachine = null;

        public bool IsVisible => this._stateMachine.CurrentState == ShowableAnimationStateMachine.State.Showing || this._stateMachine.CurrentState == ShowableAnimationStateMachine.State.Shown;

        public void Load()
        {
            this._stateMachine = new(this, ShowableAnimationStateMachine.State.Hidden);
        }

        public void Unload()
        {
            this._stateMachine = null;
        }

        public void UpdateVisibility(bool state)
        {
            bool isVisible = this.IsVisible;
            if (isVisible != state)
            {
                this._stateMachine.InjectAction(state ? ShowableAnimationStateMachine.Action.Show : ShowableAnimationStateMachine.Action.Hide);
            }
        }

        public abstract void Refresh();

        protected virtual void Update()
        {
            this._stateMachine.Update(this._animator);
        }
    }

    public abstract class Panel<TDirtyFlags> : Panel
        where TDirtyFlags : System.Enum
    {
        [ShowInInspector]
        protected TDirtyFlags _dirtyFlags = default(TDirtyFlags);

        public abstract TDirtyFlags NoneDirtyFlag { get; }

        protected override void Update()
        {
            base.Update();
            if (!this._dirtyFlags.Equals(this.NoneDirtyFlag))
            {
                this.Refresh();
                this._dirtyFlags = this.NoneDirtyFlag;
            }
        }
    }
}
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Framework.UI.ShowableAnimationStateMachine;

namespace Framework.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Window : Entity, IWindow
    {
        private static Dictionary<Type, Window> _windowsByType = new();

        public static TWindow Get<TWindow>() where TWindow : Window
        {
            return (TWindow)_windowsByType[typeof(TWindow)];
        }

        [FoldoutGroup("Required Components")]
        [SerializeField, Required]
        protected CanvasGroup _canvasGroup = null;

        [ShowInInspector, HideInEditorMode]
        protected ShowableAnimationStateMachine _stateMachine = null;

        public bool IsVisible => this._stateMachine.CurrentState == State.Showing || this._stateMachine.CurrentState == State.Shown;

        public virtual void Load()
        {
            _windowsByType.Add(this.GetType(), this);

            State initalState = this.ShouldBeVisible() ? State.Shown : State.Hidden;
            this._stateMachine = new(this, initalState);
        }

        public virtual void Unload()
        {
            this._stateMachine = null;

            _windowsByType.Remove(this.GetType());
        }

        public void UpdateVisibility()
        {
            bool isVisible = this.IsVisible;
            bool newVisibility = this.ShouldBeVisible();
            if (isVisible != newVisibility)
            {
                this._stateMachine.InjectAction(newVisibility ? ShowableAnimationStateMachine.Action.Show : ShowableAnimationStateMachine.Action.Hide);
            }
        }

        protected abstract void Refresh();

        protected abstract bool ShouldBeVisible();

        protected virtual void Update()
        {
            this._stateMachine.Update(this._animator);
        }
    }

    public abstract class Window<TDirtyFlags> : Window 
        where TDirtyFlags : Enum
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
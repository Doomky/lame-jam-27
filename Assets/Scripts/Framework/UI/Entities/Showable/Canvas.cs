using Framework.Helpers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    public abstract class Canvas : Entity, ICanvas
    {
        [SerializeField]
        private List<GameObject> _windowGroupPrefabs = new();

        [ShowInInspector]
        private List<IWindowGroup> _windowGroups = new();

        [SerializeField]
        private ShowableAnimationStateMachine _stateMachine = null;

        public abstract void Bind();

        public abstract void Unbind();

        public void Load()
        {
            int windowGroupsCount = this._windowGroupPrefabs.Count;
            for (int i = 0; i < windowGroupsCount; i++)
            {
                IWindowGroup windowGroup = PrefabHelpers.InstantiateUI(this._windowGroupPrefabs[i].GetComponent<WindowGroup>(), transform);

                windowGroup.Load();

                this._windowGroups.Add(windowGroup);
            }

            this._stateMachine.EnterState += this.AnimationStateMachine_EnterState;
            this._stateMachine.ExitState += this.AnimationStateMachine_ExitState;
        }

        public void Unload()
        {
            this._stateMachine.ExitState -= this.AnimationStateMachine_ExitState;
            this._stateMachine.EnterState -= this.AnimationStateMachine_EnterState;

            int windowGroupsCount = this._windowGroups.Count;
            for (int i = 0; i < windowGroupsCount; i++)
            {
                this._windowGroups[i].Unload();
            }

            this._windowGroups.Clear();
        }

        public void UpdateVisibility()
        {
            int windowGroupsCount = this._windowGroups.Count;
            for (int i = 0; i < windowGroupsCount; i++)
            {
                this._windowGroups[i].UpdateVisibility();
            }
        }

        private void AnimationStateMachine_EnterState(StateMachine.EnumStateMachine<ShowableAnimationStateMachine.State, ShowableAnimationStateMachine.Action> action, ShowableAnimationStateMachine.State state)
        {
            switch (state)
            {
                case ShowableAnimationStateMachine.State.Hidden:
                    {
                        this.transform.gameObject.SetActive(false);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void AnimationStateMachine_ExitState(StateMachine.EnumStateMachine<ShowableAnimationStateMachine.State, ShowableAnimationStateMachine.Action> action, ShowableAnimationStateMachine.State state)
        {
            switch (state)
            {
                case ShowableAnimationStateMachine.State.Hidden:
                    {
                        this.transform.gameObject.SetActive(true);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
}
using System;
using Framework.Managers;
using Framework.Managers.Audio;
using OldGame.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    public abstract class Game<TGameStateMachine, TGameAction> : SerializedMonoBehaviour
        where TGameStateMachine : GameStateMachine<TGameAction>, new()
        where TGameAction : Enum
    {
        public static Game<TGameStateMachine, TGameAction> Singleton = null;

        [SerializeField]
        protected SuperManager _superManager = null;

        [HideInEditorMode]
        [ShowInInspector]
        protected TGameStateMachine _stateMachine = new();

        public TGameStateMachine StateMachine => this._stateMachine;

        public virtual void LoadCoreServices()
        {
            this._superManager.Add<TimeManager>();
            this._superManager.Add<CameraManager>();
            this._superManager.Add<SaveManager>();
            this._superManager.Add<SFXManager>();
            this._superManager.Add<BGMManager>();
            this._superManager.Add<InputManager>();
            this._superManager.Add<UIManager>();
        }

        public virtual void UnloadCoreServices()
        {
            this._superManager.Remove<UIManager>();
            this._superManager.Remove<InputManager>();
            this._superManager.Remove<BGMManager>();
            this._superManager.Remove<SFXManager>();
            this._superManager.Remove<SaveManager>();
            this._superManager.Remove<CameraManager>();
            this._superManager.Remove<TimeManager>();
        }

        protected virtual void Awake()
        {
            Singleton = this;
            this.LoadCoreServices();
        }

        protected virtual void OnDestroy()
        {
            this.UnloadCoreServices();
            Singleton = null;
        }
    }
}

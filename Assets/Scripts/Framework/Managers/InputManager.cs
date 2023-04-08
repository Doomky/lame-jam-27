using Game;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Framework.Managers
{
    [RequireComponent(typeof(InputSystemUIInputModule))]
    public class InputManager : Manager
    {
        public event Action<Vector2> Moved;
        public event Action<Vector2> Pointed;
        public event Action Fired;
        public event Action Switched;

        private InputActions inputActions;

        [ShowInInspector]
        private Vector2 _pointedPosition;

        [Required, SerializeField]
        private InputSystemUIInputModule _uiInputModule = null;
        private Vector2 _moveDirection;

        private bool _isFiring;

        public override void Bind()
        {
            this.inputActions = new InputActions();
            this.inputActions.Enable();
        }

        public override void Unbind()
        {
            this.inputActions.Disable();
            this.inputActions = null;
        }

        public void FixedUpdate()
        {
            this.Moved?.Invoke(this._moveDirection);

            if (this.inputActions.Player.Fire.inProgress)
            {
                this.Fired?.Invoke();
            }
        }

        public void OnMove(InputValue inputValue)
        {
            this._moveDirection = inputValue.Get<Vector2>();
        }

        public void OnPoint(InputValue inputValue)
        {
            this._pointedPosition = inputValue.Get<Vector2>();
            this.Pointed?.Invoke(this._pointedPosition);
        }

        public void OnSwitch(InputValue inputValue)
        {
            this.Switched?.Invoke();
        }
    }
}
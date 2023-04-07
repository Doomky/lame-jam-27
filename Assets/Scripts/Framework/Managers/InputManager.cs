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
        public event Action<Vector2, float> Clicked;

        [SerializeField]
        private Player player;

        [ShowInInspector]
        private Vector2 _pointedPosition;

        [Required, SerializeField]
        private InputSystemUIInputModule _uiInputModule = null;
        private Vector2 _moveDirection;

        public override void Bind()
        {
        }

        public override void Unbind()
        {
        }

        public void FixedUpdate()
        {
            this.Moved?.Invoke(this._moveDirection);
        }

        public void OnMove(InputValue inputValue)
        {
            this._moveDirection = inputValue.Get<Vector2>();
        }

        public void OnClick(InputValue inputValue)
        {
            this.Clicked?.Invoke(this._pointedPosition, inputValue.Get<float>());
        }

        public void OnPoint(InputValue inputValue)
        {
            this._pointedPosition = inputValue.Get<Vector2>();
            this.Pointed?.Invoke(this._pointedPosition);
        }
    }
}
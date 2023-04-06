using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Framework.Managers
{
    [RequireComponent(typeof(InputSystemUIInputModule))]
    public class InputManager : Manager
    {
        public event Action<Vector2> Pointed;
        public event Action<Vector2, float> Clicked;

        [ShowInInspector]
        private Vector2 _pointedPosition;

        [Required, SerializeField]
        private InputSystemUIInputModule _uiInputModule = null;

        public override void Bind()
        {
        }

        public override void Unbind()
        {
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
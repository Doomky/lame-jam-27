using Framework.Managers;
using Framework.Managers.Audio;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.UI.Components
{
    [RequireComponent(typeof(Button))]
    public class UnityButton : Entity, IButton
    {
        public event Action<IButton> Click;
        public event Action<IButton> MouseEnter;
        public event Action<IButton> MouseExit;

        [FoldoutGroup("Required Component")]
        [Required] 
        [SerializeField]
        private Button _button;

        [FoldoutGroup("Audio")]
        [SerializeField] 
        private AudioClip _clickSFX;

        [FoldoutGroup("Audio")]
        [SerializeField]
        private AudioClip _hoverSFX;

        public Button Button => this._button;

        protected override void Awake()
        {
            base.Awake();
            this._button.onClick.AddListener(this.Button_OnClick);
        }

        private void Button_OnClick()
        {
            SuperManager.Singleton.Get<SFXManager>().PlayGlobalSFX(this._clickSFX);
            this.Click?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SuperManager.Singleton.Get<SFXManager>().PlayGlobalSFX(this._hoverSFX);
            this.MouseEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.MouseExit?.Invoke(this);
        }
    }
}
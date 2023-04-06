using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Framework.UI.Components
{
    [RequireComponent(typeof(LocalizeStringEvent))]
    public abstract class LocalizedText<TTextComponent, TTextStyle, TFont> : Text<TTextComponent, TTextStyle, TFont>, ILocalizedText<TTextComponent, TTextStyle, TFont>
        where TTextComponent : Component
        where TTextStyle : TextStyle<TTextComponent, TFont>
    {
        [FoldoutGroup("Required Components")]
        [SerializeField]
        [Required]
        protected LocalizeStringEvent _localizeStringEvent = null;

        public LocalizeStringEvent LocalizeStringEvent => this._localizeStringEvent;

        protected abstract string Value { set; }

        public void Refresh(LocalizedString localizedString)
        {
            this._localizeStringEvent.StringReference = localizedString;
            this._localizeStringEvent.RefreshString();
        }

        public void Refresh<TArgument>(LocalizedString localizedString, TArgument arguments) where TArgument : ILocalizationArgument
        {
            this._localizeStringEvent.StringReference = localizedString;
            this._localizeStringEvent.StringReference.Arguments = arguments.Get();
            this._localizeStringEvent.RefreshString();
        }

        public void Refresh<TArgument>(TArgument arguments) where TArgument : ILocalizationArgument
        {
            this._localizeStringEvent.StringReference.Arguments = arguments.Get();
            this._localizeStringEvent.RefreshString();
        }

        protected virtual void OnEnable()
        {
            this._localizeStringEvent.OnUpdateString.AddListener(this.OnUpdateString);
        }

        protected virtual void OnDisable()
        {
            this._localizeStringEvent.OnUpdateString.RemoveListener(this.OnUpdateString);
        }

        private void OnUpdateString(string value)
        {
            this.Value = value;
        }
    }
}
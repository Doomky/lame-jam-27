using Framework.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI.Components
{
    public abstract class Text<TTextComponent, TTextStyle, TFont> : Entity, IText<TTextComponent, TTextStyle, TFont>
        where TTextComponent : Component
        where TTextStyle : TextStyle<TTextComponent, TFont>
    {
        [SerializeField]
        protected TTextStyle _textStyle = null;

        [FoldoutGroup("Required Components")]
        [SerializeField]
        [Required]
        protected TTextComponent _textComponent = null;

        [FoldoutGroup("Display Settings")]
        [SerializeField]
        [HideLabel]
        private TextDisplaySettings _displaySettings = new();

        public abstract Color Color { get; set; }

        public virtual TTextStyle TextStyle
        {
            get
            {
                return this._textStyle;
            }
            set
            {
                this._textStyle = value;
                this._textStyle?.Apply(this._textComponent);
            }
        }

        public TTextComponent TextComponent => this._textComponent;

        public TextDisplaySettings DisplaySettings => this._displaySettings;

        protected override void Awake()
        {
            base.Awake();

            this._textStyle?.Apply(this._textComponent);
        }
    }
}
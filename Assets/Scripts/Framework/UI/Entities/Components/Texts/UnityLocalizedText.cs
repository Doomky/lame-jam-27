using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI.Components
{
    [RequireComponent(typeof(Text))]
    public class UnityLocalizedText : LocalizedText<Text, UnityTextStyle, Font>
    {
        protected override string Value
        {
            set
            {
                this._textComponent.text = value;
            }
        }

        public override Color Color 
        {
            get
            {
                return this._textComponent.color;
            }

            set
            {
                this._textComponent.color = value;
            }
        }
    }
}
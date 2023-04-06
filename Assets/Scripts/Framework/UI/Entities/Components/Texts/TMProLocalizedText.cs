using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace Framework.UI.Components
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMProLocalizedText : LocalizedText<TextMeshProUGUI, TMProTextStyle, TMP_FontAsset>
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
using TMPro;
using UnityEngine;

namespace Framework.UI.Components
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMProUnlocalizedText : UnlocalizedText<TextMeshProUGUI, TMProTextStyle, TMP_FontAsset>
    {
        public override string Value
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

        public string GetText()
        {
            return this._textComponent.text;
        }
    }
}
using TMPro;
using UnityEngine;

namespace Framework.UI.Components
{
    [CreateAssetMenu(fileName = "TMProTextStyle", menuName = "Framework/UI/TMProTextStyle")]
    public class TMProTextStyle : TextStyle<TextMeshProUGUI, TMP_FontAsset>
    {
        public override void Apply(TextMeshProUGUI textComponent)
        {
            textComponent.font = _font;
            textComponent.fontSize = GetFontSizeFromObjectScale(this, textComponent);
            textComponent.color = _color;
        }
    }
}

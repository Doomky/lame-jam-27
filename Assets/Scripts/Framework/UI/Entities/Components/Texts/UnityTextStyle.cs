using UnityEngine;

namespace Framework.UI.Components
{
    [CreateAssetMenu(fileName = "UnityTextStyle", menuName = "Framework/UI/UnityTextStyle")]
    public class UnityTextStyle : TextStyle<UnityEngine.UI.Text, Font>
    {
        public override void Apply(UnityEngine.UI.Text textComponent)
        {
            textComponent.font = _font;
            textComponent.fontSize = Mathf.RoundToInt(GetFontSizeFromObjectScale(this, textComponent));
            textComponent.color = _color;
        }
    }
}

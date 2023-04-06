using UnityEngine;

namespace Framework.UI.Components
{
    public abstract class UnlocalizedText<TTextComponent, TTextStyle, TFont> : Text<TTextComponent, TTextStyle, TFont>, IUnlocalizedText<TTextComponent, TTextStyle, TFont>
        where TTextComponent : Component
        where TTextStyle : TextStyle<TTextComponent, TFont>
    {
        public abstract string Value { set; }
    }
}
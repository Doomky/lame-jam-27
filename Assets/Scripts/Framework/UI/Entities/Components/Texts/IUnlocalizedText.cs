using UnityEngine;

namespace Framework.UI.Components
{
    public interface IUnlocalizedText<TComponent, TTextStyle, TFont> : IText<TComponent, TTextStyle, TFont>
    where TComponent : Component
    where TTextStyle : ITextStyle<TComponent, TFont>

    {
        string Value { set; }
    }
}

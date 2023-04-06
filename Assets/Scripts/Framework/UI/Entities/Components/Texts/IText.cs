using UnityEngine;

namespace Framework.UI.Components
{
    public interface IText<TComponent, TTextStyle, TFont>
        where TComponent : Component
        where TTextStyle : ITextStyle<TComponent, TFont>

    {
        TTextStyle TextStyle { get; set; }

        Color Color { get; set; }

        TextDisplaySettings DisplaySettings { get; }
    }
}

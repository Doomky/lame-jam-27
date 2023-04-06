using UnityEngine;
using TMPro;

namespace Framework.UI.Components
{
    public interface ITextStyle<TComponent, TFont> where TComponent : Component
    {
        TFont Font { get; }
        Color Color { get; }
        float Size { get; }
        void Apply(TComponent textComponent);
    }
}
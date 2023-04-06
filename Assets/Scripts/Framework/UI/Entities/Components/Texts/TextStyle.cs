using System;
using UnityEngine;
using UnityText = UnityEngine.UI.Text;

namespace Framework.UI.Components
{
    public abstract class TextStyle<TComponent, TFont> : ScriptableObject, ITextStyle<TComponent, TFont> where TComponent : Component
    {

        protected static float GetFontSizeFromObjectScale(TextStyle<TComponent, TFont> textStyle, TComponent textComponent)
        {
            return textStyle.Size / ((Vector2)textComponent.transform.localScale).magnitude;
        }

        [SerializeField] protected TFont _font;
        [SerializeField] protected Color _color;
        [SerializeField] protected float _size;

        public TFont Font => _font;
        public Color Color => _color;
        public float Size => _size;
        public abstract void Apply(TComponent textComponent);
    }
}

using UnityEngine;
using UnityEngine.Localization;

namespace Framework.UI.Components
{
    public interface ILocalizedText<TComponent, TTextStyle, TFont> : IText<TComponent, TTextStyle, TFont>
    where TComponent : Component
    where TTextStyle : ITextStyle<TComponent, TFont>

    {
        void Refresh(LocalizedString localizedString);

        void Refresh<TArgument>(LocalizedString localizedString, TArgument arguments) where TArgument : ILocalizationArgument;

        void Refresh<TArgument>(TArgument arguments) where TArgument : ILocalizationArgument;
    }
}

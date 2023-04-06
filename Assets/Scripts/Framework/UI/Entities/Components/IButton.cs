using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.UI.Components
{
    public interface IButton : IPointerEnterHandler, IPointerExitHandler
    {
        event Action<IButton> Click;

        Button Button { get; }
    }
}
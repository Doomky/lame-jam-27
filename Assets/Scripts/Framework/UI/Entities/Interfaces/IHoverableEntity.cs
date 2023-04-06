using UnityEngine;

namespace Framework.UI
{
    public interface IHoverableEntity
    {
        void OnHoverEnter(Vector2 mousePosition);

        void OnHoverLeave(Vector2 mousePosition);
    }
}
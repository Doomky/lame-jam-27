using Framework.UI;
using UnityEngine;

namespace Framework.Managers
{
    public class ScreenUIEntity : Entity, IScreenUIEntity
    {
        public bool IsVisible => gameObject.activeSelf;

        protected virtual void Show()
        {
            gameObject.SetActive(true);
        }

        protected virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateVisibility(bool visibility)
        {
            if (IsVisible)
            {
                if (!visibility)
                {
                    Hide();
                }
                else
                {
                    Debug.LogWarning($"{this.GetType().FullName}|{gameObject.name}: cannot show it's already visible");
                }
            }
            else
            {
                if (visibility)
                {
                    Show();
                }
                else
                {
                    Debug.LogWarning($"{this.GetType().FullName} | {gameObject.name}: cannot hide it's already visible");
                }
            }
        }
    }
}
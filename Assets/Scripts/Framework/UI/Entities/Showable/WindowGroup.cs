using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class WindowGroup : Entity, IWindowGroup
    {
        [SerializeField, Required]
        private CanvasGroup _canvasGroup = null;

        [SerializeField]
        private List<IWindow> _windows = new();

        public List<IWindow> Windows => this._windows;

        public bool IsVisible => this.transform.gameObject.activeSelf;

        public void Add(IWindow window)
        {
            this._windows.Add(window);
        }

        public void Remove(IWindow window)
        {
            this._windows.Remove(window);
        }

        public void UpdateVisibility()
        {
            int windowsCount = this._windows.Count;
            for (int i = 0; i < windowsCount; i++)
            {
                this._windows[i].UpdateVisibility();
            }

            bool isVisible = this.IsVisible;
            bool newVisibility = this.ShouldBeVisible();
            if (isVisible != newVisibility)
            {
                this.transform.gameObject.SetActive(newVisibility);

                // TODO: replace with a show animation
                this._canvasGroup.alpha = newVisibility ? 1 : 0;
            }
        }

        public void Load()
        {
            int windowsCount = this._windows.Count;
            for (int i = 0; i < windowsCount; i++)
            {
                this._windows[i].Load();
            }

            this.UpdateVisibility();
        }

        public void Unload()
        {
            int windowsCount = this._windows.Count;
            for (int i = 0; i < windowsCount; i++)
            {
                this._windows[i].Unload();
            }
        }

        protected abstract bool ShouldBeVisible();
    }
}
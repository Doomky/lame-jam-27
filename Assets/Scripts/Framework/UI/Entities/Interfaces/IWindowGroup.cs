using System.Collections.Generic;

namespace Framework.UI
{
    public interface IWindowGroup : IUIEntity
    {
        List<IWindow> Windows { get; }

        bool IsVisible { get; }

        void Load();

        void Unload();
        
        void UpdateVisibility();

        public void Add(IWindow window);

        public void Remove(IWindow window);
    }
}
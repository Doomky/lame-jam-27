namespace Framework.UI
{
    public interface IWindow : IShowable
    {
        bool IsVisible { get; }

        void Load();
        
        void Unload();

        void UpdateVisibility();
    }
}
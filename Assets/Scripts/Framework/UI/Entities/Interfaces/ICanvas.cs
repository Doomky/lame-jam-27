namespace Framework.UI
{
    public interface ICanvas : IShowable
    {
        void Bind();

        void Unbind();

        public void Load();

        public void Unload();

        void UpdateVisibility();
    }
}
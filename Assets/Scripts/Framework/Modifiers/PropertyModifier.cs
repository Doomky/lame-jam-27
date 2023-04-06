using System;

namespace Framework.Modifiers
{
    public interface IPropertyModifier<T>
    {
        Func<T, T> Func { get; set; }
    }

    public class PropertyModifier<T> : IPropertyModifier<T>
    {
        private static int _globalNextId = 0;

        private int _id = _globalNextId++;

        private Func<T, T> _func;

        public int Id => _id;

        public Func<T, T> Func
        {
            get
            {
                return _func;
            }
            set
            {
                _func = value;
            }
        }

        public PropertyModifier(Func<T> func)
        {
            _func = (_) => func();
        }

        public PropertyModifier(Func<T, T> func)
        {
            _func = func;
        }

        public PropertyModifier(int id, Func<T> func)
        {
            _id = id;
            _func = (_) => func();
        }

        public PropertyModifier(int id, Func<T, T> func)
        {
            _id = id;
            _func = func;
        }

        public T Apply(T x)
        {
            return _func(x);
        }
    }
}

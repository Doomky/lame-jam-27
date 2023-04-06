using System;
using UnityEngine;

namespace Framework.Selectors
{
    [Serializable]
    public class SingleSelector<T> : ISelector<T>
    {
        [SerializeField]
        private T _selectable;

        public T Select(Predicate<T> predicate = null)
        {
            return this._selectable;
        }

        public bool TryToSelect(out T selectable, Predicate<T> predicate = null)
        {
            selectable = this._selectable;
            return true;
        }
    }
}
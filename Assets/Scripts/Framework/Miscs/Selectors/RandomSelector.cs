using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Selectors
{
    [Serializable]
    public class RandomSelector<T> : ISelector<T>
    {
        [SerializeField]
        private List<T> _selectables = new();

        private int? _seed = null;

        public RandomSelector()
        {
        }

        public RandomSelector(int? seed)
        {
            this._seed = seed;
        }

        public T Select(Predicate<T> preSelectionPredicate = null)
        {
            if (this._seed.HasValue)
            {
                UnityEngine.Random.InitState(this._seed.Value);
            }

            return this._selectables[UnityEngine.Random.Range(0, this._selectables.Count)];
        }

        public bool TryToSelect(out T selected, Predicate<T> preSelectionPredicate = null)
        {
            selected = this.Select(preSelectionPredicate);
            return true;
        }
    }
}
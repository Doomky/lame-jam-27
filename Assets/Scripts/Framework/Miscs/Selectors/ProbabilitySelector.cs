using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.Selectors
{
    [Serializable]
    public class ProbabilitySelector<T> : ISelector<T>
    {
        [Serializable]
        private class ProbabilitySelectableWrapper
        {
            [SerializeField]
            [HorizontalGroup]
            private float _probability;

            [SerializeField]
            [HorizontalGroup]
            private T _selectable;

            public T Selectable => this._selectable;

            public float Probability => this._probability;

            public ProbabilitySelectableWrapper(T selectable, float probability)
            {
                this._selectable = selectable;
                this._probability = probability;
            }

#if UNITY_EDITOR
            public void Editor_Normalize(float total)
            {
                this._probability /= total;
            }
#endif
        }

        [SerializeField]
        private List<ProbabilitySelectableWrapper> _selectables = new();

        private int? _seed = null;

        public ProbabilitySelector()
        {

        }

        public ProbabilitySelector(int? seed)
        {
            this._seed = seed;
        }

        public T Select(Predicate<T> predicate = null)
        {
            int count = this._selectables.Count;

            if (count == 0)
            {
                return default;
            }

            List<ProbabilitySelectableWrapper> selectables = this._selectables
                .Where(selector => predicate?.Invoke(selector.Selectable) ?? true)
                .ToList();

            float totalProbability = this._selectables
                .Select(wrapper => wrapper.Probability)
                .Sum();

            if (totalProbability == 0)
            {
                return default;
            }

            if (this._seed.HasValue)
            {
                UnityEngine.Random.InitState(this._seed.Value);
            }

            float probality = UnityEngine.Random.Range(0, totalProbability);

            int selectedIndex = 0;
            float currentProbablityThreshold = selectables[0].Probability;
            T selectable = default;

            while (currentProbablityThreshold < probality && selectedIndex < count - 1)
            {
                ProbabilitySelectableWrapper wrapper = selectables[++selectedIndex];

                currentProbablityThreshold += wrapper.Probability;
                selectable = wrapper.Selectable;
            }

            return selectables[selectedIndex].Selectable;
        }

        public bool TryToSelect(out T selectable, Predicate<T> predicate = null)
        {
            selectable = Select(predicate);

            return true;
        }

#if UNITY_EDITOR
        [Button]
        private void Editor_Normalize()
        {
            float total = this._selectables
                .Select(wrapper => wrapper.Probability)
                .Sum();

            foreach (ProbabilitySelectableWrapper wrapper in this._selectables)
            {
                wrapper.Editor_Normalize(total);
            }
        }
#endif
    }
}
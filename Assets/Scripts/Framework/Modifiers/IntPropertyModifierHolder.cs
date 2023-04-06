using UnityEngine;

namespace Framework.Modifiers
{
    public class IntPropertyModifierHolder : PropertyModifierHolder<int>
    {
        private int _min;
        private int _max;

        protected override int Min => _min;
        protected override int Max => _max;

        public IntPropertyModifierHolder(int min = int.MinValue, int max = int.MaxValue)
        {
            _min = min;
            _max = max;
        }

        protected override int Clamp(int x)
        {
            return Mathf.Clamp(x, _min, _max);
        }
    }
}

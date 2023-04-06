using UnityEngine;

namespace Framework.Modifiers
{
    public class FloatPropertyModifierHolder : PropertyModifierHolder<float>
    {
        private float _min;
        private float _max;

        protected override float Min => _min;
        protected override float Max => _max;

        public FloatPropertyModifierHolder(float min = float.MinValue, float max = float.MaxValue)
        {
            _min = min;
            _max = max;
        }

        protected override float Clamp(float x)
        {
            return Mathf.Clamp(x, _min, _max);
        }
    }
}

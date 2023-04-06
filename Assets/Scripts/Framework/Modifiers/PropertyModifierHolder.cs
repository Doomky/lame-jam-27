using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Modifiers
{
    public abstract class PropertyModifierHolder<T>
    {
        public List<PropertyModifier<T>> propertyModifiers = new();

        public bool Add(int id, Func<T,T> func)
        {
            PropertyModifier<T> propertyModifier = new(func);

            if (!propertyModifiers.Exists(prop => prop.Id == propertyModifier.Id))
            {
                propertyModifiers.Add(propertyModifier);
                return true;
            }

            return false;
        }

        public bool Add(PropertyModifier<T> propertyModifier)
        {
            if (!propertyModifiers.Exists(prop => prop.Id == propertyModifier.Id))
            {
                propertyModifiers.Add(propertyModifier);
                return true;
            }

            return false;
        }

        public void Remove(PropertyModifier<T> propertyModifier)
        {
            propertyModifiers.Remove(propertyModifier);
        }

        protected abstract T Min { get; }
        protected abstract T Max { get; }

        protected abstract T Clamp(T x);

        public T ApplyModifiers(T x)
        {
            int propertyModifiersLength = propertyModifiers.Count;
            for (int i = 0; i < propertyModifiersLength; i++)
            {
                x = Clamp(propertyModifiers[i].Apply(x));
            }

            return x;
        }
    }
}

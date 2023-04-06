using System;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Framework.Modifiers;

public class ModifierResource
{
    public class ExtendedDictionnary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public void RemoveAll(Predicate<TValue> predicate)
        {
            List<TKey> keysToRemove = new();
            foreach (KeyValuePair<TKey, TValue> item in this)
            {
                if (predicate(item.Value))
                {
                    keysToRemove.Add(item.Key);
                }
            }
            
            foreach (TKey key in keysToRemove)
            {
                Remove(key);
            }
        }
    }

    public ExtendedDictionnary<string, float> floats;
    public ExtendedDictionnary<string, int> ints;
    public ExtendedDictionnary<string, GameObject> gameObjects;
    public ExtendedDictionnary<string, bool> bools;
    public ExtendedDictionnary<string, Timer> timers;
    public ExtendedDictionnary<string, ParticleSystem> ps;
    public ExtendedDictionnary<string, PropertyModifier<int>> intPropertyModifiers;
    public ExtendedDictionnary<string, PropertyModifier<float>> floatPropertyModifiers;
    public ExtendedDictionnary<string, object> objects;

    public ModifierResource()
    {
        floats = new ExtendedDictionnary<string, float>();
        ints = new ExtendedDictionnary<string, int>();
        gameObjects = new ExtendedDictionnary<string, GameObject>();
        bools = new ExtendedDictionnary<string, bool>();
        timers = new ExtendedDictionnary<string, Timer>();
        ps = new ExtendedDictionnary<string, ParticleSystem>();
        intPropertyModifiers = new ExtendedDictionnary<string, PropertyModifier<int>>();
        floatPropertyModifiers = new ExtendedDictionnary<string, PropertyModifier<float>>();
        objects = new ExtendedDictionnary<string, object>();
    }
}

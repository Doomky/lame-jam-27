using System;
using UnityEngine;

[Serializable]
public class IntervallFloat
{
    [SerializeField] private float _min;
    [SerializeField] private float _max;

    private float _value;

    public IntervallFloat(float min, float max)
    {
        _min = min;
        _max = max;
    }

    public float Get()
    {
        return _value;
    }

    public void Reset()
    {
        _value = UnityEngine.Random.Range(_min, _max);
    }
}

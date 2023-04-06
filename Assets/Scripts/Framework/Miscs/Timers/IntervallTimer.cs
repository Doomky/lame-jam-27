using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IntervallTimer : ITimer
{
    [SerializeField] private float _minDuration;
    [SerializeField] private float _maxDuration;
    [SerializeField] private float _duration;

    public float MinDuration => _minDuration;
    public float MaxDuration => _maxDuration;
    public float Duration => _duration;

    protected float _lastResetTime;

    public IntervallTimer(float minDuration, float maxDuration)
    {
        _minDuration = minDuration;
        _maxDuration = maxDuration;
        _duration = _minDuration;
    }

    public bool IsRunning()
    {
        return !IsTriggered();
    }

    public bool IsTriggered()
    {
        return Time.time - _lastResetTime > _duration;
    }

    public void Trigger()
    {
        _lastResetTime = Time.time - _duration;
    }

    public virtual void Reset()
    {
        _lastResetTime = Time.time;
        _duration = UnityEngine.Random.Range(_minDuration, _maxDuration);
    }

    public void Reset(float minDuration, float maxDuration)
    {
        _minDuration = minDuration;
        _maxDuration = maxDuration;
        Reset();
    }

    public float TimeLeftToTrigger()
    {
        return Mathf.Max(0, _duration - (Time.time - _lastResetTime));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    public abstract class TimerBase
    {
        [SerializeField] private float _duration;

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public float ElapsedTime => Time - _lastResetTime;

        protected abstract float Time { get; }

        protected float _lastResetTime;

        public TimerBase(float duration)
        {
            this.Duration = duration;
        }

        public bool IsRunning()
        {
            return !IsTriggered();
        }

        public bool IsTriggered()
        {
            return Time - _lastResetTime > _duration;
        }

        public bool IsTriggered(float timeOffset)
        {
            return (Time + timeOffset) - _lastResetTime  > _duration;
        }

        public void Trigger()
        {
            _lastResetTime = Time - _duration;
        }

        public void Trigger(float triggerOffset = 0)
        {
            _lastResetTime = Time - (_duration - triggerOffset);
        }

        public virtual void Reset()
        {
            _lastResetTime = Time;
        }

        public float TimeLeftToTrigger()
        {
            return Mathf.Max(0, _duration - (Time  - _lastResetTime));
        }
    }
}

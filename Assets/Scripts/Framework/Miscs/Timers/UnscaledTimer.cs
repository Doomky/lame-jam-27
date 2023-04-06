using System;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class UnscaledTimer : TimerBase
    {
        protected override float Time => UnityEngine.Time.unscaledTime;

        public UnscaledTimer(float duration) : base(duration)
        {
        }
    }
}
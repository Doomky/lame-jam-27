using System;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class Timer : TimerBase
    {
        protected override float Time => UnityEngine.Time.time;

        public Timer(float duration) : base(duration)
        {
        }
    }
}

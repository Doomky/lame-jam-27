using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Managers
{

    public class TimeManager : Manager
    {
        private static readonly IComparer<ITimeManagerClient> _comparaison = Comparer<ITimeManagerClient>.Create((lockerA, lockerB) => lockerA.Priority.CompareTo(lockerB.Priority));

        public event Action<TimeManager, float> TimeScaleChanged;

        [SerializeField]
        [HideInInspector]
        private float _minTimeScale;

        [HideInInspector]
        [SerializeField]
        private float _maxTimeScale;

        private float _unlockTimescale = 1;

        [ShowInInspector]
        private SortedSet<ITimeManagerClient> _lockers = new(_comparaison);

        [ShowInInspector]
        [DisableIf(nameof(IsLocked))]
        public float MinTimeScale
        {
            get
            {
                return this._minTimeScale;
            }
            set
            {
                this._minTimeScale = Mathf.Max(0, value);
                float clampedTimescale = this.ClampTimeScale(this.TimeScale);
                if (Time.timeScale != clampedTimescale)
                {
                    Time.timeScale = clampedTimescale;
                    this.TimeScaleChanged?.Invoke(this, Time.timeScale);
                }
            }
        }

        [ShowInInspector]
        [DisableIf(nameof(IsLocked))]
        public float MaxTimeScale
        {
            get
            {
                return this._maxTimeScale;
            }
            set
            {
                this._maxTimeScale = Mathf.Min(10, value);
                float clampedTimescale = this.ClampTimeScale(this.TimeScale);
                if (Time.timeScale != clampedTimescale)
                {
                    Time.timeScale = clampedTimescale;
                    this.TimeScaleChanged?.Invoke(this, Time.timeScale);
                }
            }
        }

        [ShowInInspector]
        [DisableIf(nameof(IsLocked))]
        [PropertyRange(0, 100, MinGetter = nameof(MinTimeScale), MaxGetter = nameof(MaxTimeScale))]
        public float UnlockedTimeScale
        {
            get
            {
                return this._unlockTimescale;
            }

            private set
            {
                float clampedTimescale = this.ClampTimeScale(value);
                _unlockTimescale = clampedTimescale;
                if (Time.timeScale != clampedTimescale && !this.IsLocked)
                {
                    Time.timeScale = clampedTimescale;
                    this.TimeScaleChanged?.Invoke(this, Time.timeScale);
                }
            }
        }

        [ShowInInspector]
        [ReadOnly]
        public float TimeScale
        {
            get
            {
                return Time.timeScale;
            }
        }

        public bool IsLocked => this._lockers.Count > 0;

        public void Add(ITimeManagerClient locker)
        {
            this._lockers.Add(locker);
            float clampedTimescale = Mathf.Clamp(this._lockers.Min.GetTimeScale(), this._minTimeScale, this._maxTimeScale);
            if (Time.timeScale != clampedTimescale)
            {
                Time.timeScale = clampedTimescale;
                this.TimeScaleChanged?.Invoke(this, Time.timeScale);
            }
        }

        public void Remove(ITimeManagerClient locker)
        {
            this._lockers.Remove(locker);
            float clampedTimescale = Mathf.Clamp(this.IsLocked ? this._lockers.Min.GetTimeScale() : this._unlockTimescale, this._minTimeScale, this._maxTimeScale) ;
            if (Time.timeScale != clampedTimescale)
            {
                Time.timeScale = clampedTimescale;
                this.TimeScaleChanged?.Invoke(this, Time.timeScale);
            }
        }

        public override void Bind()
        {
            Time.timeScale = 1;
        }

        public override void Unbind()
        {
        }
        
        private float ClampTimeScale(float value)
        {
            return Mathf.Clamp(value, this._minTimeScale, this._maxTimeScale);
        }
    }
}
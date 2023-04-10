using Framework.Managers;
using Framework.Managers.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public partial class Player
    {
        [BoxGroup("Data/Souls")] [SerializeField]
        private Soul _primarySoul = null;

        [SerializeField] private ParticleSystem _soulParticles = null;

        [BoxGroup("Data/Souls")] [SerializeField]
        private Soul[] _secondarySouls = null;

        [BoxGroup("Data/Souls")] [SerializeField]
        private AudioClip _switchSoul = null;

        [BoxGroup("Data/Souls")] [SerializeField]
        private Soul _emptySoul = null;

        public Soul PrimarySoul
        {
            get => this._primarySoul;
            set => this._primarySoul = value;
        }

        public Soul[] SecondarySouls
        {
            get => this._secondarySouls;
            set => this._secondarySouls = value;
        }

        private void UpdateSouls()
        {
            bool anySoulHasExpired = false;

            if (this._primarySoul.HasExpired && this._primarySoul != this._emptySoul)
            {
                this._primarySoul.Unbind(this);
                this._primarySoul = this._emptySoul;
                this._primarySoul.Bind(this, true, false);
                anySoulHasExpired = true;
            }
            else
            {
                this._primarySoul.OnFixedUpdate(this);
            }

            for (int i = 0; i < this._secondarySouls.Length; i++)
            {
                Soul soul = this._secondarySouls[i];

                if (soul.HasExpired && soul != this._emptySoul)
                {
                    this._secondarySouls[i].Unbind(this);
                    this._secondarySouls[i] = this._emptySoul;
                    this._secondarySouls[i].Bind(this, false, false);
                    anySoulHasExpired = true;
                }
                else
                {
                    soul.OnFixedUpdate(this);
                }
            }

            if (anySoulHasExpired)
            {
                OnSwapSoul?.Invoke(this._primarySoul, this._secondarySouls);
            }
        }

        public void AddSoul(Soul soul)
        {
            bool hasBeenAdded = false;

            if (this._primarySoul.Name == soul.Name)
            {
                this._primarySoul.SoulDurationTimer.Reset();
                hasBeenAdded = true;
            }

            for (int i = 0; i < this._secondarySouls.Length; i++)
            {
                if (this._secondarySouls[i].Name == soul.Name)
                {
                    this._secondarySouls[i].SoulDurationTimer.Reset();
                    hasBeenAdded = true;
                    break;
                }
            }

            if (!hasBeenAdded)
            {
                if (this._primarySoul == this._emptySoul)
                {
                    this._primarySoul.Unbind(this);
                    this._primarySoul = soul;
                    this._primarySoul.Bind(this, true, false);
                    hasBeenAdded = true;
                }
            }

            if (!hasBeenAdded)
            {
                for (int i = 0; i < this._secondarySouls.Length; i++)
                {
                    if (this._secondarySouls[i] == this._emptySoul)
                    {
                        this._secondarySouls[i].Unbind(this);
                        this._secondarySouls[i] = soul;
                        this._secondarySouls[i].Bind(this, false, false);
                        hasBeenAdded = true;
                        break;
                    }
                }
            }

            if (!hasBeenAdded)
            {
                float lowestRemainingTimer = float.MaxValue;
                int soulIndex = 0;

                for (int i = 0; i < this._secondarySouls.Length; i++)
                {
                    if (this._secondarySouls[i].SoulDurationTimer.TimeLeftToTrigger() < lowestRemainingTimer)
                    {
                        lowestRemainingTimer = this._secondarySouls[i].SoulDurationTimer.TimeLeftToTrigger();
                        soulIndex = i;
                    }
                }

                this._secondarySouls[soulIndex].Unbind(this);
                this._secondarySouls[soulIndex] = soul;
                this._secondarySouls[soulIndex].Bind(this, false, false);
            }

            OnSwapSoul?.Invoke(this._primarySoul, this._secondarySouls);
        }

        public void SwitchSoul()
        {
            this._primarySoul.Unbind(this);
            this._secondarySouls[0].Unbind(this);

            Manager.Get<SFXManager>().PlayGlobalSFX(this._switchSoul);

            Soul previousPrimarySoul = this._primarySoul;

            int length = this._secondarySouls.Length;

            this._primarySoul = this._secondarySouls[0];
            for (int i = 1; i < length; i++)
            {
                this._secondarySouls[i - 1] = this._secondarySouls[i];
            }

            this._secondarySouls[length - 1] = previousPrimarySoul;

            this._primarySoul.Bind(this, true, true);
            this._secondarySouls[length - 1].Bind(this, false, true);

            OnSwapSoul?.Invoke(this._primarySoul, this._secondarySouls);
        }

        internal bool HasAnyEmptySoul()
        {
            if (this._primarySoul == this._emptySoul)
            {
                return true;
            }

            for (int i = 0; i < this._secondarySouls.Length; i++)
            {
                if (this._secondarySouls[i] == this._emptySoul)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
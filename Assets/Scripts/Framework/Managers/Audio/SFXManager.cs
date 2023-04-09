using Framework.Data.Audio;
using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Managers.Audio
{
    public class SFXManager : Manager
    {
        [ShowInInspector] 
        protected bool _isMuted = false;

        [ShowInInspector]
        [Range(0, 1)] 
        protected float _volume = 1f;

        [ShowInInspector]
        [ReadOnly]
        protected List<AudioSource> _audioSources = new();

        [SerializeField] 
        protected AudioSourceConfiguration _audioSourceConfiguration;

        private TimeManager _timeManager = null;

        public bool IsMuted => this._isMuted;
        public float Volume => this._isMuted ? 0f : this._volume;

        public void UpdatePitch()
        {
            int count = this._audioSources?.Count ?? 0;

            for (int i = 0; i < count; i++)
            {
                _audioSources[i].pitch = this._timeManager.TimeScale;
            }
        }

        protected AudioSource GetSFXAudioSource(Transform transform, Vector2? position = null, bool isLooping = false, bool isPitchRandomized = false)
        {
            AudioSource sfxSource;

            bool isSpatilized = transform || position.HasValue;

            sfxSource = new GameObject("SFX").AddComponent<AudioSource>();

            sfxSource.loop = isLooping;
            sfxSource.playOnAwake = false;
            sfxSource.volume = Volume;

            sfxSource.spatialize = isSpatilized;
            sfxSource.spatialBlend = isSpatilized ? 1 : 0;

            sfxSource.transform.parent = transform ? transform : this.transform;
            sfxSource.transform.position = position.HasValue ? position.Value : Vector2.zero;
            sfxSource.pitch = isSpatilized ? _timeManager.TimeScale : 1;

            if (isPitchRandomized)
            {
                sfxSource.pitch *= UnityEngine.Random.Range(0.8f, 1.2f);
            }

            sfxSource.dopplerLevel = this._audioSourceConfiguration.dopplerLevel;
            sfxSource.panStereo = this._audioSourceConfiguration.panStereo;
            sfxSource.rolloffMode = this._audioSourceConfiguration.rolloffMode;
            sfxSource.SetCustomCurve(this._audioSourceConfiguration.customCurveType, this._audioSourceConfiguration.customCurve);
            sfxSource.maxDistance = this._audioSourceConfiguration.maxDistance;

            this._audioSources.Add(sfxSource);

            return sfxSource;
        }

        protected void UpdateAllSFXVolume()
        {
            float volume = this.Volume;

            int audioSourceCount = this._audioSources?.Count ?? 0;
            for (int i = 0; i < audioSourceCount; i++)
            {
                this._audioSources[i].volume = volume;
            }
        }

        public void MuteImmediate()
        {
            if (!this._isMuted)
            {
                this._isMuted = true;
                this.UpdateAllSFXVolume();
            }
        }

        public void UnmuteImmediate()
        {
            if (this._isMuted)
            {
                this._isMuted = false;
                this.UpdateAllSFXVolume();
            }
        }

        public void SetSFXVolume(float newVolume)
        {
            this._volume = Mathf.Clamp01(newVolume);
            UpdateAllSFXVolume();
        }

        public AudioSource PlayGlobalSFX(AudioClip sfxClip, bool isLooping = false, bool isPitchRandomized = false)
        {
            if (sfxClip == null)
            {
                return null;
            }

            AudioSource source = this.GetSFXAudioSource(null, null, isLooping, isPitchRandomized);

            source.clip = sfxClip;
            source.Play();

            if (!isLooping)
            {
                this.StartCoroutine(RemoveSFXSourceAtEnd(source));
            }

            return source;
        }

        public void StopSFX(AudioSource audioSource)
        {
            this.RemoveSFXSource(audioSource);
        }

        public AudioSource PlayLocalSFX(AudioClip sfxClip, Transform transform, bool isLooping = false, bool isPitchRandomized = false)
        {
            if (sfxClip == null)
            {
                return null;
            }

            AudioSource source = this.GetSFXAudioSource(transform, transform?.position ?? null, isLooping, isPitchRandomized);

            source.clip = sfxClip;
            source.Play();

            if (!isLooping)
            {
                this.StartCoroutine(this.RemoveSFXSourceAtEnd(source));
            }

            return source;
        }

        public AudioSource PlayLocalSFX(AudioClip sfxClip, Vector2 position, bool isLooping = false, bool isPitchRandomized = false)
        {
            if (sfxClip == null)
            {
                return null;
            }

            AudioSource source = this.GetSFXAudioSource(null, position, isLooping, isPitchRandomized);

            source.clip = sfxClip;
            source.Play();

            if (!isLooping)
            {
                this.StartCoroutine(this.RemoveSFXSourceAtEnd(source));
            }

            return source;
        }

        public void PlaySFXFixedDuration(AudioClip sfxClip, float duration, Transform transform)
        {
            if (sfxClip == null)
            {
                return;
            }

            AudioSource source = this.GetSFXAudioSource(transform, transform?.position ?? null);
            source.clip = sfxClip;
            source.loop = true;
            source.Play();

            this.StartCoroutine(this.RemoveSFXSourceFixedLength(source, duration));
        }

        public void PlayAmbientSFX(AudioClip sfxClip, Transform transform, float duration = float.PositiveInfinity)
        {
            if (sfxClip == null)
            {
                return;
            }

            AudioSource source = this.GetSFXAudioSource(transform, transform?.position ?? null);
            source.clip = sfxClip;
            source.loop = true;
            source.Play();

            if (duration != float.PositiveInfinity)
            {
                this.StartCoroutine(this.RemoveSFXSourceFixedLength(source, duration));
            }
        }

        public void PlayAmbientSFX(AudioClip sfxClip, Vector2 position, float duration = float.PositiveInfinity)
        {
            if (sfxClip == null)
            {
                return;
            }

            AudioSource source = this.GetSFXAudioSource(null, position);
            source.transform.position = position;
            source.clip = sfxClip;
            source.loop = true;
            source.Play();

            if (duration != float.PositiveInfinity)
            {
                this.StartCoroutine(this.RemoveSFXSourceFixedLength(source, duration));
            }
        }

        protected void RemoveSFXSource(AudioSource sfxSource)
        {
            this._audioSources.Remove(sfxSource);

            GameObject.Destroy(sfxSource.gameObject);
        }

        protected IEnumerator RemoveSFXSourceAtEnd(AudioSource sfxSource)
        {
            yield return new WaitForSeconds(sfxSource.clip.length);

            this.RemoveSFXSource(sfxSource);
        }

        protected IEnumerator RemoveSFXSourceFixedLength(AudioSource sfxSource, float length)
        {
            yield return new WaitForSeconds(length);

            this._audioSources.Remove(sfxSource);
            GameObject.Destroy(sfxSource.gameObject);
        }

        public override void Bind()
        {
            if (!SuperManager.Singleton.TryGet(out this._timeManager))
            {
                Debug.Log($"{nameof(SFXManager)}: {nameof(TimeManager)} isn't loaded yet");
                this._timeManager.TimeScaleChanged += this.TimeManager_TimeScaleChanged;
            }
        }

        public override void Unbind()
        {
            this._timeManager.TimeScaleChanged -= this.TimeManager_TimeScaleChanged;
            this._timeManager = null;
        }

        private void TimeManager_TimeScaleChanged(TimeManager arg1, float timescale)
        {
            this.UpdatePitch();
        }
    }
}
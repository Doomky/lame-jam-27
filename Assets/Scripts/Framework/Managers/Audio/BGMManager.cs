using System.Collections;
using UnityEngine;

namespace Framework.Managers.Audio
{
    public class BGMManager : Manager
    {
        const float VolumeMin = 0f;
        const float VolumeMax = 1f;

        private bool _isMuted = false;
        private float _volume = 1f;
        private AudioSource _audioSource;

        public bool IsMuted => this._isMuted;

        public float Volume => this._isMuted ? 0 : this._volume;

        public void Mute()
        {
            if (!this._isMuted)
            {
                this._isMuted = true;
                SetBGMVolume(0);
            }
        }

        public void Unmute()
        {
            if (this._isMuted)
            {
                this._isMuted = false;
                SetBGMVolume(Volume);
            }
        }

        public void SetBGMVolume(float newVolume)
        {
            this._volume = newVolume;
            this.UpdateSourceVolume();
        }

        public void FadeBGMOut(float fadeDuration)
        {
            float delay = 0;
            float toVolume = 0;
            this.StartCoroutine(this.FadeBGM(toVolume, delay, fadeDuration));
        }
        public void FadeBGMIn(AudioClip bgmClip, float delay, float fadeDuration)
        {
            this._audioSource.clip = bgmClip;
            this._audioSource.volume = 0;
            this._audioSource.Play();

            this.StartCoroutine(this.FadeBGM(this.Volume, delay, fadeDuration));
        }

        public void PlayBGM(AudioClip bgmClip, bool fade, float fadeDuration)
        {
            if (fade)
            {
                if (this._audioSource.isPlaying)
                {
                    this.FadeBGMOut(fadeDuration / 2);
                    this.FadeBGMIn(bgmClip, fadeDuration / 2, fadeDuration / 2);
                }
                else
                {
                    this.FadeBGMIn(bgmClip, 0, fadeDuration);
                }
            }
            else
            {
                // play immediately
                this._audioSource.volume = Volume;
                this._audioSource.clip = bgmClip;
                this._audioSource.Play();
            }
        }
        public void StopBGM(float fadeDuration = 0)
        {
            if (!this._audioSource.isPlaying)
            {
                return;
            }

            if (fadeDuration == 0)
            {
                this._audioSource.Stop();
            }
            else
            {
                this.FadeBGMOut(fadeDuration);
            }
        }

        protected IEnumerator FadeBGM(float volume, float delay, float duration)
        {
            yield return new WaitForSeconds(delay);
            float StartVolume = this._audioSource.volume;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float volumeFixedDelta = Time.fixedDeltaTime * (volume - StartVolume) / duration;
                this._audioSource.volume = Mathf.Clamp01(this._audioSource.volume + volumeFixedDelta);

                yield return new WaitForFixedUpdate();
                elapsed += Time.fixedDeltaTime;
            }
        }

        private void UpdateSourceVolume()
        {
            this._audioSource.volume = Volume;
        }

        public override void Bind()
        {
        }

        public override void Unbind()
        {
        }
    }
}
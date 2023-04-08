using System;
using Framework;
using Framework.Extensions;
using Framework.Helpers;
using Framework.Managers;
using Framework.Managers.Audio;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Game
{
    public abstract class Actor : SerializedMonoBehaviour, IActor
    {
        [BoxGroup("Components")]
        [Required]
        [SerializeField]
        protected SpriteRenderer _spriteRenderer = null;

        [BoxGroup("Components")]
        [Required]
        [SerializeField]
        private Animator _animator = null;

        [BoxGroup("Data")]
        [ShowInInspector]
        [HideInPrefabs]
        [VerticalGroup("Data/Health")]
        private int _currentHealth = 3;

        [BoxGroup("Data")]
        [SerializeField]
        [VerticalGroup("Data/Health")]
        private int _maxHealth = 3;

        [BoxGroup("Data")]
        [SerializeField]
        [VerticalGroup("Data/Health")]
        private float _movementSpeed = 3;

        private Timer _isMovingAnimatorTimer = new(0.1f);

        [Header("On Get Hit")]
        [Space]
        [SerializeField] protected GameObject _onGetHitPSPrefab;
        protected bool _isMaterialSwapLocked = false;
        protected Material _swapedMaterial = null;
        protected float _onGetHitVisualEffectDuration = 0.2f;
        [SerializeField] protected List<AudioClip> _onGetHitSFXs;
        [SerializeField] private GameObject _damagePopup;

        [Header("On Death")]
        [Space]
        [SerializeField] protected GameObject _onDeathPSPrefab;
        [SerializeField] protected List<GameObject> _onDeathPSPrefabList;
        [SerializeField] protected List<AudioClip> _onDeathSFXs;
        [SerializeField] protected GameObject _onDeathPrefab = null;

        Timer _hasMovedTimer = new(0.2f);
        protected bool onDeathVisualEffectStarted = false;
        protected bool _isDead = false;
        protected bool _defaultDeathAnimationHasStarted = false;

        public int CurrentHealth
        {
            get => this._currentHealth;
            set => this._currentHealth = value;
        }

        public int MaxHealth
        {
            get => this._maxHealth;
            set => this._maxHealth = value;
        }

        public float MovementSpeed
        {
            get => this._movementSpeed;
            set => this._movementSpeed = value;
        }

        protected virtual void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public virtual void Move(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                return;
            }

            this._spriteRenderer.flipX = direction.x < 0;

            this.transform.position += (Vector3)(direction * this._movementSpeed * Time.fixedDeltaTime);

            this._animator.SetBool("IsMoving", true);
            this._isMovingAnimatorTimer.Reset();
        }

        [Button]
        public void TakeDamage(IDamage damage)
        {
            this._currentHealth -= damage.Amount;

            if (_damagePopup)
            {
                var popup = Instantiate(_damagePopup, this.transform.position, Quaternion.identity);
                popup.GetComponent<UI_DamagePopup>().Bind(damage);
            }

            if (_onGetHitSFXs.TryGetRandom(out AudioClip getHitSFX))
            {
                Manager.Get<SFXManager>().PlayLocalSFX(getHitSFX, transform.position, isPitchRandomized: true);
            }

            if (this._currentHealth <= 0)
            {
                this.OnDeath();
            }
            else
            {
                this.StartCoroutine(this.OnGetHit_VisualEffect(transform));
            }
        }

        protected void FixedUpdate()
        {
            if (this._isMovingAnimatorTimer.IsTriggered())
            {
                this._animator.SetBool("IsMoving", false);
            }
        }

        protected virtual void OnDeath()
        {
            if (this._isDead)
            {
                return;
            }

            this._isDead = true;

            if (_onDeathPSPrefab != null)
            {
                GameObject.Instantiate(_onDeathPSPrefab, transform.position, Quaternion.identity, parent: null);
            }

            int count = _onDeathPSPrefabList?.Count ?? 0;
            for (int i = 0; i < count; i++)
            {
                GameObject.Instantiate(_onDeathPSPrefabList[i], transform.position, Quaternion.identity, parent: null);
            }

            if (_onDeathSFXs.TryGetRandom(out AudioClip deathSFX))
            {
                Manager.Get<SFXManager>().PlayLocalSFX(deathSFX, transform.position);
            }

            if (this._animator.HasAnimation(ActorAnimatorParamName.AnimationType.Dying.ToString()))
            {
                this._animator.SetBool(ActorAnimatorParamName.IS_ATTACKING, false);
                this._animator.SetBool(ActorAnimatorParamName.IS_MOVING, false);
                this._animator.SetBool(ActorAnimatorParamName.IS_DEAD, true);
            }
            else
            {
                OnDeath_DefaultCallback();
            }
        }

        protected virtual void OnDeath_DefaultCallback()
        {
            this.StartCoroutine(OnDeath_DefaultAnimation());
        }

        public IEnumerator OnDeath_DefaultAnimation()
        {
            if (!_defaultDeathAnimationHasStarted)
            {
                _defaultDeathAnimationHasStarted = true;

                const float animationDuration = 0.3f;
                const float growRate = 0.25f;
                const float alphaRate = 3f;
                float elapsedTime = 0;

                while (elapsedTime < animationDuration)
                {
                    yield return new WaitForFixedUpdate();
                    this._spriteRenderer.color -= new Color(0, 0, 0, alphaRate * Time.fixedDeltaTime);
                    transform.localScale += Vector3.one * growRate * Time.fixedDeltaTime;
                    elapsedTime += Time.fixedDeltaTime;
                }

                UnityEngine.GameObject.Destroy(this.gameObject);
                yield return 0;
            }
        }

        protected virtual IEnumerator OnGetHit_VisualEffect(Transform transform)
        {
            if (_onGetHitPSPrefab != null)
            {
                GameObject.Instantiate(_onGetHitPSPrefab, transform.position, Quaternion.identity);
            }

            if (this._animator.HasParam(ActorAnimatorParamName.IS_GETTING_HIT))
            {
                this._animator.SetBool(ActorAnimatorParamName.IS_GETTING_HIT, true);
            }

            if (!_isMaterialSwapLocked)
            {
                _isMaterialSwapLocked = true;
                _swapedMaterial = this._spriteRenderer.material;
                this._spriteRenderer.material = MaterialHelpers.GetMaterial(MaterialHelpers.MaterialType.OnGetHit);

                // sleep for duration
                yield return new WaitForSeconds(_onGetHitVisualEffectDuration);

                this._spriteRenderer.material = _swapedMaterial;
                _swapedMaterial = null;
                _isMaterialSwapLocked = false;
            }

            yield break;
        }
    }
}

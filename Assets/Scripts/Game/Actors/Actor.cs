using Framework;
using Framework.Extensions;
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
        private SpriteRenderer _spriteRenderer = null;

        [BoxGroup("Components")]
        [Required]
        [SerializeField]
        private Animator _animator = null;

        [BoxGroup("Data")]
        [ShowInInspector]
        [HideInEditorMode]
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

            if (_onGetHitSFXs.TryGetRandom(out AudioClip getHitSFX))
            {
                Manager.Get<SFXManager>().PlayLocalSFX(getHitSFX, transform.position, isPitchRandomized: true);
            }

            this.StartCoroutine(this.OnGetHit_VisualEffect(transform));
        }

        protected void FixedUpdate()
        {
            if (this._isMovingAnimatorTimer.IsTriggered())
            {
                this._animator.SetBool("IsMoving", false);
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

using Framework;
using Framework.Helpers;
using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using Sirenix.Utilities;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using Unity.Properties;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using Framework.Managers.Audio;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

namespace Game
{
    public class Player : Actor, IPlayer
    {
        [BoxGroup("Data/FireSystem")]
        [SerializeField]
        private int _baseMovementSpeed = 5;

        [BoxGroup("Data/FireSystem")]
        [HideInEditorMode]
        [ShowInInspector]
        private Timer _fireCooldownTimer = new(1);

        [BoxGroup("Data/FireSystem")]
        [SerializeField]
        private Transform _projectileSpawnpoint = null;

        [BoxGroup("Data/Souls")]
        [SerializeField]
        private Soul _primarySoul = null;

        [SerializeField]
        private ParticleSystem _soulParticles = null;

        [SerializeField]
        private SpriteRenderer _aimIndicatorSpriteRenderer = null;

        [BoxGroup("Data/Souls")]
        [SerializeField]
        private Soul[] _secondarySouls = null;
        private InputManager _inputManager;

        [SerializeField]
        private float _invulnerabilityTime;

        private Timer _invulnerabilityTimer;

        [BoxGroup("Data/Souls")]
        [SerializeField]
        private AudioClip _switchSoul = null;

        [BoxGroup("Data/Souls")]
        [SerializeField]
        private Soul _emptySoul = null;

        public SpriteRenderer knightAoeSprite;

        public event Action<IPlayer, IProjectile, Vector2> OnFire;
        public event Action<IPlayer, Vector2> OnMove;
        public event Action<IPlayer> OnFixedUpdate;
        public event Action<IPlayer, IProjectile, IEnemy> OnHit;
        public event Action<IPlayer, IDamage, IEnemy> OnTakeDamage;
        public event Action<ISoul, ISoul[]> OnSwapSoul;
        public event Action<Player> OnPauseInput;

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

        public Vector2 AimDirection => this._projectileSpawnpoint.transform.right;

        protected void Awake()
        {
            base.Awake();
            
            this._inputManager = Manager.Get<InputManager>();
            this._inputManager.Moved += this.Move;
            this._inputManager.Pointed += screenPosition =>
            {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                Vector2 aimDirection = (worldPosition - (Vector2)transform.position).normalized;
                this.Aim(aimDirection);
            };
            
            this._inputManager.Fired += () =>
            {
                this.Fire();
            };

            this._inputManager.Switched += () =>
            {
                this.SwitchSoul();
            };
            
            this._inputManager.Paused += () =>
            {
                OnPauseInput?.Invoke(this);
            };

            this._primarySoul?.Bind(this, true, false);

            for (int i = 0; i < this._secondarySouls?.Length; i++)
            {
                this._secondarySouls[i]?.Bind(this, false, false);
            }

            this._invulnerabilityTimer = new(this._invulnerabilityTime);

            this._emptySoul.SoulDurationTimer.Trigger();
        }

        protected void OnDestroy()
        {
            this._inputManager = Manager.Get<InputManager>();
            this._inputManager.Moved -= this.Move;
            this._inputManager.Pointed -= screenPosition =>
            {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                Vector2 aimDirection = (worldPosition - (Vector2)transform.position).normalized;
                this.Aim(aimDirection);
            };

            this._inputManager.Fired -= () =>
            {
                this.Fire();
            };

            this._inputManager.Switched -= () =>
            {
                this.SwitchSoul();
            };

            this._inputManager.Paused -= () =>
            {
                OnPauseInput?.Invoke(this);
            };
        }

        public void Start()
        {
            this.OnSwapSoul?.Invoke(this._primarySoul, this._secondarySouls);
        }

        protected void FixedUpdate()
        {
            base.FixedUpdate();
            this.UpdateStats();
            this.UpdateColor();
            this.UpdateSouls();

            Camera mainCamera = Camera.main;
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        }

        private void UpdateSouls()
        {
            bool anySoulHasExpired = false;

            if (this._primarySoul.HasExpired && this._primarySoul != this._emptySoul)
            {
                PickableSoul.SpawnedSouls.Remove(this._primarySoul);
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
                    PickableSoul.SpawnedSouls.Remove(this._secondarySouls[i]);
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

        private void UpdateStats()
        {
            this.UpdateMovementSpeed();
            this.UpdateAttackSpeed();
        }

        private void UpdateMovementSpeed()
        {
            float movementSpeed = this._baseMovementSpeed;

            for (int i = 0; i < this._secondarySouls.Length; i++)
            {
                movementSpeed *= (1 +  this._secondarySouls[i].PercentageMovementSpeedModifier);
            }

            this._movementSpeed = movementSpeed;
        }

        private void UpdateColor()
        {
            this._spriteRenderer.material.SetColor("_PrimaryColor", this._primarySoul.Color1);
            this._spriteRenderer.material.SetColor("_SecondaryColor", this._primarySoul.Color2);

            this._aimIndicatorSpriteRenderer.color = this._aimIndicatorSpriteRenderer.color.SetColor(this.PrimarySoul.Color2);

            ParticleSystem.MainModule main = this._soulParticles.main;
            main.startColor = this._primarySoul.Color2;
        }

        private void UpdateAttackSpeed()
        {
            float attackSpeed = this._primarySoul.BaseAttackSpeed;

            for (int i = 0; i < this._secondarySouls.Length; i++)
            {
                attackSpeed *= (1 + this._secondarySouls[i].PercentageAttackSpeedModifier);
            }

            float fireCooldown = Mathf.Clamp(1 / attackSpeed, 0.1f, 10f);

            this._fireCooldownTimer.Duration = fireCooldown;
        }

        public void Aim(Vector2 direction)
        {
            this._projectileSpawnpoint.transform.rotation = Quaternion.FromToRotation(Vector2.right, direction);
        }

        public void Fire()
        {
            if (this._fireCooldownTimer.IsRunning())
            {
                return;
            }

            GameObject projectilePrefab = this._primarySoul.ProjectilePrefab;
            GameObject psPrefab = this._primarySoul.ParticleSystemPrefab;

            for (int i = 0; i < this._primarySoul.NumberOfProjectiles; i++)
            {
                if (projectilePrefab)
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, this._projectileSpawnpoint.rotation.eulerAngles.z + UnityEngine.Random.Range(-this._primarySoul.SpreadAngle / 2, this._primarySoul.SpreadAngle));

                    Transform instance = Instantiate(projectilePrefab.transform, this._projectileSpawnpoint.position, rotation, parent: null);

                    IProjectile projectile = instance.GetComponent<IProjectile>();
                    
                    projectile.Hit += this.Projectile_Hit;
                    
                    OnFire?.Invoke(this, projectile, this._projectileSpawnpoint.transform.right);
                }
            }

            if (psPrefab)
            {
                Instantiate(psPrefab.transform, this._projectileSpawnpoint.position, this._projectileSpawnpoint.rotation,
                    parent: null);
            }

            Manager.Get<SFXManager>().PlayGlobalSFX(this._primarySoul.FireSFX, isPitchRandomized: true);

            this._fireCooldownTimer.Reset();           
        }

        private void Projectile_Hit(IProjectile projectile, IEnemy actor)
        {
            this.OnHit?.Invoke(this, projectile, actor);
        }

        public void AddSoul(Soul soul)
        {
            bool hasBeenAdded = false;

            if(this._primarySoul == this._emptySoul)
            {
                this._primarySoul.Unbind(this);
                this._primarySoul = soul;
                this._primarySoul.Bind(this, true, false);
                hasBeenAdded = true;
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

        protected override void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
        {
            // walk on soul and destroy it
            if (go.TryGetComponent(out PickableSoul pickableSoul) && collisionType == CollisionType.Enter)
            {
                Manager.Get<SFXManager>().PlayGlobalSFX(pickableSoul.PickupSFX);
                this.AddSoul(pickableSoul.SelectedSoul);
                Destroy(go);
            }

            if (this._invulnerabilityTimer.IsRunning())
            {
                return;
            }

            if (go.TryGetComponent(out IEnemy enemy))
            {
                if (!enemy.IsDead())
                {
                    Debug.Log("aie !");
                    Damage damage = new Damage(1, Color.white);
                    this.TakeDamage(damage);
                    this._invulnerabilityTimer.Reset();
                }
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            Manager.Get<GameManager>().GameOver();
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
using Framework;
using Framework.Helpers;
using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using Sirenix.Utilities;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using Unity.Properties;

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

        public event Action<IPlayer, IProjectile, Vector2> OnFire;
        public event Action<IPlayer, Vector2> OnMove;
        public event Action<IPlayer> OnFixedUpdate;
        public event Action<IPlayer, IProjectile, IEnemy> OnHit;
        public event Action<IPlayer, IDamage, IEnemy> OnTakeDamage;
        public event Action<ISoul, ISoul[]> OnSwapSoul;

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

            this._primarySoul?.Bind(this, true);

            for (int i = 0; i < this._secondarySouls?.Length; i++)
            {
                this._secondarySouls[i]?.Bind(this, false);
            }

            this._invulnerabilityTimer = new(this._invulnerabilityTime);
        }

        protected void FixedUpdate()
        {
            base.FixedUpdate();
            
            this.UpdateStats();
            this.UpdateColor();

            this._primarySoul.OnFixedUpdate(this);
            for (int i = 0; i < this._secondarySouls.Length; i++)
            {
                this._secondarySouls[i].OnFixedUpdate(this);
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

            this._fireCooldownTimer.Reset();           
        }

        private void Projectile_Hit(IProjectile projectile, IEnemy actor)
        {
            this.OnHit?.Invoke(this, projectile, actor);
        }

        public void SwitchSoul()
        {
            this._primarySoul.Unbind(this);
            this._secondarySouls[0].Unbind(this);

            Soul previousPrimarySoul = this._primarySoul;

            int length = this._secondarySouls.Length;

            this._primarySoul = this._secondarySouls[0];
            for (int i = 1; i < length; i++)
            {
                this._secondarySouls[i - 1] = this._secondarySouls[i];
            }

            this._secondarySouls[length - 1] = previousPrimarySoul;

            this._primarySoul.Bind(this, true);
            this._secondarySouls[length - 1].Bind(this, false);
            
            OnSwapSoul?.Invoke(this._primarySoul, this._secondarySouls);
        }

        protected override void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
        {
            if (this._invulnerabilityTimer.IsRunning())
            {
                return;
            }

            Debug.Log("aie !");
            Damage damage = new Damage(1);
            this.TakeDamage(damage);
            this._invulnerabilityTimer.Reset();
        }
    }
}
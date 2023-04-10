using Framework;
using Framework.Helpers;
using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Framework.Managers.Audio;

namespace Game
{
    public partial class Player : Actor, IPlayer
    {
        [BoxGroup("Wipe")]
        [SerializeField] private float _wipeRadius = 5;
        [BoxGroup("Wipe")]
        [SerializeField] private int _wipeDamage = 10;
        [BoxGroup("Wipe")]
        [SerializeField] private GameObject _wipePrefab;
        [BoxGroup("Wipe")]
        [SerializeField] private float _wipeForce = 5;

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

        [SerializeField]
        private SpriteRenderer _aimIndicatorSpriteRenderer = null;

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
        public event Action<Player> OnPauseInput;

        public Vector2 AimDirection => this._projectileSpawnpoint.transform.right;

        protected void Awake()
        {
            base.Awake();

            this._inputManager = Manager.Get<InputManager>();
            this._inputManager.Moved += this.Move;
            this._inputManager.Pointed += InputManager_Pointed;
            this._inputManager.Fired += this.Fire;
            this._inputManager.Switched += this.SwitchSoul;
            this._inputManager.Paused += InputManager_Paused;

            this._primarySoul?.Bind(this, true, false);

            for (int i = 0; i < this._secondarySouls?.Length; i++)
            {
                this._secondarySouls[i]?.Bind(this, false, false);
            }

            this._invulnerabilityTimer = new(this._invulnerabilityTime);

            this._emptySoul.SoulDurationTimer.Trigger();
        }

        private void InputManager_Paused()
        {
            OnPauseInput?.Invoke(this);
        }

        private void InputManager_Pointed(Vector2 screenPosition)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            Vector2 aimDirection = (worldPosition - (Vector2)transform.position).normalized;
            this.Aim(aimDirection);
        }

        protected void OnDestroy()
        {
            this._inputManager.Moved -= this.Move;
            this._inputManager.Pointed -= InputManager_Pointed;
            this._inputManager.Fired -= this.Fire;
            this._inputManager.Switched -= this.SwitchSoul;
            this._inputManager.Paused -= InputManager_Paused;
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

        private void UpdateStats()
        {
            this.UpdateMovementSpeed();
            this.UpdateAttackSpeed();
        }

        private void UpdateMovementSpeed()
        {
            float movementSpeed = this._baseMovementSpeed;

            movementSpeed *= (1 + this._primarySoul.PercentageMovementSpeedModifier);

            for (int i = 0; i < this._secondarySouls.Length; i++)
            {
                movementSpeed *= (1 + this._secondarySouls[i].PercentageMovementSpeedModifier);
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

        protected override void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
        {
            // walk on soul and destroy it
            if (go.TryGetComponent(out PickableSoul pickableSoul) && collisionType == CollisionType.Enter)
            {
                Manager.Get<SFXManager>().PlayGlobalSFX(pickableSoul.PickupSFX);
                this.AddSoul(pickableSoul.SelectedSoul);
                Destroy(go);
                WipeCloseEnemies();
            }

            if (this._invulnerabilityTimer.IsRunning())
            {
                return;
            }

            if (go.TryGetComponent(out IEnemy enemy) && collisionType == CollisionType.Enter)
            {
                if (!enemy.IsDead())
                {
                    Damage damage = new Damage(1, Color.white);
                    this.TakeDamage(damage);
                    this._invulnerabilityTimer.Reset();
                    WipeCloseEnemies();
                }
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            Manager.Get<GameManager>().GameOver();
        }

        internal void WipeCloseEnemies()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, this._wipeRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out Enemy enemy))
                {
                    Vector3 distance = (this.transform.position - enemy.transform.position);
                    enemy.TakeDamage(new Damage(_wipeDamage, Color.white, distance.normalized, this._wipeRadius - distance.magnitude));
                }
            }

            Instantiate(this._wipePrefab, this.transform.position, Quaternion.identity, parent: null);
        }
    }
}
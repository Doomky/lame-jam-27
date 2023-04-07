using Framework;
using Framework.Helpers;
using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using Sirenix.Utilities;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace Game
{
    public class Player : Actor, IPlayer
    {
        [BoxGroup("Data/FireSystem")]
        [HideInEditorMode]
        [ShowInInspector]
        private Timer _fireCooldownTimer = new(1);

        [BoxGroup("Data/FireSystem")]
        [SerializeField]
        private Transform _projectileSpawnpoint = null;

        [BoxGroup("Data/Souls")]
        [SerializeField]
        private ISoul _primarySoul = null;

        [BoxGroup("Data/Souls")]
        [SerializeField]
        private ISoul[] _secondarySouls = null;
        private InputManager _inputManager;

        public event Action<IPlayer, IProjectile, Vector2> OnFire;
        public event Action<IPlayer, Vector2> OnMove;
        public event Action<IPlayer> OnFixedUpdate;
        public event Action<IPlayer, IProjectile, IEnemy> OnHit;
        public event Action<IPlayer, IDamage, IEnemy> OnTakeDamage;

        public ISoul PrimarySoul
        {
            get => this._primarySoul;
            set => this._primarySoul = value;
        }

        public ISoul[] SecondarySouls
        {
            get => this._secondarySouls;
            set => this._secondarySouls = value;
        }

        protected void Awake()
        {
            this._inputManager = Manager.Get<InputManager>();
            this._inputManager.Moved += this.Move;
            this._inputManager.Pointed += screenPosition =>
            {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                Vector2 aimDirection = (worldPosition - (Vector2)transform.position).normalized;
                this.Aim(aimDirection);
            };
            this._inputManager.Clicked += (screenPosition, _) =>
            {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                Vector2 aimDirection = (worldPosition - (Vector2)transform.position).normalized;
                this.Fire(aimDirection);
            };

            this._primarySoul?.Bind(this, true);

            for (int i = 0; i < this._secondarySouls?.Length; i++)
            {
                this._secondarySouls[i]?.Bind(this, false);
            }
        }

        public void Aim(Vector2 direction)
        {
            this._projectileSpawnpoint.transform.rotation = Quaternion.FromToRotation(Vector2.right, direction);
        }

        public void Fire(Vector2 direction)
        {
            if (this._fireCooldownTimer.IsRunning())
            {
                return;
            }

            GameObject projectilePrefab = this._primarySoul.PrimaryFragment.ProjectilePrefab;
            GameObject psPrefab = this._primarySoul.PrimaryFragment.ParticleSystemPrefab;

            if (projectilePrefab)
            {
                Transform instance = Instantiate(projectilePrefab.transform, this._projectileSpawnpoint.position, this._projectileSpawnpoint.rotation, parent: null);

                IProjectile projectile = instance.GetComponent<IProjectile>();
                OnFire?.Invoke(this, projectile, direction);
            }

            if (psPrefab)
            {
                Instantiate(psPrefab.transform, this._projectileSpawnpoint.position, this._projectileSpawnpoint.rotation,
                    parent: null);
            }

            this._fireCooldownTimer.Reset();           
        }

        public void SwapMode()
        {
            this._primarySoul.Unbind(this);
            this._secondarySouls[0].Unbind(this);

            ISoul previousPrimarySoul = this._primarySoul;

            int length = this._secondarySouls.Length;

            this._primarySoul = this._secondarySouls[0];
            for (int i = 1; i < length; i++)
            {
                this._secondarySouls[i - 1] = this._secondarySouls[i];
            }

            this._secondarySouls[length - 1] = previousPrimarySoul;

            this._primarySoul.Bind(this, true);
            this._secondarySouls[length - 1].Bind(this, false);
        }
    }
}
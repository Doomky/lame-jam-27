using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class SecondaryFragment : ISecondaryFragment
    {
        [FoldoutGroup("Secondary Fragment")]
        [SerializeField] private int _level = 0;
        [FoldoutGroup("Secondary Fragment")]
        [SerializeField] private float _movementSpeedModifier = 1;
        [FoldoutGroup("Secondary Fragment")]
        [SerializeField] private float _attackSpeedModifier = 1;
        [FoldoutGroup("Secondary Fragment")]
        [SerializeField] private float _damageModifier = 1;
        [FoldoutGroup("Secondary Fragment")]
        [SerializeField] private float _maxHealthModifier = 1;
        [FoldoutGroup("Secondary Fragment")]
        [SerializeField] private float _projectileLifetimeModifier = 1;
        [FoldoutGroup("Secondary Fragment")]
        [SerializeField] private float _projectileSpeedModifier = 1;

        public int Level => _level;

        public float MovementSpeedModifier
        {
            get => _movementSpeedModifier;
            set => _movementSpeedModifier = value;
        }

        public float AttackSpeedModifier
        {
            get => _attackSpeedModifier;
            set => _attackSpeedModifier = value;
        }

        public float DamageModifier
        {
            get => _damageModifier;
            set => _damageModifier = value;
        }

        public float MaxHealthModifier
        {
            get => _maxHealthModifier;
            set => _maxHealthModifier = value;
        }

        public float ProjectileLifetimeModifier
        {
            get => _projectileLifetimeModifier;
            set => _projectileLifetimeModifier = value;
        }

        public float ProjectileSpeedModifier
        {
            get => _projectileSpeedModifier;
            set => _projectileSpeedModifier = value;
        }

        public virtual void OnFire(IPlayer player, IProjectile projectile, Vector2 direction)
        {
            Debug.Log($"Fragment '{this.GetType()}' => OnFire");
        }

        public virtual void OnMove(IPlayer player, Vector2 direction)
        {
            Debug.Log($"Fragment '{this.GetType()}' => OnFire");
        }

        public virtual void OnFixedUpdate(IPlayer player)
        {
        }

        public virtual void OnHit(IPlayer player, IProjectile projectile, IEnemy enemy)
        {
            Debug.Log($"Fragment '{this.GetType()}' => OnHit");
        }

        public virtual void OnTakeDamage(IPlayer player, IDamage damage, IEnemy enemy)
        {
            Debug.Log($"Fragment '{this.GetType()}' => OnTakeDamage");
        }
    }
}
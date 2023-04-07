using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "New/SecondaryFragment", fileName = "SecondaryFragment", order = 0)]
    public class SecondaryFragment : SerializedScriptableObject, ISecondaryFragment
    {
        private int _level;
        private float _movementSpeedModifier;
        private float _attackSpeedModifier;
        private float _damageModifier;
        private float _maxHealthModifier;
        private float _projectileLifetimeModifier;
        private float _projectileSpeedModifier;

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

        public void OnFire(IPlayer player, IProjectile projectile, Vector2 direction)
        {
            Debug.Log($"{this.GetType()} Shard '{name}' => OnFire");
        }

        public void OnMove(IPlayer player, Vector2 direction)
        {
            Debug.Log($"{this.GetType()} '{name}' => OnFire");
        }

        public void OnFixedUpdate(IPlayer player)
        {
        }

        public void OnHit(IPlayer player, IProjectile projectile, IEnemy enemy)
        {
            Debug.Log($"{this.GetType()} '{name}' => OnHit");
        }

        public void OnTakeDamage(IPlayer player, IDamage damage, IEnemy enemy)
        {
            Debug.Log($"{this.GetType()} '{name}' => OnTakeDamage");
        }
    }
}
using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Primary:
    /// - Damage scales with lifetime (distance).
    /// - projectile with high lifetime.
    /// 
    /// Secondary:
    /// - Projectile Lifetime +10%.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "New/SniperSoul", fileName = "SniperSoul", order = 0)]
    public class SniperSoul : Soul
    {
        [SerializeField]
        private float _distantDamageMultiplier = 1;

        [SerializeField]
        private float _bonusProjectileCooldown = 0.8f;

        [SerializeField]
        private GameObject _bonusProjectilePrefab = null;

        private Timer _bonusProjectileCooldodwnTimer = new(0.8f);

        public override void OnHit(IPlayer player, IProjectile projectile, IEnemy enemy)
        {
            base.OnHit(player, projectile, enemy);
            Debug.Log("SniperFragment => OnFire");

            if (this._isPrimary)
            {
                Damage damage = new((int)(_distantDamageMultiplier * projectile.CurrentLifetime));

                enemy.TakeDamage(damage);
            }
        }

        public override void OnFire(IPlayer player, IProjectile projectile, Vector2 direction)
        {
            base.OnFire(player, projectile, direction);

            if (!this._isPrimary)
            {
                if (this._bonusProjectileCooldodwnTimer.IsTriggered())
                {
                    Instantiate(_bonusProjectilePrefab, ((Player)player).transform.position, Quaternion.FromToRotation(Vector3.right, direction));
                    this._bonusProjectileCooldodwnTimer.Reset();
                }
            }
        }
    }
}

using System;
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
        [FoldoutGroup("Sniper Fragment")]
        [SerializeField] private float _distantDamageMultiplier = 1;
        
        public override void OnHit(IPlayer player, IProjectile projectile, IEnemy enemy)
        {
            base.OnHit(player, projectile, enemy);
            Debug.Log("SniperFragment => OnFire");

            Damage damage = new()
            {
                Amount = (int)(_distantDamageMultiplier * projectile.CurrentLifetime)
            };
            
            enemy.TakeDamage(damage);
        }
    }
}

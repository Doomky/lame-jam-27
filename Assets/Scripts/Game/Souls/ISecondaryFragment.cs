using UnityEngine;

namespace Game
{
    public interface ISecondaryFragment
    {
        int Level { get;  }

        float MovementSpeedModifier { get; set; }

        float AttackSpeedModifier { get; set; }

        float DamageModifier { get; set; }

        float ProjectileLifetimeModifier { get; set; }
        
        float ProjectileSpeedModifier { get; set; }

        void OnFire(IPlayer player, IProjectile projectile, Vector2 direction);

        void OnMove(IPlayer player, Vector2 direction);

        void OnFixedUpdate(IPlayer player);

        void OnHit(IPlayer player, IProjectile projectile, IEnemy enemy);

        void OnTakeDamage(IPlayer player, IDamage damage, IEnemy enemy);
    }
}

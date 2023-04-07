using System;
using UnityEngine;

namespace Game
{
    public interface IPlayer : IActor
    {
        event Action<IPlayer, IProjectile, Vector2> OnFire;

        event Action<IPlayer, Vector2> OnMove;

        event Action<IPlayer> OnFixedUpdate;

        event Action<IPlayer, IProjectile, IEnemy> OnHit;

        event Action<IPlayer, IDamage, IEnemy> OnTakeDamage;

        ISoul PrimarySoul { get; set; }
        
        ISoul[] SecondarySouls { get; set; }

        void Aim(Vector2 direction);

        /// <summary>
        /// Fire using primary soul projectiles and use modifiers from secondary.
        /// </summary>
        /// <param name="direction"></param>
        void Fire(Vector2 direction);

        /// <summary>
        /// Swap between souls mode, put the current soul in the secondary souls array and get the next soul from the array.
        /// </summary>
        void SwapMode();
    }
}

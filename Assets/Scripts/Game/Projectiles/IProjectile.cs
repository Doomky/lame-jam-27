
using System;
using System.Collections;

namespace Game
{
    public interface IProjectile
    {
        event Action<IProjectile, IEnemy> Hit;
        IDamage Damage { get; }

        float MovementSpeed { get; set; }

        float CurrentLifetime { get; }
        
        float MaxLifetime { get; set; }
    }
}

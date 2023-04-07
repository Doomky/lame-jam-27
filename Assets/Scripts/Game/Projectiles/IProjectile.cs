using UnityEngine;

namespace Game
{
    public interface IProjectile
    {
        IDamage Damage { get; }

        float MovementSpeed { get; set; }

        float CurrentLifetime { get; }
        
        float MaxLifetime { get; set; }
    }
}

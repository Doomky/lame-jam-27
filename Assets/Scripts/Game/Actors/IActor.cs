using Unity.Properties;
using UnityEngine;

namespace Game
{
    public interface IActor
    {        
        int CurrentHealth { get; set; }
        
        int MaxHealth { get; set; }

        float MovementSpeed { get; set; }

        bool IsDead();

        void Move(Vector2 direction);

        void TakeDamage(IDamage damage);
    }
}

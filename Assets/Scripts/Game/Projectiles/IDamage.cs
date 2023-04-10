using UnityEngine;

namespace Game
{
    public interface IDamage
    {
        int Amount { get; }
        Color Color { get; }
        Vector2 KnockbackDirection { get; set; }
        float KnockbackForce { get; set; }
    }
}

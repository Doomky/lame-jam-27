using Sirenix.OdinInspector;

namespace Game
{
    public abstract class Projectile : SerializedMonoBehaviour, IProjectile
    {
        public abstract IDamage Damage { get; set; }
        public abstract float MovementSpeed { get; set; }
        public abstract float Lifetime { get; set; }
    }
}

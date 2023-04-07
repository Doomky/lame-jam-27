using UnityEngine;

namespace Game
{
    public interface IPrimaryFragment : ISecondaryFragment
    {
        GameObject ParticleSystemPrefab { get; set; }

        IProjectile Projectile { get; set; }
    }
}

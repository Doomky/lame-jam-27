using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public interface IPrimaryFragment : ISecondaryFragment
    {
        GameObject ParticleSystemPrefab { get; set; }

        GameObject ProjectilePrefab { get; }
    }
}

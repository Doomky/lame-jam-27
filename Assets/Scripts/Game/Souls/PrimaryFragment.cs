using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "New/PrimaryFragment", fileName = "PrimaryFragment", order = 0)]
    public class PrimaryFragment: SecondaryFragment, IPrimaryFragment
    {
        private GameObject _particleSystemPrefab;
        private IProjectile _projectile;

        public GameObject ParticleSystemPrefab
        {
            get => _particleSystemPrefab;
            set => _particleSystemPrefab = value;
        }

        public IProjectile Projectile
        {
            get => _projectile;
            set => _projectile = value;
        }
    }
}
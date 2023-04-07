using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PrimaryFragment: SecondaryFragment, IPrimaryFragment
    {
        [FoldoutGroup("Primary Fragment")]
        [SerializeField] private GameObject _particleSystemPrefab;
        [FoldoutGroup("Primary Fragment")]
        [SerializeField] private IProjectile _projectile;

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
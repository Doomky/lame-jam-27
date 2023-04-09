using System;
using UnityEngine;

namespace Game
{
    public class ExplosiveProjectile : SingleCollisionPipelineMonoBehaviour, IProjectile
    {
        [SerializeField] private float _wipeRadius;
        [SerializeField] private Damage _damage;
        [SerializeField] private GameObject _wipePrefab;
        public event Action<IProjectile, IEnemy> Hit;

        public IDamage Damage => _damage;

        public float MovementSpeed { get; set; }

        public float CurrentLifetime { get; set; }

        public float MaxLifetime { get; set; }
        
        protected override void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
        {
            base.OnCollision(go, collisionPosition, isTrigger, collisionType);

            if (go.TryGetComponent(out IActor actor) && collisionType == CollisionType.Enter)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, this._wipeRadius);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent(out IEnemy enemy))
                    {
                        enemy.TakeDamage(this.Damage);
                        this.Hit?.Invoke(this, enemy);
                    }
                }
                
                if (colliders.Length > 0)
                {
                    Instantiate(this._wipePrefab, this.transform.position, Quaternion.identity, parent: null);
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }
}
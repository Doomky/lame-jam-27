using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game
{
    public class Projectile : SingleCollisionPipelineMonoBehaviour, IProjectile
    {
        [SerializeField]
        private Damage _damage = null;

        [SerializeField]
        private AnimationCurve _movementSpeedCurve = AnimationCurve.Linear(0,1,1,1);

        [SerializeField]
        private float _movementSpeed = 5;

        private float _currentLifeTime = 5;

        [SerializeField]
        private float _maxLifeTime = 5;

        public event Action<IProjectile, IEnemy> Hit;

        public IDamage Damage 
        { 
            get => this._damage; 
        }
        
        public float MovementSpeed 
        { 
            get => this._movementSpeed; 
            set => this._movementSpeed = value; 
        }

        public float CurrentLifetime 
        { 
            get => this._currentLifeTime; 
            set => this._currentLifeTime = value;
        }
        public float MaxLifetime 
        {
            get => this._maxLifeTime; 
            set => this._maxLifeTime = value; 
        }

        protected void Awake()
        {
            this._currentLifeTime = 0;
        }

        protected void FixedUpdate()
        {
            this.transform.position += (Vector3)(this.transform.right * this._movementSpeedCurve.Evaluate(this._maxLifeTime - this._currentLifeTime) * this._movementSpeed * Time.fixedDeltaTime);
            this._currentLifeTime += Time.fixedDeltaTime;

            if (this._currentLifeTime >= this.MaxLifetime)
            {
                Destroy(this.gameObject);
            }
        }

        protected override void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
        {
            base.OnCollision(go, collisionPosition, isTrigger, collisionType);

            if (go.TryGetComponent(out IActor actor) && collisionType == CollisionType.Enter)
            {
                actor.TakeDamage(this._damage);

                this.Hit?.Invoke(this, actor as IEnemy);

                GameObject.Destroy(this.gameObject);
            }
        }
    }
}

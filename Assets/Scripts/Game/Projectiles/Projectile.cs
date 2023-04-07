using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class Projectile : SerializedMonoBehaviour, IProjectile
    {
        [SerializeField]
        private IDamage _damage = null;

        [SerializeField]
        private float _movementSpeed = 5;

        [SerializeField]
        private float _currentLifeTime = 5;

        [SerializeField]
        private float _maxLifeTime = 5;

        public IDamage Damage 
        { 
            get => this._damage; 
            set => this._damage = value; 
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
            this._currentLifeTime = this._maxLifeTime;
        }

        protected void FixedUpdate()
        {
            this.transform.position += (Vector3)(this.transform.right * this._movementSpeed * Time.fixedDeltaTime);
            this._currentLifeTime -= Time.fixedDeltaTime;

            if (this._currentLifeTime <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

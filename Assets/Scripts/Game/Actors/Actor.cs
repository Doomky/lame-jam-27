using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public abstract class Actor : SerializedMonoBehaviour, IActor
    {
        [BoxGroup("Components")]
        [Required]
        [SerializeField]
        private SpriteRenderer _spriteRenderer = null;

        [BoxGroup("Data")]
        [ShowInInspector]
        [HideInEditorMode]
        [VerticalGroup("Data/Health")]
        private int _currentHealth = 3;

        [BoxGroup("Data")]
        [SerializeField]
        [VerticalGroup("Data/Health")]
        private int _maxHealth = 3;

        [BoxGroup("Data")]
        [SerializeField]
        [VerticalGroup("Data/Health")]
        private float _movementSpeed = 3;

        public int CurrentHealth 
        { 
            get => this._currentHealth; 
            set => this._currentHealth = value; 
        }

        public int MaxHealth 
        { 
            get => this._maxHealth; 
            set => this._maxHealth = value; 
        }

        public float MovementSpeed 
        { 
            get => this._movementSpeed; 
            set => this._movementSpeed = value; 
        }

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                return;
            }

            this._spriteRenderer.flipX = direction.x < 0;
                
            this.transform.position += (Vector3)(direction * this._movementSpeed * Time.fixedDeltaTime);
        }

        public void TakeDamage(IDamage damage)
        {
            this._currentHealth -= damage.Amount;
        }
    }
}

using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Damage : IDamage
    {
        [SerializeField] private int _amount;
        [SerializeField] private Color _color;
        [SerializeField] private Vector2 _knockbackDirection;
        [SerializeField] private float _knockbackForce;

        public int Amount => _amount;
        public Color Color => _color;
        public Vector2 KnockbackDirection
        {
            get => _knockbackDirection;
            set => _knockbackDirection = value;
        }

        public float KnockbackForce
        {
            get => _knockbackForce;
            set => _knockbackForce = value;
        }

        public Damage(int amount, Color color, Vector2 knockbackDirection = default, float knockbackForce = 0)
        {
            _amount = amount;
            _color = color;
            _knockbackDirection = knockbackDirection;
            _knockbackForce = knockbackForce;
        }
    }
}
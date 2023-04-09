using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Damage : IDamage
    {
        [SerializeField]
        private int _amount;

        [SerializeField]
        private Color _color;

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }

        public Color color
        {
            get => _color;
            set => _color = value;
        }

        public Damage(int amount, Color color)
        {
            _amount = amount;
            _color = color;
        }
    }
}

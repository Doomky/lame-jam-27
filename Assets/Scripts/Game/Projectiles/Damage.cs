using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Damage : IDamage
    {
        [SerializeField]
        private int _amount;

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }

        public Damage()
        {
            _amount = 0;
        }


        public Damage(int amount)
        {
            _amount = amount;
        }
    }
}

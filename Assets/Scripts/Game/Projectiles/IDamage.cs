using System;
using UnityEngine;

namespace Game
{
    public interface IDamage
    {
        int Amount { get; set; }
    }

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
    }
}

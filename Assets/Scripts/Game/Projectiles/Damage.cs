using Sirenix.OdinInspector;

namespace Game
{
    [System.Serializable]
    public struct Damage : IDamage
    {
        [ShowInInspector]
        private int _amount;

        public Damage(int amount)
        {
            this._amount = amount;
        }

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }
    }
}

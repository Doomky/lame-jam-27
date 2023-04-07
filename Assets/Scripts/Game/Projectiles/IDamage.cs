namespace Game
{
    public interface IDamage
    {
        int Amount { get; set; }
    }

    public struct Damage : IDamage
    {
        private int _amount;

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }
    }
}

namespace Game
{

    public interface ISoul
    {
        /// <summary>
        /// Subscribe to events mainly.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="isPrimary"></param>
        void Bind(IPlayer player, bool isPrimary);

        /// <summary>
        /// Unsubscribe to events mainly.
        /// </summary>
        void Unbind(IPlayer player);

        IPrimaryFragment PrimaryFragment { get; set; }
        
        ISecondaryFragment SecondaryFragment { get; set; }
    }
}

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
        void Unbind();

        IPrimaryFragment PrimaryFragement { get; set; }
        
        ISecondaryFragment SecondaryFragement { get; set; }
    }
}

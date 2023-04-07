namespace Game
{
    public interface IPrimaryFragment : ISecondaryFragment
    {
        IProjectile Projectile { get; set; }
    }
}

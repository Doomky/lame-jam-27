using UnityEngine;

namespace Game
{
    public interface IDamage
    {
        int Amount { get; set; }
        Color color { get; set; }
    }
}

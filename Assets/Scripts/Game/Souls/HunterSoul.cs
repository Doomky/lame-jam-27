using Framework;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "New/HunterSoul", fileName = "HunterSoul", order = 0)]
    public class HunterSoul : Soul
    {
        [SerializeField]
        private GameObject _trapPrefab = null;
        
        [SerializeField]
        private float _trapCooldown = 1f;

        [SerializeField]
        private float _trapOffset = 0.125f;

        private Timer _trapCooldownTimer = new(1);

        public override void Bind(IPlayer player, bool isPrimary, bool isSwap)
        {
            base.Bind(player, isPrimary, isSwap);

            if (!isSwap)
            {
                this._trapCooldownTimer.Reset();
            }
        }

        public override void OnFixedUpdate(IPlayer player)
        {
            base.OnFixedUpdate(player);

            if (!this._isPrimary)
            {
                this._trapCooldownTimer.Duration = this._trapCooldown;
                if (this._trapCooldownTimer.IsTriggered())
                {
                    // Spawn a trap next to the player
                    GameObject trap = Instantiate(this._trapPrefab, ((Player)player).transform.position + (Vector3)(Random.insideUnitCircle * this._trapOffset), Quaternion.identity);

                    this._trapCooldownTimer.Reset();
                }
            }
        }
    }
}

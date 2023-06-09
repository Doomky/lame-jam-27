﻿using Framework;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "New/DrunkenSoul", fileName = "DrunkenSoul", order = 0)]
    public class DrunkenSoul : Soul
    {
        [SerializeField]
        private GameObject _bonusProjectile = null;

        [SerializeField]
        private float _bonusProjectileCooldown = 1f;

        private Timer _bonusProjectileCooldownTimer = new(1);

        public override void Bind(IPlayer player, bool isPrimary, bool isSwap)
        {
            base.Bind(player, isPrimary, isSwap);

            if (!isSwap)
            {
                this._bonusProjectileCooldownTimer.Reset();
            }
        }

        public override void OnFixedUpdate(IPlayer player)
        {
            base.OnFixedUpdate(player);

            this._bonusProjectileCooldownTimer.Duration = this._bonusProjectileCooldown;
            if (this._bonusProjectileCooldownTimer.IsTriggered())
            {
                // Instantiate a bonus projectile in a random direction.

                Vector2 direction = Random.insideUnitCircle.normalized;

                GameObject projectile = Instantiate(this._bonusProjectile, ((Player)player).transform.position, Quaternion.FromToRotation(Vector3.right, direction));

                this._bonusProjectileCooldownTimer.Reset();
            }
        }
    }
}

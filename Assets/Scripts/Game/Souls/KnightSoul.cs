using Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Primary:
    ///     - Projectile is a close range slash:
    ///         - low lifetime
    ///         - low speed
    ///         - high cleave
    ///         - high damage
    ///     
    /// Secondary:
    ///     - Health +1 per level.
    /// </summary>
    
    [Serializable]
    [CreateAssetMenu(menuName = "New/KnightSoul", fileName = "KnightSoul", order = 0)]
    public class KnightSoul : Soul
    {
        [SerializeField]
        private int _meleeDamage;

        [SerializeField]
        private int _meleeDamageRange = 2;

        [SerializeField]
        private float _meleeDamageCooldown = 0.3f;

        private Timer _meleeDamageCooldownTimer = new(0.3f);

        public override void Bind(IPlayer player, bool isPrimary, bool isSwap)
        {
            base.Bind(player, isPrimary, isSwap);

            if (!isSwap)
            {
                this._meleeDamageCooldownTimer.Reset();
            }
        }

        public override void OnFixedUpdate(IPlayer player)
        {
            base.OnFixedUpdate(player);

            this._meleeDamageCooldownTimer.Duration = this._meleeDamageCooldown;

            if (!this._isPrimary)
            {
                if (this._meleeDamageCooldownTimer.IsTriggered())
                {
                    List<Enemy> enemies = Enemy.Enemies;
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        Enemy enemy = enemies[i];

                        if (Vector2.Distance(((Player)player).transform.position, enemy.transform.position) < _meleeDamageRange)
                        {
                            enemy.TakeDamage(new Damage(this._meleeDamage, this.Color2));
                        }
                    }

                    this._meleeDamageCooldownTimer.Reset();
                }
            }
        }
    }
}

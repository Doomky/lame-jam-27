﻿using Framework;
using System;
using System.Collections.Generic;
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

        [SerializeField]
        private GameObject aoeSpriteSample;

        private GameObject _aoeSprite;

        public override void Bind(IPlayer player, bool isPrimary, bool isSwap)
        {
            base.Bind(player, isPrimary, isSwap);

            if(!isPrimary)
            {
                Player player1 = player as Player;
                this._aoeSprite = Instantiate(aoeSpriteSample, player1.transform.position, Quaternion.identity);
                this._aoeSprite.gameObject.SetActive(true);
            }
            

            if (!isSwap)
            {
                this._meleeDamageCooldownTimer.Reset();
            }
        }

        public override void Unbind(IPlayer player)
        {
            base.Unbind(player);

            Destroy(this._aoeSprite);
        }

        public override void OnFixedUpdate(IPlayer player)
        {
            base.OnFixedUpdate(player);

            Player player1 = player as Player;
            if(this._aoeSprite)
            {
                this._aoeSprite.gameObject.transform.position = player1.transform.position;
            }
            

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

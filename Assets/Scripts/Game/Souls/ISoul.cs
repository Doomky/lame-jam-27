﻿using Sirenix.OdinInspector;
using UnityEngine;

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

        Texture2D Image { get; }
        string Name { get; }
        string Description { get; }
        Color Color1 { get; }
        Color Color2 { get; }
        
        float BaseAttackSpeed { get; }

        float SpreadAngle { get; }

        int NumberOfProjectiles { get; }

        float MovementSpeedModifier { get; }

        float PercentageAttackSpeedModifier { get; }

        float ProjectileLifetimeModifier { get; }

        float ProjectileSpeedModifier { get; }
    }
}

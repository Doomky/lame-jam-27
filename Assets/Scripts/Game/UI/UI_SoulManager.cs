using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class UI_SoulManager : SerializedMonoBehaviour
    {
        [SerializeField] private UI_Soul[] _souls;
        [SerializeField] private UI_Soul[] CurentsSouls;
        [SerializeField] private UI_Soul[] NextSouls;

        private Player _player;

        private void LateUpdate()
        {
            BindPlayer();
        }

        private void BindPlayer()
        {
            if (!_player)
            {
                _player = FindObjectOfType<Player>();
                if (_player)
                {
                    _player.OnSwapSoul += Player_OnSwapSoul;
                    Player_OnSwapSoul(_player.PrimarySoul, _player.SecondarySouls);
                }
            }
        }

        private void Player_OnSwapSoul(ISoul primarySoul, ISoul[] secondarySoul)
        {
            _souls[0].Bind(primarySoul, true);
            for (int i = 0; i < secondarySoul.Length; i++)
            {
                _souls[i + 1].Bind(secondarySoul[i], true);
                _souls[i + 1].Bind(secondarySoul[i], true);
            }
        }
    }
}
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
    public class UI_SoulManager : SerializedMonoBehaviour
    {
        [SerializeField] private UI_Soul[] _souls;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private UI_Soul[] _descriptionSouls; 
        
        private ISoul[] soul;
        private Player _player;

        private void Awake()
        {
            _pauseMenu.SetActive(false);
        }

        private void LateUpdate()
        {
            if (!_player) BindPlayer();
        }

        private void BindPlayer()
        {
            _player = FindObjectOfType<Player>();
            if (_player)
            {
                _player.OnSwapSoul += Player_OnSwapSoul;
                _player.OnPauseInput += PauseMenu;
                Player_OnSwapSoul(_player.PrimarySoul, _player.SecondarySouls);
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

        public void PauseMenu(Player player)
        {
            _descriptionSouls[0].Bind(player.PrimarySoul, true, true);
            for (int i = 0; i < player.SecondarySouls.Length; i++)
            {
                _descriptionSouls[i + 1].Bind(player.SecondarySouls[i], true, true);
            }
            
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);
            Time.timeScale = _pauseMenu.activeSelf ? 0 : 1;
        }
    }
}
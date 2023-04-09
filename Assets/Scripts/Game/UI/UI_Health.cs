using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UI_Health : MonoBehaviour
    {
        [SerializeField]
        private Image[] _hearts = null;

        [SerializeField]
        private Sprite _fullHeart = null;

        [SerializeField]
        private Sprite _emptyHeart = null;

        public void Update()
        {
            Player player = FindObjectOfType<Player>();
            if (player == null)
            {
                return;
            }

            int currentHealth = player.CurrentHealth;
            int maxHealth = player.MaxHealth;

            int i = 0;
            
            for (; i < currentHealth; i++)
            {
                this._hearts[i].sprite = this._fullHeart;
            }

            for (; i < maxHealth; i++)
            {
                this._hearts[i].sprite = this._emptyHeart;
            }
        }
    }
}
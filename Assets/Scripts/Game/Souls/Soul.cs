using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "New/Soul", fileName = "Soul", order = 0)]
    public class Soul : SerializedScriptableObject, ISoul
    {
        [SerializeField, InlineEditor] private IPrimaryFragment _primaryFragment = new PrimaryFragment();
        [SerializeField, InlineEditor] private ISecondaryFragment _secondaryFragment = new SecondaryFragment();

        public IPrimaryFragment PrimaryFragment
        {
            get => _primaryFragment;
            set => _primaryFragment = value;
        }

        public ISecondaryFragment SecondaryFragment
        {
            get => _secondaryFragment;
            set => _secondaryFragment = value;
        }

        public void Bind(IPlayer player, bool isPrimary)
        {
            if (isPrimary)
            {
                Bind(player, PrimaryFragment);
                Bind(player, SecondaryFragment);
            }
            else
            {
                Bind(player, SecondaryFragment);
            }
        }

        private void Bind(IPlayer player, ISecondaryFragment fragment)
        {
            player.OnFire        += fragment.OnFire;
            player.OnHit         += fragment.OnHit;
            player.OnMove        += fragment.OnMove;
            player.OnFixedUpdate += fragment.OnFixedUpdate;
            player.OnTakeDamage  += fragment.OnTakeDamage;
        }

        void ISoul.Unbind(IPlayer player)
        {
            player.OnFire        -= PrimaryFragment.OnFire;
            player.OnHit         -= PrimaryFragment.OnHit;
            player.OnMove        -= PrimaryFragment.OnMove;
            player.OnFixedUpdate -= PrimaryFragment.OnFixedUpdate;
            player.OnTakeDamage  -= PrimaryFragment.OnTakeDamage;
            
            player.OnFire        -= SecondaryFragment.OnFire;
            player.OnHit         -= SecondaryFragment.OnHit;
            player.OnMove        -= SecondaryFragment.OnMove;
            player.OnFixedUpdate -= SecondaryFragment.OnFixedUpdate;
            player.OnTakeDamage  -= SecondaryFragment.OnTakeDamage;
        }
    }
}
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "New/Soul", fileName = "Soul", order = 0)]
    public class Soul : SerializedScriptableObject, ISoul
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Texture2D _image;
        [SerializeField] private Color _color1;
        [SerializeField] private Color _color2;
        [SerializeField, InlineEditor] private IPrimaryFragment _primaryFragment = new PrimaryFragment();
        [SerializeField, InlineEditor] private ISecondaryFragment _secondaryFragment = new SecondaryFragment();

        public IPrimaryFragment PrimaryFragment
        {
            get => _primaryFragment;
        }

        public ISecondaryFragment SecondaryFragment
        {
            get => _secondaryFragment;
        }

        public Texture2D Image => _image;

        public string Name => _name;

        public string Description => _description;

        public Color Color1 => _color1;

        public Color Color2 => _color2;

        public void Bind(IPlayer player, bool isPrimary)
        {
            if (isPrimary)
            {
                // TODO COLOR 
                Bind(player, PrimaryFragment);
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
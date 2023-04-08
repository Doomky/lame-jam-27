using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UI_Soul : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI data;
        [SerializeField] private Image image;

        public void Bind(ISoul Soul, bool showText)
        {
            if (showText)
            {
                var dataText = "";

                dataText += Soul.Name; 
                dataText += Soul.Description; 
                
                ////dataText += "Active Effect\n";
                ////dataText += Soul.PrimaryFragment.DamageModifier != 1 ? $"Damage x{Soul.PrimaryFragment.DamageModifier}\n" : "";
                ////dataText += Soul.PrimaryFragment.AttackSpeedModifier != 1 ? $"Attack Speed x{Soul.PrimaryFragment.AttackSpeedModifier}\n" : "";
                ////dataText += Soul.PrimaryFragment.MaxHealthModifier != 1 ? $"Health x{Soul.PrimaryFragment.MaxHealthModifier}\n" : "";
                ////dataText += Soul.PrimaryFragment.MovementSpeedModifier != 1 ? $"Speed x{Soul.PrimaryFragment.MovementSpeedModifier}\n" : "";
                ////dataText += Soul.PrimaryFragment.ProjectileLifetimeModifier != 1 ? $"Projectile durability x{Soul.PrimaryFragment.ProjectileLifetimeModifier}\n" : "";
                ////dataText += Soul.PrimaryFragment.ProjectileSpeedModifier != 1 ? $"Projectile speed x{Soul.PrimaryFragment.ProjectileSpeedModifier}\n" : "";

                ////dataText += "Passive Effect\n";
                ////dataText += Soul.SecondaryFragment.DamageModifier != 1 ? $"Damage x{Soul.SecondaryFragment.DamageModifier}\n" : "";
                ////dataText += Soul.SecondaryFragment.AttackSpeedModifier != 1 ? $"Attack Speed x{Soul.SecondaryFragment.AttackSpeedModifier}\n" : "";
                ////dataText += Soul.SecondaryFragment.MaxHealthModifier != 1 ? $"Health x{Soul.SecondaryFragment.MaxHealthModifier}\n" : "";
                ////dataText += Soul.SecondaryFragment.MovementSpeedModifier != 1 ? $"Speed x{Soul.SecondaryFragment.MovementSpeedModifier}\n" : "";
                ////dataText += Soul.SecondaryFragment.ProjectileLifetimeModifier != 1 ? $"Projectile durability x{Soul.SecondaryFragment.ProjectileLifetimeModifier}\n" : "";
                ////dataText += Soul.SecondaryFragment.ProjectileSpeedModifier != 1 ? $"Projectile speed x{Soul.SecondaryFragment.ProjectileSpeedModifier}\n" : "";

                data.text = dataText;
            }

            this.image.color = Soul.Color1;
            
            this.data.gameObject.SetActive(showText);
        }

        public void Unbind()
        {
            this.data.gameObject.SetActive(false);
        }
    }
}
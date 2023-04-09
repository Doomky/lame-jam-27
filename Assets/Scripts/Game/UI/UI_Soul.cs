using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UI_Soul : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI data;

        [SerializeField] 
        private Image image;

        [SerializeField]
        private Slider _timeLeftSlder = null;

        private ISoul _soul;

        public void Bind(ISoul Soul, bool showDurationLeft, bool showDescription = false)
        {
            this._soul = Soul;

            var dataText = Soul.Name + "\n" + Soul.Description;
            data.text = dataText;
            data.gameObject.SetActive(showDescription);

            _timeLeftSlder.gameObject.SetActive(showDurationLeft);
            
            this.image.material = new Material(this.image.material);
            this.image.material.SetColor("_PrimaryColor", Soul.Color1);
            this.image.material.SetColor("_SecondaryColor", Soul.Color2);
        }

        public void Update()
        {
            if (this._soul != null)
            {
                this._timeLeftSlder.value = this._soul.SoulDurationTimer.TimeLeftToTrigger() / this._soul.SoulDurationTimer.Duration;
                this._timeLeftSlder.gameObject.SetActive(this._soul.SoulDurationTimer.IsRunning());
            }
        }

        public void Unbind()
        {
            this.data.gameObject.SetActive(false);
        }
    }
}
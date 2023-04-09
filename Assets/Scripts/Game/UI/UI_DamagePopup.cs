using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class UI_DamagePopup : SerializedMonoBehaviour
    {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private float spawnDelta = 0.2f;
        [SerializeField] private float speedDelta = 1;
        [SerializeField] private float maxLifetime = 0.5f;
        
        private Vector3 _direction;
        private float lifetime;
        
        public void Bind(IDamage damage)
        {
            this.transform.position += (Vector3)Random.insideUnitCircle * spawnDelta;
            this._text.text = damage.Amount.ToString();
            this._direction = Random.insideUnitCircle * speedDelta;
            lifetime = maxLifetime;
            this._text.color = (0.5f * damage.color + 0.5f * UnityEngine.Color.white);
        }

        private void Update()
        {
            this.transform.position += _direction * Time.deltaTime;

            lifetime -= Time.deltaTime;
            _text.color = new Color(_text.color.r,_text.color.g,_text.color.b, lifetime / maxLifetime);
            if (lifetime <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
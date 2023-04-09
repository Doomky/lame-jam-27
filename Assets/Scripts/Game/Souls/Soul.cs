using Framework;
using Sirenix.OdinInspector;
using Unity.Properties;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "New/Soul", fileName = "Soul", order = 0)]
    public class Soul : SerializedScriptableObject, ISoul
    {
        [BoxGroup("Data")]
        [SerializeField] 
        private string _name;

        [BoxGroup("Data")]
        [SerializeField, TextArea] 
        private string _description;

        [BoxGroup("Data")]
        [SerializeField]
        private Texture2D _image;

        [BoxGroup("Data")]
        [SerializeField]
        private Color _color1;

        [BoxGroup("Data")]
        [SerializeField]
        private Color _color2;

        [BoxGroup("Primary")]
        [SerializeField]
        private  GameObject _projectilePrefab;

        [BoxGroup("Primary")]
        [SerializeField]
        private  GameObject _particleSystemPrefab;

        [BoxGroup("Primary")]
        [SerializeField]
        private float _baseAttackSpeed = 1;

        [BoxGroup("Primary")]
        [SerializeField]
        private float _spreadAngle = 0;

        [BoxGroup("Primary")]
        [SerializeField]
        private int _numberOfProjectiles = 0;

        [BoxGroup("Primary")]
        [SerializeField]
        private AudioClip _fireSFX = null;

        [BoxGroup("Secondary")]
        [SerializeField] 
        private  float _percentageMovementSpeedModifier = 0;

        [BoxGroup("Secondary")]
        [SerializeField]
        private  float _percentageAttackSpeedModifier = 0;
        
        [BoxGroup("Secondary")]
        [SerializeField]
        private  float _porojectileLifetimeModifier = 0;
        
        [BoxGroup("Secondary")]
        [SerializeField] 
        private  float _projectileSpeedModifier = 0;

        [BoxGroup("Data")]
        [SerializeField]
        private bool _isEmpty;

        protected bool _isPrimary = false;

        public Texture2D Image => _image;

        public string Name => _name;

        public string Description => _description;

        public Color Color1 => _color1;

        public Color Color2 => _color2;

        public GameObject ProjectilePrefab => this._projectilePrefab;

        public GameObject ParticleSystemPrefab => this._particleSystemPrefab;

        public float BaseAttackSpeed => this._baseAttackSpeed;

        public float SpreadAngle => this._spreadAngle;

        public int NumberOfProjectiles => this._numberOfProjectiles;

        public float PercentageMovementSpeedModifier => this._percentageMovementSpeedModifier;

        public float PercentageAttackSpeedModifier => this._percentageAttackSpeedModifier;

        public float ProjectileLifetimeModifier => this._porojectileLifetimeModifier;

        public float ProjectileSpeedModifier => this._projectileSpeedModifier;

        public AudioClip FireSFX => this._fireSFX;

        private Timer _soulDurationTimer = new(20f);

        public virtual void Bind(IPlayer player, bool isPrimary, bool isSwap)
        {
            this._isPrimary = isPrimary;
            
            player.OnFire        += this.OnFire;
            player.OnHit         += this.OnHit;
            player.OnMove        += this.OnMove;
            player.OnFixedUpdate += this.OnFixedUpdate;
            player.OnTakeDamage  += this.OnTakeDamage;

            if (!isSwap && !this._isEmpty)
            {
                this._soulDurationTimer.Reset();
            }
        }

        public virtual void Unbind(IPlayer player)
        {
            player.OnFire        -= this.OnFire;
            player.OnHit         -= this.OnHit;
            player.OnMove        -= this.OnMove;
            player.OnFixedUpdate -= this.OnFixedUpdate;
            player.OnTakeDamage  -= this.OnTakeDamage;
        }

        public bool HasExpired => this._soulDurationTimer.IsTriggered();

        public Timer SoulDurationTimer => this._soulDurationTimer;

        public virtual void OnFire(IPlayer player, IProjectile projectile, Vector2 direction)
        {
        }

        public virtual void OnMove(IPlayer player, Vector2 direction)
        {
        }

        public virtual void OnFixedUpdate(IPlayer player)
        {
        }

        public virtual void OnHit(IPlayer player, IProjectile projectile, IEnemy enemy)
        {
        }

        public virtual void OnTakeDamage(IPlayer player, IDamage damage, IEnemy enemy)
        {
        }
    }
}
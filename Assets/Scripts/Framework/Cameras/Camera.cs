using UnityEngine;
using UnityEngine.U2D;
using Framework.Managers;
using UnityCamera = UnityEngine.Camera;
using Sirenix.OdinInspector;

namespace Framework.Cameras
{
    [RequireComponent(typeof(PixelPerfectCamera))]
    [RequireComponent(typeof(UnityCamera))]
    public class Camera : MonoBehaviour
    {
        [BoxGroup("Required")]
        [SerializeField, Required]
        private UnityCamera _unityCamera;

        [BoxGroup("Required")]
        [SerializeField, Required]
        private PixelPerfectCamera _pixelPerfectCamera;

        [BoxGroup("Required")]
        [SerializeField]
        private Transform _target;

        [Header("Params")]
        [Space]
        [SerializeField] protected float _timeToLerp = 0.2f;

        public UnityCamera UnityCamera => this._unityCamera;

        public PixelPerfectCamera PixelPerfectCamera => this._pixelPerfectCamera;

        public Transform Target 
        { 
            get => this._target; 
            set => this._target = value; 
        }

        private void Awake()
        {
            this._unityCamera = GetComponent<UnityCamera>();
            this._pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        }

        public void Update()
        {
            if (this._target != null)
            {
                Vector2 translation = (Vector2)(this._target.transform.position - transform.position);
                transform.Translate(translation, Space.World);
            }
        }

        public void Transfer(Camera newMain)
        {
            this._target = newMain.Target;
        }
    }
}

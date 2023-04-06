using UnityEngine;
using Sirenix.OdinInspector;
using Framework.Managers;

namespace Framework.UI
{
    [RequireComponent(typeof(Canvas))]
    public class ScreenCanvas : Canvas
    {
        [FoldoutGroup("Required Components")]
        [SerializeField, Required] 
        protected UnityEngine.Canvas _canvas;

        private CameraManager _cameraManager;

        public override void Bind()
        {
            SuperManager superManager = SuperManager.Singleton;

            this._cameraManager = superManager.Get<CameraManager>();

            this._canvas.worldCamera = this._cameraManager.Main.UnityCamera;
        }

        public override void Unbind()
        {
            this._cameraManager = null;
        }
    }
}
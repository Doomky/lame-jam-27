using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI
{
    [RequireComponent(typeof(UnityEngine.Canvas))]
    public class WorldUIEntity : Entity, IWorldUIEntity
    {
        [SerializeField, Required]
        protected UnityEngine.Canvas _canvas;

        protected virtual void Init(Transform parent)
        {
            _canvas.worldCamera = Camera.main;
        }
    }
}
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(RectTransform))]
    public abstract class Entity : SerializedMonoBehaviour, IUIEntity
    {
        [FoldoutGroup("Required Components")]
        [SerializeField, Required]
        protected RectTransform _rectTransform;

        [FoldoutGroup("Required Components")]
        [SerializeField, Required]
        protected Animator _animator;

        public RectTransform RectTransform => this._rectTransform;

        public Animator Anim => this._animator;

        protected virtual void Awake()
        {
            this._animator = GetComponent<Animator>();
            this._rectTransform = this.transform.GetComponent<RectTransform>();

            this._rectTransform.anchoredPosition3D = new Vector3(this._rectTransform.anchoredPosition.x, this._rectTransform.anchoredPosition.y, 0);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;
using Sirenix.OdinInspector;
using UnityEngine.Localization;

namespace Framework.Managers
{
    public class UIManager : Manager
    {
        [Required]
        [SerializeField]
        [ShowInInspector]
        private List<ICanvas> _canvases = new();

        [Required]
        [SerializeField]
        private Dictionary<LocalizedString, Color> _localizedStringColor = new();

        public override void Bind()
        {
            int canvasesCount = this._canvases.Count;
            for (int i = 0; i < canvasesCount; i++)
            {
                this._canvases[i].Bind();
                this._canvases[i].Load();
            }

            for (int i = 0; i < canvasesCount; i++)
            {
                this._canvases[i].UpdateVisibility();
            }
        }

        public override void Unbind()
        {
            int canvasesCount = this._canvases.Count;
            for (int i = 0; i < canvasesCount; i++)
            {
                this._canvases[i].Unload();
                this._canvases[i].Unbind();
            }
        }

        protected void Update()
        {
            int canvasesCount = this._canvases.Count;
            for (int i = 0; i < canvasesCount; i++)
            {
                this._canvases[i].UpdateVisibility();
            }
        }
    }
}
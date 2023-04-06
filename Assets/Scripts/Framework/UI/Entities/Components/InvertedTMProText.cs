using UnityEngine;
using UnityEngine.Rendering;

namespace Framework.UI.Components
{
    public class InvertedTMProText : TMPro.TextMeshProUGUI
    {
        public override Material materialForRendering
        {
            get
            {
                Material result = base.materialForRendering;
                result.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return result;
            }
        }
    }
}
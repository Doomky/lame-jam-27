using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Framework.UI.Components
{
    public class InvertedMask : Image
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
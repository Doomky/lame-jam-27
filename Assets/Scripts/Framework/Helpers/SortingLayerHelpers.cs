using System;

namespace OldGame.Layers
{
    public static partial class LayerHelpers
    {
        public static class SortingLayerHelpers
        {
            public static string GetLayer<TEnum>(TEnum layer) where TEnum : Enum
            {
                return layer.ToString();
            }
        }
    }
}
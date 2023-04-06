using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OldGame.Layers
{
    public static class PhysicalLayerHelpers
    {
        public static int GetLayer<TLayerEnum>(TLayerEnum layerType) where TLayerEnum : Enum
        {
            return LayerMask.NameToLayer(layerType.ToString());
        }

        public static TLayerEnum GetLayer<TLayerEnum>(string layerName, TLayerEnum defaultValue) where TLayerEnum : Enum
        {
            Array arr = Enum.GetValues(typeof(TLayerEnum));
            int arrLength = arr.Length;
            List<string> layerNames = new();
            for (int i = 0; i < arrLength; i++)
            {
                TLayerEnum layerTypeTested = (TLayerEnum)arr.GetValue(i);
                if (layerName.Equals(layerTypeTested.ToString()))
                {
                    return layerTypeTested;
                }
            }

            return defaultValue;
        }

        private static TLayerEnum GetLayer<TLayerEnum>(int layerIndex) where TLayerEnum : Enum
        {
            string layerName = LayerMask.LayerToName(layerIndex);
            return PhysicalLayerHelpers.GetLayer<TLayerEnum>(layerName, default(TLayerEnum));
        }

        public static bool IsLayerInMask<TLayerEnum>(int layerIndex, TLayerEnum mask) where TLayerEnum : Enum
        {
            return PhysicalLayerHelpers.IsLayerInMask(GetLayer<TLayerEnum>(layerIndex), mask);
        }

        public static bool IsLayerInMask<TLayerEnum>(string layerName, TLayerEnum mask) where TLayerEnum : Enum
        {
            TLayerEnum layer = PhysicalLayerHelpers.GetLayer<TLayerEnum>(layerName, default(TLayerEnum));
            return PhysicalLayerHelpers.IsLayerInMask(layer, mask);
        }

        public static bool IsLayerInMask<TLayerEnum>(TLayerEnum layerType, TLayerEnum mask) where TLayerEnum : Enum
        {
             long layerValue = (int)Convert.ChangeType(layerType, typeof(int));
             long maskValue = (int)Convert.ChangeType(layerType, typeof(int));

            return (layerValue & maskValue) != 0;
        }

        public static int GetLayerMaskFromFlags<TLayerEnum>(TLayerEnum layerType) where TLayerEnum : Enum
        {
            Array arr = Enum.GetValues(typeof(TLayerEnum));
            int arrLength = arr.Length;
            List<string> layerNames = new();
            for (int i = 0; i < arrLength; i++)
            {
                TLayerEnum layerTypeTested = (TLayerEnum)arr.GetValue(i);
                if (PhysicalLayerHelpers.IsLayerInMask(layerType, layerTypeTested))
                {
                    layerNames.Add(layerTypeTested.ToString());
                }
            }

            return LayerMask.GetMask(layerNames.ToArray());
        }

        public static int GetLayerMask<TLayerEnum>(TLayerEnum layerType) where TLayerEnum : Enum
        {
            return LayerMask.GetMask(layerType.ToString());
        }

        public static int GetLayerMask<TLayerEnum>(params TLayerEnum[] layerTypes) where TLayerEnum : Enum
        {
            string[] layerNames = layerTypes.Select(layerType => layerType.ToString()).ToArray();
            return LayerMask.GetMask(layerNames);
        }
    }
}

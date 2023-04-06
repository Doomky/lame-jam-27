using UnityEngine;

namespace Framework.Helpers
{
    public static class MathHelpers
    {
        public static float RectPerlinNoise(in float x, in float y, in float scale = 1, in float unscaledX = 0, in float unscaledY = 0)
        {
            float xPos = scale * x + unscaledX;
            float yPos = scale * y + unscaledY;

            return Mathf.Clamp01(Mathf.PerlinNoise(xPos, yPos));
        }
    }
}
using UnityEngine;

namespace Framework.Extensions
{
    public static class Vector2Extensions
    {
        public enum Format
        {
            Floor,
            Rounded,
            Ceil,
        }

        public static Quaternion FromToRotation(this Vector2 dir)
        {
            return Quaternion.FromToRotation(Vector2.right, dir);
        }

        public static Quaternion FromToRotation(this Vector2 dir, Vector2 fromVect)
        {
            float angle = Vector2.Angle(fromVect, dir);

            if (angle % 180 < 1f && (fromVect.x * dir.x < 0 || fromVect.y * dir.y < 0))
            {
                return Quaternion.Euler(0, 0, 180);
            }

            return Quaternion.FromToRotation(fromVect, dir);
        }

        public static Vector2Int ToVectorInt(this Vector2 vector2, Format format = Format.Floor)
        {
            switch (format)
            {
                case Format.Floor:
                    {
                        return new Vector2Int(Mathf.FloorToInt(vector2.x), Mathf.FloorToInt(vector2.y));
                    }

                case Format.Ceil:
                    {
                        return new Vector2Int(Mathf.CeilToInt(vector2.x), Mathf.CeilToInt(vector2.y));
                    }

                default:
                case Format.Rounded:
                    {
                        return new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
                    }
            }
        }
    }
}

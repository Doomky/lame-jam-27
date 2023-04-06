using UnityEngine;
using static Direction;

namespace Framework.Extensions
{
    public static class Vector2IntExtensions 
    {
        public enum Direction
        {
            Unknown,
            Horizontal,
            Vertical
        }

        public static bool IsAlignedOnAxis(this Vector2Int v, Vector2Int w)
        {
            return v.x == w.x || v.y == w.y;
        }

        public static Direction GetDirection(this Vector2Int v)
        {
            if (v.x * v.y != 0)
            {
                return Direction.Unknown;
            }

            if (v.x == 0)
            {
                return Direction.Horizontal;
            }
            else
            {
                return Direction.Vertical;
            }
        }

        public static Direction4 ToDirection4(this Vector2Int v)
        {
            if (v.x * v.y != 0 || (v.x == 0 && v.y == 0))
            {
                return Direction4.None;
            }

            if (v.x == 0)
            {
                return v.y > 0 ? Direction4.Up : Direction4.Down;
            }
            else
            {
                return v.x > 0 ? Direction4.Right : Direction4.Left;
            }
        }

        public static Vector2 ToVector2(this Vector2Int v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3Int ToVector3Int(this Vector2Int v)
        {
            return new Vector3Int(v.x, v.y, 0);
        }
    }
}

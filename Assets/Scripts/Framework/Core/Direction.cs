using Framework.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Direction
{
    public enum Direction4
    {
        None = 0,
        Right = 1,
        Down = 2,
        Left = 4,
        Up = 8,
    }

    public enum Direction8
    {
        None = 0,
        Right = 1,
        DownRight = 3,
        Down = 2,
        DownLeft = 6,
        Left = 4,
        UpLeft = 12,
        Up = 8,
        UpRight = 9,
    }

    private static readonly Direction4[] _direction4NeighborArray = new Direction4[]
    {
        Direction4.Right,
        Direction4.Down,
        Direction4.Left,
        Direction4.Up
    };

    private static readonly Direction8[] _direction8NeighborArray = new Direction8[]
    {
        Direction8.Right,
        Direction8.DownRight,
        Direction8.Down,
        Direction8.DownLeft,
        Direction8.Left,
        Direction8.UpLeft,
        Direction8.Up,
        Direction8.UpRight,
    };

    public static Direction4[] AsArray()
    {
        return new Direction4[] { Direction4.Right, Direction4.Down, Direction4.Left, Direction4.Up };
    }

    public static Vector2 ToVector2(this Direction8 dir8)
    {
        return ToVector2(dir8, 0);
    }

    public static Vector2Int ToVector2Int(this Direction4 dir4)
    {
        switch (dir4)
        {
            case Direction4.None:
                return Vector2Int.zero;
            case Direction4.Right:
                return Vector2Int.right;
            case Direction4.Down:
                return Vector2Int.down;
            case Direction4.Left:
                return Vector2Int.left;
            case Direction4.Up:
                return Vector2Int.up;
            default:
                throw new NotSupportedException(dir4 + " is not supported");
        }
    }

    public static Vector2Int ToVector2Int(this Direction8 dir8)
    {
        switch (dir8)
        {
            case Direction8.None:
                return Vector2Int.zero;
            case Direction8.Right:
                return Vector2Int.right;
            case Direction8.DownRight:
                return new Vector2Int(1, -1);
            case Direction8.Down:
                return Vector2Int.down;
            case Direction8.DownLeft:
                return -Vector2Int.one;
            case Direction8.Left:
                return Vector2Int.left;
            case Direction8.UpLeft:
                return new Vector2Int(-1, 1);
            case Direction8.Up:
                return Vector2Int.up;
            case Direction8.UpRight:
                return Vector2Int.one;
            default:
                throw new NotSupportedException(dir8 + " is not supported");
        }
    }

    public static Vector2 ToVector2(Direction8 dir8, int _ = 0)
    {
        switch (dir8)
        {
            case Direction8.None:
                return Vector2.zero;
            case Direction8.Right:
                return Vector2.right;
            case Direction8.DownRight:
                return new Vector2(1, -1).normalized;
            case Direction8.Down:
                return Vector2.down;
            case Direction8.DownLeft:
                return new Vector2(-1, -1).normalized;
            case Direction8.Left:
                return Vector2.left;
            case Direction8.UpLeft:
                return new Vector2(-1, 1).normalized;
            case Direction8.Up:
                return Vector2.up;
            case Direction8.UpRight:
                return new Vector2(1, 1).normalized;
            default:
                throw new NotSupportedException(dir8 + " is not supported");
        }
    }

    public static Direction8 FromVector2(Vector2 vector2)
    {
        int directionValue = 0;
        float delta = 0.2f;
        if (Math.Abs(vector2.y) > delta)
        {
            directionValue += (int)(vector2.y > 0 ? Direction8.Up : Direction8.Down);
        }

        if (Math.Abs(vector2.x) > delta)
        {
            directionValue += (int)(vector2.x > 0 ? Direction8.Right : Direction8.Left);
        }

        return (Direction8)directionValue;
    }

    public static Vector2 ToVector2(this Direction4 dir4)
    {
        return ToVector2(dir4, 0);
    }

    public static Vector2 ToVector2(Direction4 dir4, int _ = 0)
    {
        switch (dir4)
        {
            case Direction4.None:
                return Vector2.zero;
            case Direction4.Right:
                return Vector2.right;
            case Direction4.Down:
                return Vector2.down;
            case Direction4.Left:
                return Vector2.left;
            case Direction4.Up:
                return Vector2.up;
            default:
                throw new NotSupportedException(dir4 + " is not supported");
        }
    }

    public static Quaternion FromToRotation(Direction4 dir)
    {
        return FromToRotation((Direction8)dir, Vector2.right);
    }

    public static Quaternion FromToRotation(Direction4 dir, Vector2 fromVect)
    {
        return FromToRotation((Direction8)dir, fromVect);
    }

    public static Quaternion FromToRotation(Direction8 dir)
    {
        return FromToRotation(dir, Vector2.right);
    }

    public static Quaternion FromToRotation(Direction8 dir, Vector2 fromVect)
    {
        return dir.ToVector2().FromToRotation(fromVect);
    }

    public static IEnumerable<Vector2Int> _4DirNeighbors(Vector2Int position)
    {
        int count = _direction4NeighborArray?.Length ?? 0;
        for (int i = 0; i < count; i++)
        {
            yield return position + _direction4NeighborArray[i].ToVector2Int();
        }
    }

    public static IEnumerable<Vector2Int> _8DirNeighbors(Vector2Int position)
    {
        int count = _direction8NeighborArray?.Length ?? 0;
        for (int i = 0; i < count; i++)
        {
            yield return position + _direction8NeighborArray[i].ToVector2Int();
        }
    }

    public static void ForEach4DirNeighbors(Vector2Int position, Action<Vector2Int> action)
    {
        int count = _direction4NeighborArray?.Length ?? 0;
        for (int i = 0; i < count; i++)
        {
            action.Invoke(position + _direction4NeighborArray[i].ToVector2Int());
        }
    }

    public static void ForEach8DirNeighbors(Vector2Int position, Action<Vector2Int> action)
    {
        int count = _direction8NeighborArray?.Length ?? 0;
        for (int i = 0; i < count; i++)
        {
            action.Invoke(position + _direction8NeighborArray[i].ToVector2Int());
        }
    }
}

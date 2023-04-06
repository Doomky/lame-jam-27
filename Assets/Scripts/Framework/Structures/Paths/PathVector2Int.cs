using Framework.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Direction;

namespace Framework
{
    [System.Serializable]
    public class PathVector2Int : Path<Vector2Int>
    {
        public PathVector2Int() : base()
        {
        }

        public PathVector2Int(List<Vector2Int> positions) : base(positions)
        {
        }

        public PathVector2Int(PathVector2Int pathVector) : base(pathVector.KeyPositions)
        {

        }

        public override IEnumerable<Vector2Int> AllPositions()
        {
            Vector2Int currentPosition = _keyPositions[0];

            yield return currentPosition;

            IEnumerable<Vector2Int> segments = this.GetSegments();
            foreach (Vector2Int segment in segments)
            {
                Direction4 direction = segment.ToDirection4();

                Vector2Int step = direction.ToVector2Int();

                int segmentLength = segment.x + segment.y;
                for (int j = 0; j < segmentLength; ++j)
                {
                    currentPosition += step;
                    yield return currentPosition;
                }
            }
        }

        public override Vector2 GetKeyPositionsAsVector2(int i)
        {
            return _keyPositions[i];
        }

        public static implicit operator PathVector2(PathVector2Int pathVector)
        {
            return new PathVector2(pathVector.KeyPositions.Select(v => (Vector2)v).ToList());
        }

        public void Split()
        {
            List<Vector2Int> newKeyPositions = new();

            int positionsCount = _keyPositions.Count;

            newKeyPositions.Add(_keyPositions[0]);

            if (positionsCount > 2)
            {
                Vector2Int prePosition = _keyPositions[0];
                Vector2Int position = _keyPositions[1];
                Vector2Int postPosition = _keyPositions[2];

                for (int i = 1; i < positionsCount - 1; i++)
                {

                    bool hasANewPrePosition = Vector2Int.Distance(position, prePosition) > 8;
                    Vector2Int newPrePosition = prePosition;
                    if (hasANewPrePosition)
                    {
                        newPrePosition = prePosition + (UnityEngine.Random.Range(0.2f, 0.8f) * (Vector2)(position - prePosition)).ToVectorInt(Vector2Extensions.Format.Rounded);
                    }

                    bool hasANewPostPosition = Vector2Int.Distance(position, postPosition) > 6;
                    Vector2Int newPostPosition = postPosition;
                    if (hasANewPostPosition)
                    {
                        newPostPosition = position + (UnityEngine.Random.Range(0.2f, 0.8f) * (Vector2)(postPosition - position)).ToVectorInt(Vector2Extensions.Format.Rounded);
                    }

                    if (hasANewPrePosition && hasANewPostPosition)
                    {
                        position = prePosition.x != position.x ? new Vector2Int(newPrePosition.x, newPostPosition.y) : new Vector2Int(newPostPosition.x, newPrePosition.y);
                    }
                    else if (hasANewPostPosition)
                    {
                        position = prePosition.x != position.x ? new Vector2Int(prePosition.x, newPostPosition.y) : new Vector2Int(newPostPosition.x, prePosition.y);
                    }

                    if (hasANewPrePosition)
                    {
                        newKeyPositions.Add(newPrePosition);
                    }

                    newKeyPositions.Add(position);

                    if (hasANewPostPosition)
                    {
                        newKeyPositions.Add(newPostPosition);
                    }

                    prePosition = hasANewPostPosition ? newPostPosition : position;
                    position = postPosition;
                    if (i < positionsCount - 2)
                    {
                        postPosition = _keyPositions[i + 2];
                    }
                }
            }

            newKeyPositions.Add(_keyPositions[positionsCount - 1]);
            _keyPositions = newKeyPositions;
        }

        public override Vector2Int GetSegment(int index)
        {
            return this._keyPositions[index + 1] - this._keyPositions[index];
        }
    }
}
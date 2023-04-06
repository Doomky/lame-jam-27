using Framework.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Direction;

namespace Framework
{
    [System.Serializable]
    public class PathVector2 : Path<Vector2>
    {
        public PathVector2()
        {
        }

        public PathVector2(List<Vector2> positions) : base(positions)
        {
        }

        public override IEnumerable<Vector2> AllPositions()
        {
            Vector2 currentPosition = _keyPositions[0];

            yield return currentPosition;

            IEnumerable<Vector2> segments = this.GetSegments();
            foreach (Vector2 segment in segments)
            {
                Direction4 direction = segment.ToVectorInt().ToDirection4();

                Vector2 step = direction.ToVector2();

                int segmentLength = Mathf.RoundToInt(segment.x) + Mathf.RoundToInt(segment.y);
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

        public override Vector2 GetSegment(int index)
        {
            return this._keyPositions[index + 1] - this._keyPositions[index];
        }

        public static implicit operator PathVector2Int(PathVector2 pathVector)
        {
            return new PathVector2Int(pathVector.KeyPositions.Select(v => v.ToVectorInt()).ToList());
        }
    }
}
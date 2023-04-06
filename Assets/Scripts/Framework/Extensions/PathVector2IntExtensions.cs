using System.Collections.Generic;
using UnityEngine;

namespace Framework.Extensions
{
    public static class PathVector2IntExtensions
    {
        public static PathVector2Int XSymetry(this PathVector2Int path, int centerX)
        {
            PathVector2Int resultPath = new();

            List<Vector2Int> pathKeyPositions = path.KeyPositions;

            int pathKeyPositionsCount = pathKeyPositions?.Count ?? 0;
            for (int i = 0; i < pathKeyPositionsCount; ++i)
            {
                int x = pathKeyPositions[i].x;
                resultPath.KeyPositions.Add(new Vector2Int(centerX - (x - centerX), pathKeyPositions[i].y));
            }

            return resultPath;
        }

        public static PathVector2Int YSymetry(this PathVector2Int path, int centerY)
        {
            PathVector2Int resultPath = new();

            List<Vector2Int> pathKeyPositions = path.KeyPositions;

            int pathKeyPositionsCount = pathKeyPositions?.Count ?? 0;
            for (int i = 0; i < pathKeyPositionsCount; ++i)
            {
                int y = pathKeyPositions[i].y;
                resultPath.KeyPositions.Add(new Vector2Int(pathKeyPositions[i].x, centerY - (y - centerY)));
            }

            return resultPath;
        }
    }
}

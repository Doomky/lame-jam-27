using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework
{
    [System.Serializable]
    public abstract class Path<TNumeric>
    {
        [SerializeField] protected List<TNumeric> _keyPositions = new();

        public List<TNumeric> KeyPositions => _keyPositions;

        public abstract IEnumerable<TNumeric> AllPositions();

        public abstract Vector2 GetKeyPositionsAsVector2(int index);

        public abstract TNumeric GetSegment(int index);

        public Path()
        {

        }

        public Path(List<TNumeric> positions)
        {
            _keyPositions = new List<TNumeric>(positions);
        }

        public IEnumerable<TNumeric> GetSegments()
        {
            int segmentsCount = _keyPositions.Count - 1;
            for (int i = 0; i < segmentsCount; i++)
            {
                yield return this.GetSegment(i);
            }
        }
    }
}
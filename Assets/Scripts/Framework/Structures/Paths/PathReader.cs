using UnityEngine;

namespace Framework
{
    public class PathReader
    {
        public enum LoopModeEnum
        {
            None,
            Loop,
            PingPong
        }
    }

    public class PathReader<TNumeric> : PathReader
    {

        Path<TNumeric> _path;

        [SerializeField] protected int _index;

        [SerializeField] protected LoopModeEnum _loopMode;

        protected int _increment = 1;

        public Path<TNumeric> Path => _path;

        public LoopModeEnum LoopMode
        {
            get => _loopMode;
            set => _loopMode = value;
        }

        public int Index => _index;

        public PathReader(Path<TNumeric> path)
        {
            _path = path;
        }

        public bool IsOnEdge()
        {
            return _index == 0 || _index == _path.KeyPositions.Count - 1;
        }

        public TNumeric GetCurrentPosition()
        {
            return _path.KeyPositions[_index];
        }

        public void MoveIndex()
        {
            int lastIndex = _path.KeyPositions.Count - 1;

            switch (_loopMode)
            {
                case LoopModeEnum.None:
                    {
                        if (_index < lastIndex)
                        {
                            _index += _increment;
                        }

                        break;
                    }

                case LoopModeEnum.Loop:
                    {
                        if (_index < lastIndex)
                        {
                            _index += _increment;
                        }
                        else
                        {
                            _index = 0;
                        }

                        break;
                    }

                case LoopModeEnum.PingPong:
                    {
                        if (_index == lastIndex || _index == 0)
                        {
                            _increment *= -1;
                            _index += _increment;
                        }
                        else
                        {
                            _index += _increment;
                        }

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
}
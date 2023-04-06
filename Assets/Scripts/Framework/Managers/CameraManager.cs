using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.Managers
{
    public class CameraManager : Manager
    {
        [SerializeField]
        private List<Cameras.Camera> _cameras = new();

        [SerializeField] 
        private Cameras.Camera _main;

        public List<Cameras.Camera> Cameras => _cameras;

        public Cameras.Camera Main => _main;

        public void Add(Cameras.Camera camera)
        {
            if (!_cameras.Contains(camera))
            {
                _cameras.Add(camera);
            }
        }

        public void Remove(Cameras.Camera camera)
        {
            if (_cameras.Remove(camera))
            {
                if (_main == camera)
                {
                    Cameras.Camera newMain = _cameras.Any() ? _cameras[0] : null;
                    
                    if (newMain)
                    {
                        _main.Transfer(newMain);
                    }

                    _main = newMain;
                }
            }
        }

        public override void Bind()
        {
        }

        public override void Unbind()
        {
        }
    }
}
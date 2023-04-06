using Framework.Helpers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Managers
{
    public class SuperManager : SerializedMonoBehaviour
    {
        [SerializeField]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
        private Dictionary<Type, IManager> _managerPrefabByType = new();

        [ShowInInspector]
        [HideInEditorMode]
        private Dictionary<Type, IManager> _managersByType = new();

        private static SuperManager _singleton = null;

        public static SuperManager Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = FindObjectOfType<SuperManager>();
                }

                return _singleton;
            }
        }

        public TManager Add<TManager>() where TManager : Manager
        {
            Type serviceType = typeof(TManager);

            if (this._managersByType.ContainsKey(serviceType))
            {
                Debug.LogError($"Service: Could not Add {typeof(TManager)}, the service is already existing");
                return (TManager)this._managersByType[serviceType];
            }

            TManager manager = PrefabHelpers.Instantiate((TManager)this._managerPrefabByType[serviceType], this.transform);
            this._managersByType.Add(typeof(TManager), manager);

            manager.Bind();

            return manager;
        }

        public void Remove<TManager>() where TManager : Manager
        {
            if (!this._managersByType.TryGetValue(typeof(TManager), out IManager genericService))
            {
                Debug.LogError($"Service: Could not Remove {typeof(TManager)}");
                return;
            }

            TManager service = (TManager)genericService;
            genericService.Unbind();

            this._managersByType.Remove(typeof(TManager));
            GameObject.Destroy(service.gameObject);
        }

        public TManager Get<TManager>() where TManager : Manager
        {
            return (TManager)this._managersByType[typeof(TManager)];
        }

        public bool TryGet<TManager>(out TManager service) where TManager : Manager
        {
            service = null;

            if (!this._managersByType.TryGetValue(typeof(TManager), out IManager genericService))
            {
                return false;
            }

            service = (TManager)genericService;
            return true;
        }
    }
}

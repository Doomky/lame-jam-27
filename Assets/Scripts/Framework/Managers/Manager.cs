
using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Framework.Helpers;

namespace Framework.Managers
{
    public abstract class Manager : SerializedMonoBehaviour, IManager
    {
        public const string DefaultPrefabsDirectoryRelativePath = "Prefabs/Managers/";

        public abstract void Bind();
        
        public abstract void Unbind();

        public Manager()
        {

        }

        public static TManager Add<TManager>() where TManager : Manager
        {
            return SuperManager.Singleton.Add<TManager>();
        }

        public static void Remove<TManager>() where TManager : Manager
        {
            SuperManager.Singleton.Remove<TManager>();
        }

        public static TManager Get<TManager>() where TManager : Manager
        {
            return SuperManager.Singleton.Get<TManager>();
        }

        public static bool TryGet<TManager>(out TManager service) where TManager : Manager
        {
            return SuperManager.Singleton.TryGet(out service);
        }
    }
}

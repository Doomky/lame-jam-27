using System;
using UnityEngine;

namespace Framework.Helpers
{
    public static class PrefabHelpers
    {
        public static T InstantiateUI<T>(T prefabComponent, Transform parent = null) where T : Component
        {
            return InstantiateUI(prefabComponent, position: null, parent);
        }

        public static T InstantiateUI<T>(T prefabComponent, Vector2? position, Transform parent = null) where T : Component
        {
            GameObject gameObject = GameObject.Instantiate(prefabComponent.gameObject, parent);
            
            if (position.HasValue)
            {
                gameObject.GetComponent<RectTransform>().anchoredPosition = position.Value;
            }

            return gameObject.GetComponent<T>();
        }

        public static T Instantiate<T>(T prefabComponent, Transform parent = null) where T : Component
        {
            return Instantiate(prefabComponent, Vector2.zero, parent);
        }

        public static T Instantiate<T>(T prefabComponent, string name, Transform parent = null) where T : Component
        {
            return Instantiate(prefabComponent, name, Vector2.zero, parent);
        }

        public static T Instantiate<T>(T prefabComponent, Vector2 position, Transform parent = null) where T : Component
        {
            GameObject gameObject = GameObject.Instantiate(prefabComponent.gameObject, parent);
            gameObject.transform.position = position;
            return gameObject.GetComponent<T>();
        }

        public static T Instantiate<T>(T prefabComponent, string name, Vector2 position, Transform parent = null) where T : Component
        {
            GameObject gameObject = GameObject.Instantiate(prefabComponent.gameObject, parent);
            gameObject.name = name;
            gameObject.transform.position = position;
            return gameObject.GetComponent<T>();
        }

        public static T Instantiate<T>(GameObject prefab, Vector2 position, Quaternion rotation, Transform parent = null) where T : Component
        {
            GameObject gameObject = GameObject.Instantiate(prefab.gameObject, position, rotation, parent);
            gameObject.transform.position = position;
            return gameObject.GetComponent<T>();
        }

        public static GameObject InstantiateGo(GameObject prefab, Transform parent = null)
        {
            return InstantiateGo(prefab, Vector2.zero, parent);
        }

        public static GameObject InstantiateGo(GameObject prefab, Vector2 position, Transform parent = null)
        {
            GameObject gameObject = GameObject.Instantiate(prefab.gameObject, parent);
            gameObject.transform.position = position;
            return gameObject;
        }

        public static GameObject InstantiateGo(GameObject prefab, Vector2 position = default, Quaternion rotation = default, Transform parent = null)
        {
            GameObject gameObject = GameObject.Instantiate(prefab.gameObject, position, rotation, parent);
            return gameObject;
        }

        internal static void Instantiate<T>(object cycleEndPS)
        {
            throw new NotImplementedException();
        }
    }
}
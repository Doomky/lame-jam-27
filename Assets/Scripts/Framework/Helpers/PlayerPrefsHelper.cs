using System;

namespace Framework.Helpers
{
    public static class PlayerPrefsHelper
    {
        public static void SaveEnum<TEnum>(in string key, in TEnum value)
        {
            Save(in key, in value, (str, value) => UnityEngine.PlayerPrefs.SetInt(str, (int)(object)value));
        }

        public static void SaveInt(in string key, in int value)
        {
            Save(in key, in value, UnityEngine.PlayerPrefs.SetInt);
        }

        public static void SaveFloat(in string key, in float value)
        {
            Save(in key, in value, UnityEngine.PlayerPrefs.SetFloat);
        }

        public static void SaveString(in string key, in string value)
        {
            Save(in key, in value, UnityEngine.PlayerPrefs.SetString);
        }

        internal static void SaveBool(string path, bool state)
        {
            Save<int, bool>(in path, in state, (str, state) => UnityEngine.PlayerPrefs.SetInt(str, state ? 1 : 0));
        }

        public static bool TryGetEnum<TEnum>(in string key, out TEnum value, TEnum defaultValue) where TEnum : Enum
        {
            return TryGet(key, out value, in defaultValue, (str) => (TEnum)(object)UnityEngine.PlayerPrefs.GetInt(str));
        }

        public static bool TryGetInt(in string key, out int value, int defaultValue = int.MinValue)
        {
            return TryGet(key, out value, in defaultValue, UnityEngine.PlayerPrefs.GetInt);
        }

        public static bool TryGetFloat(in string key, out float value, float defaultValue = float.NaN)
        {
            return TryGet(key, out value, in defaultValue, UnityEngine.PlayerPrefs.GetFloat);
        }

        public static bool TryGetString(in string key, out string value, string defaultValue = null)
        {
            return TryGet(key, out value, in defaultValue, UnityEngine.PlayerPrefs.GetString);
        }

        internal static bool TryGetBool(string key, out bool value, bool defaultValue = false)
        {
            return TryGet(key, out value, defaultValue, (str) => UnityEngine.PlayerPrefs.GetInt(str) == 1);
        }

        private static void Save<T, T2>(in string key, in T2 value, Action<string, T2> indirectSetter)
        {
            indirectSetter(key, value);
            UnityEngine.PlayerPrefs.Save();
        }

        private static void Save<T>(in string key, in T value, Action<string, T> setter)
        {
            setter(key, value);
            UnityEngine.PlayerPrefs.Save();
        }

        private static bool TryGet<T>(in string key, out T value, in T defaultValue, Func<string, T> getter)
        {
            bool hasKey = UnityEngine.PlayerPrefs.HasKey(key);

            value = defaultValue;

            if (hasKey)
            {
                value = getter(key);
            }

            return hasKey;
        }
    }
}
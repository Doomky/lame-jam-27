using Framework.Helpers;
using System;
using UnityEngine;

namespace Framework.Managers
{
    public class SaveManager : Manager
    {
        public void SaveEnum<TEnum>(string path, TEnum enumValue) where TEnum : Enum
        {
            PlayerPrefsHelper.SaveEnum(path, enumValue);
        }

        public void Save<TClass>(string path, TClass obj) where TClass : class
        {
            PlayerPrefsHelper.SaveString(path, JsonUtility.ToJson(obj));
        }

        public void SaveInt(string path, int value)
        {
            PlayerPrefsHelper.SaveInt(path, value);
        }

        public void SaveBool(string path, bool state)
        {
            PlayerPrefsHelper.SaveBool(path, state);
        }

        public int GetInt(string path)
        {
            PlayerPrefsHelper.TryGetInt(path, out int value, 0);

            return value;
        }

        public TClass Get<TClass>(string path) 
        {
            string value = string.Empty;
            PlayerPrefsHelper.TryGetString(path, out value, string.Empty);

            return JsonUtility.FromJson<TClass>(value);
        }

        public TEnum GetEnum<TEnum>(string path) where TEnum : Enum
        {
            PlayerPrefsHelper.TryGetEnum(in path, out TEnum enumValue, (TEnum)(object)0);

            return enumValue;
        }

        public bool TryGetEnum<TEnum>(string path, out TEnum state) where TEnum : Enum
        {
            state = GetEnum<TEnum>(path);
            return (int)(object)state == 0;
        }

        public bool TryGet<TClass>(string path, out TClass res)
        {
            res = Get<TClass>(path);
            return res != null;
        }

        public bool TryGetBool(string path, out bool res)
        {
            return PlayerPrefsHelper.TryGetBool(path, out res, false);
        }

        public void SaveSingleton<TClass>(TClass obj) where TClass : class
        {
            Type type = typeof(TClass); 
            PlayerPrefsHelper.SaveString(type.Name, JsonUtility.ToJson(obj));
        }

        public TClass GetSingleton<TClass>() where TClass : class
        {
            Type type = typeof(TClass);
            string value = string.Empty;
 
            if (!PlayerPrefsHelper.TryGetString(type.FullName, out value, defaultValue: null))
            {
                return null;
            }
 
             return JsonUtility.FromJson<TClass>(value);
        }

        public bool TryGetSingleton<TClass>(out TClass res) where TClass : class
        {
            res = GetSingleton<TClass>();
            return res != null;
        }

        public override void Bind()
        {

        }

        public override void Unbind()
        {

        }
    }
}
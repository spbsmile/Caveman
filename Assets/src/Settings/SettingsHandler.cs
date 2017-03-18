using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Caveman.DevSetting
{
    public class SettingsHandler 
    {
        private const string ResourcesName = "JSON";

        private static readonly TextAsset[] settings;

        static SettingsHandler()
        {
            settings = Resources.LoadAll<TextAsset>(ResourcesName);
        }

        public static T ParseSettingsFromFile<T>(string fileName)
        {
            var settingsFromFile = default(T);
            if (settings == null)
                return settingsFromFile;

            var textAsset = settings.FirstOrDefault(t => t.name == fileName);
            if (textAsset == default(TextAsset))
                return settingsFromFile;

            try
            {
                settingsFromFile = ParseSettings<T>(textAsset.text);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return settingsFromFile;
        }

        private static T ParseSettings<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            return default(T);
        }
    }
}
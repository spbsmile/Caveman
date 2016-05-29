using MiniJSON;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Caveman.Setting
{
    public class SettingsHandler 
    {
        private const string RESOURCES_NAME = "Settings";

        private static TextAsset[] settings;

        static SettingsHandler()
        {
            settings = Resources.LoadAll<TextAsset>(RESOURCES_NAME);
        }

        public static string SerializeSettings<T>(T data)
        {
            try
            {
                return Json.Serialize(data);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void SaveSettings<T>(T data, string file)
        {
            var s = SerializeSettings(data);
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(file);
                sw.WriteLine(s);
            }
            finally
            {
                if (sw != null) sw.Close();
            }
        }

        public static T ParseSettings<T>(string data)
        {
            try
            {
                return Json.Deserialize<T>(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            return default(T);
        }

        //deserilizations
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
                var i = e.Message;
                Debug.Log(i);
            }

            return settingsFromFile;
        }
    }
}
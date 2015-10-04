using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Caveman.Setting
{
    public class SettingsHandler 
    {
        public static string SerializeSettings<T>(T data)
        {
            try
            {
                return JsonConvert.SerializeObject(data, Formatting.Indented);
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
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            return default(T);
        }

        public static T ParseSettingsFromFile<T>(string file)
        {
            try
            {
                var f = File.ReadAllText(file);
                var settingsFromFile = ParseSettings<T>(f);
                return settingsFromFile;
            }
            catch (Exception e)
            {
                var i = e.Message;
            }
            return default(T);
        }
    }
}
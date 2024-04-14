using System;
using System.IO;
using System.Security.Cryptography;
using CG.Starter;
using UnityEngine;

namespace CG.QuickSave
{
    public class QuickSaver
    {
        public static void SaveData<T>(string filePath, T data)
        {
            string strJson = JsonUtility.ToJson(data);
            
            File.WriteAllText(filePath, strJson);
        }
        public static T LoadData<T>(string filePath)
        {
            string strJson = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(strJson);
        }
        
        public static string GetAppDataPath(string key)
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CGConstants.appName);
            Directory.CreateDirectory(appDataPath);
            return Path.Combine(appDataPath, key);
        }
        
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CG.Time
{
    public static class QuickTime
    {
        private const string worldTimeAPIUrl = "https://worldtimeapi.org/api/ip";
        
        public static IEnumerator GetCurrentTime(Action<DateTime> onComplete)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(worldTimeAPIUrl))
            {
                // Wait for the response
                yield return request.SendWebRequest();

                // Check for errors
                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + request.error);
                    onComplete?.Invoke(DateTime.Now);
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;
                    WorldTimeData worldTimeData = JsonUtility.FromJson<WorldTimeData>(jsonResponse);

                    // Invoke the callback
                    onComplete?.Invoke(ConvertToDateTime(worldTimeData.datetime));
                }
            }
        }
        public static DateTime ConvertToDateTime(string datetime)
        {
            if (DateTime.TryParse(datetime, out DateTime result))
            {
                return result;
            }
            else
            {
                Debug.LogError("datetime received from api could not be converted, local time is returning... " + datetime);
                return DateTime.Now;
            }
        }
    }
    
    [Serializable]
    public class WorldTimeData
    {
        public string datetime;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using CG.Audio;
using UnityEngine;
using UnityEngine.Networking;

namespace CG.VersionControl
{
    public class QuickVersion : MonoBehaviour
    {
        #region Singleton

        public static QuickVersion instance;

        private void Awake()
        {
            if (QuickVersion.instance != null)
            {
                return;
            }

            instance = this;
        }

        #endregion

        public VersionConfig Config;

        #region Events

        public event Action OnNewVersionAvailable;
        public event Action OnFailed;

        #endregion
        

        private void Start()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            StartCoroutine(SendPostRequest());
            
            
        }

        

        #region Post

        IEnumerator SendPostRequest()
        {
            
            WWWForm form = new WWWForm();
            form.AddField("type", "get");
            form.AddField("key", Config.APIKey);

            
            using (UnityWebRequest www = UnityWebRequest.Post(Config.APIUrl, form))
            {
                
                yield return www.SendWebRequest();

                
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Post request failed: " + www.error);
                }
                else
                {
                    string response = www.downloadHandler.text;
                    if (response != string.Empty && response != "fail")
                    {
                        Debug.Log(www.url+response);
                        var version = new Version(response);
                        
                        
                        if (version.CompareTo(Config.CurrentVersion) != 0)
                        {
                            OnNewVersionAvailable?.Invoke();
                        }
                    }
                    else
                    {
                        OnFailed?.Invoke();
                    }
                }
            }
        }

        #endregion
    }

}

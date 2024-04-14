using System;
using UnityEngine;

namespace CG.Starter
{
    public class CGManager : MonoBehaviour
    {
        #region Singleton

        public static CGManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        #endregion
        #region Configure
        [HideInInspector]
        public bool Popup;
        [HideInInspector] 
        public bool PopupConfigured;
        [HideInInspector]
        public bool AudioSystem;
        [HideInInspector] 
        public bool AudioConfigured;
        [HideInInspector]
        public bool VersionSystem;
        [HideInInspector] 
        public bool VersionConfigured;
        #endregion
    }
}
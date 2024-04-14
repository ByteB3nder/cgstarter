using System;
using UnityEngine;

namespace CG.Starter
{
    public class CGMain : MonoBehaviour
    {
        public static CGMain instance;
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
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


namespace CG.Alert
{
    public class AlertPopup : MonoBehaviour
    {
        public TMP_Text headerText;
        public TMP_Text contentText;
        public Transform ButtonParent;
    
        public void Initialize(AlertSettings settings)
        {
            if (AlertSystem.instance == null)
            {
                Debug.LogError("To use the popup system, please add AlertSystem to your scene!");
                return;
            }
        
            headerText.text = settings.Header;
            contentText.text = settings.Content;
            if (settings.AlertButtons != null)
            {
                foreach (var buttonEvent in settings.AlertButtons)
                {
                    Instantiate(AlertSystem.instance.GetButton(settings.Type),ButtonParent).GetComponent<AlertButtonListener>().Initialize(buttonEvent);
                }
            }
        
        }

        public void Close()
        {
            Destroy(this.gameObject);
        }
    }

  
}

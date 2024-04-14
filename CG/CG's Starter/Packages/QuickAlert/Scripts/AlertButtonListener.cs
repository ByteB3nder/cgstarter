using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CG.Alert
{
    public class AlertButtonListener : MonoBehaviour
    {
        private TMP_Text buttonText;
        private Button localButton;
        private AlertButton button;
        private void Awake()
        {
            buttonText = GetComponentInChildren<TMP_Text>();
            localButton = GetComponentInChildren<Button>();
            localButton.onClick.AddListener(ButtonAction);
            localButton.onClick.AddListener(GetComponentInParent<AlertPopup>().Close);
        }

        private void ButtonAction()
        {
            if (button.buttonEvent != null)
            {
                button.buttonEvent.Invoke();
            }
        }

        public void Initialize(AlertButton _button)
        {
            button = _button;
            buttonText.text = _button.buttonContent;
        }
    }

}

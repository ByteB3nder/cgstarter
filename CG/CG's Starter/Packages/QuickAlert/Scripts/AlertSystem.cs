using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CG.Alert
{
    public class AlertSystem : MonoBehaviour
{
    #region Singleton

    public static AlertSystem instance;

    private void Awake()
    {
        if (AlertSystem.instance != null)
        {
            return;
        }

        instance = this;
    }

    #endregion

    #region Prefabs 

    [Header("Prefabs")]
    [SerializeField] private GameObject[] AlertPrefabs;
    [SerializeField] private GameObject[] ButtonPrefabs;
    #endregion
    #region Methods
    /// <summary>
    /// Shows an alert with the specified type and settings.
    /// </summary>
    /// <param name="type">The type of alert to show.</param>
    /// <param name="settings">The settings for the alert.</param>
    /// <param name="alertParent">The parent transform under which the alert will be instantiated. If not provided, the alert will be instantiated under a Canvas found in the scene.</param>
    public void ShowAlert(AlertTypes type, AlertSettings settings,Transform alertParent = null)
    {
        settings.Type = type;
        if (alertParent == null)
        {
            alertParent = GameObject.FindObjectOfType<Canvas>().transform;
        }
        var popup = Instantiate(AlertPrefabs[(int)type],alertParent);
        popup.GetComponent<AlertPopup>().Initialize(settings);
    }

    public GameObject GetButton(AlertTypes type)
    {
        switch (type)
        {
            case AlertTypes.Danger:
                return ButtonPrefabs[0];
                break;
            case AlertTypes.Info:
                return ButtonPrefabs[1];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    #endregion

    public enum AlertTypes
    {
        Danger = 0,
        Info = 1
    }
}
[Serializable]
public class AlertSettings
{
    public List<AlertButton> AlertButtons { get; }
    public string Header;
    public string Content;
    public AlertSystem.AlertTypes Type;

    public AlertSettings(string header,string content, params AlertButton[] alertButtons)
    {
        Header = header;
        Content = content;
        AlertButtons = new List<AlertButton>(alertButtons);
    }
}

public class AlertButton
{
    public UnityAction buttonEvent;
    public string buttonContent;

    public AlertButton(UnityAction buttonEvent, string buttonContent)
    {
        this.buttonEvent = buttonEvent;
        this.buttonContent = buttonContent;
    }
}
}

using System.Collections;
using System.Collections.Generic;
using CG.Alert;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public void ShowDanger()
    {
        AlertSettings settings = new AlertSettings("Danger Header","Danger Content", 
            new AlertButton(ShowComplete,"BTN_DANGER"));
        
        //Boom!
        AlertSystem.instance.ShowAlert(AlertSystem.AlertTypes.Danger,settings);
    }

    public void ShowInfo()
    {
        AlertSettings settings = new AlertSettings("Info Header","Info Content", 
            new AlertButton(ShowComplete,"BTN_INFO"));
        
        //Done!
        AlertSystem.instance.ShowAlert(AlertSystem.AlertTypes.Info,settings);
    }


    public void ShowComplete()
    {
        Debug.Log("Popup successfully shown!");
    }
}

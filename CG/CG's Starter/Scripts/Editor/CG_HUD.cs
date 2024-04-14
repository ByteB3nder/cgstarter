using System.Collections;
using System.Collections.Generic;
using CG.Alert;
using CG.Audio;
using CG.VersionControl;
using UnityEditor;
using UnityEngine;

namespace CG.Starter
{
    [CustomEditor(typeof(CGManager))]
    public class CG_HUD : Editor
    {
        private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
        private bool Anticheat;
        private GUIStyle Centered;
        private GUIStyle Important;
        
        private Texture2D logoTexture;
        private Texture2D arrowDown;
        private Texture2D checkIcon;
        private GUIStyle darkModeStyle;
    
        //Booleans
        private bool encrypt;
        
        //Prefabs
        private GameObject PopupPrefab;
        private GameObject AudioPrefab;
        private GameObject VersionPrefab;
        private void OnEnable()
        {
            // Load the logo texture from Resources folder
            logoTexture = Resources.Load<Texture2D>("CG/Icons/CGLOGO");
            arrowDown = Resources.Load<Texture2D>("CG/Icons/ArrowDown"); 
            checkIcon = Resources.Load<Texture2D>("CG/Icons/Check"); 
            PopupPrefab = Resources.Load<GameObject>("CG/Prefabs/PopupSystem"); 
            AudioPrefab = Resources.Load<GameObject>("CG/Prefabs/AudioSystem"); 
            VersionPrefab = Resources.Load<GameObject>("CG/Prefabs/VersionSystem"); 

            ProcessStyles();
            
            // Create a dark mode style
            darkModeStyle = new GUIStyle();
            darkModeStyle.normal.textColor = Color.white;
        }
    
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // Get the target script
            CGManager targetScript = (CGManager)target;
    
            // Draw the HUD
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(logoTexture, GUILayout.Width(300), GUILayout.Height(300));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            //Set config..
            DrawInfos(targetScript);
            DrawPopup(targetScript);
            DrawAudio(targetScript);
            DrawVersionControl(targetScript);
            DrawQuickSave(targetScript);
            DrawQuickTime();
            
            
            // Draw the script's properties
            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
    
            serializedObject.ApplyModifiedProperties();
            // Apply dark mode if enabled
            if (EditorGUIUtility.isProSkin)
            {
                GUI.skin.label = darkModeStyle;
            }
            
            // Apply changes to the target script
            if (GUI.changed)
            {
                EditorUtility.SetDirty(targetScript);
            }
        }

        void DrawInfos(CGManager targetScript)
        {
            if (targetScript.Popup && !targetScript.PopupConfigured)
            {
                EditorGUILayout.HelpBox("You have enabled the popup system but you haven't configured it yet, so it won't work.", MessageType.Warning);
            }
            if (targetScript.AudioSystem && !targetScript.AudioConfigured)
            {
                EditorGUILayout.HelpBox("You have enabled the audio system but you haven't configured it yet, so it won't work.", MessageType.Warning);
            }
            if (targetScript.VersionSystem && !targetScript.VersionConfigured)
            {
                EditorGUILayout.HelpBox("You have enabled the version system but you haven't configured it yet, so it won't work.", MessageType.Warning);
            }
        }

        void DrawPopup(CGManager targetScript)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (targetScript.Popup)
            {
                var disabledStyle = new GUIStyle(Centered);
                disabledStyle.normal.textColor = Color.red;
                targetScript.Popup = GUILayout.Toggle(targetScript.Popup,"Disable Popup",disabledStyle);
            }
            else
            {
                if (targetScript.PopupConfigured)
                {
                    DestroyImmediate(FindObjectOfType<AlertSystem>().gameObject);
                    targetScript.PopupConfigured = false;
                }
                targetScript.Popup = GUILayout.Toggle(targetScript.Popup,"Enable Popup",Centered);
            }
            
            GUILayout.Label(arrowDown, GUILayout.Width(15), GUILayout.Height(15));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            // Draw the description
            if (targetScript.Popup)
            {
                var style = new GUIStyle(Centered);
                var buttonStyle = new GUIStyle(Centered);
                style.normal.textColor = Color.green;
                buttonStyle.normal.background= Texture2D.blackTexture;
                buttonStyle.normal.textColor = Color.white;
                buttonStyle.border = new RectOffset(15,15,15,15);
                EditorGUILayout.LabelField("Popup activated",style);

                if (!targetScript.PopupConfigured)
                {
                    if (GUILayout.Button("Configure Popup"))
                    {
                        targetScript.PopupConfigured = true;
                        var obj= Instantiate(PopupPrefab,GetParent());
                        obj.name = "Popup System - Instance";
                    }
                }
                
            }

           
        }

        void DrawAudio(CGManager targetScript)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (targetScript.AudioSystem)
            {
                var disabledStyle = new GUIStyle(Centered);
                disabledStyle.normal.textColor = Color.red;
                targetScript.AudioSystem = GUILayout.Toggle(targetScript.AudioSystem,"Disable Audio Manager",disabledStyle);
            }
            else
            {
                if (targetScript.AudioConfigured)
                {
                    DestroyImmediate(FindObjectOfType<AudioManager>().gameObject);
                    targetScript.AudioConfigured = false;
                }
                targetScript.AudioSystem = GUILayout.Toggle(targetScript.AudioSystem,"Enable Audio Manager",Centered);
            }
            
            GUILayout.Label(arrowDown, GUILayout.Width(15), GUILayout.Height(15));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            // Draw the description
            if (targetScript.AudioSystem)
            {
                var style = new GUIStyle(Centered);
                var buttonStyle = new GUIStyle(Centered);
                style.normal.textColor = Color.green;
                buttonStyle.normal.background= Texture2D.blackTexture;
                buttonStyle.normal.textColor = Color.white;
                buttonStyle.border = new RectOffset(15,15,15,15);
                EditorGUILayout.LabelField("Audio system activated",style);

                if (!targetScript.AudioConfigured)
                {
                    if (GUILayout.Button("Configure Audio"))
                    {
                        targetScript.AudioConfigured = true;
                        var obj= Instantiate(AudioPrefab,GetParent());
                        obj.name = "Audio System - Instance";
                    }
                }
                
            }

        }
        
        void DrawQuickSave(CGManager targetScript)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            var enabledStyle = new GUIStyle(Centered);
            enabledStyle.normal.textColor = Color.green;
            
            GUILayout.Label("Quick Save Enabled", enabledStyle);
            GUILayout.Label(checkIcon, GUILayout.Width(15), GUILayout.Height(15));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        void DrawQuickTime()
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            var enabledStyle = new GUIStyle(Centered);
            enabledStyle.normal.textColor = Color.green;
            if(GUILayout.Button("Quick Time Enabled", enabledStyle)) EditorUtility.OpenWithDefaultApp("https://cgstarter.gitbook.io");
            GUILayout.Label(checkIcon, GUILayout.Width(15), GUILayout.Height(15));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        void DrawVersionControl(CGManager targetScript)
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (targetScript.VersionSystem)
            {
                var disabledStyle = new GUIStyle(Centered);
                disabledStyle.normal.textColor = Color.red;
                targetScript.VersionSystem = GUILayout.Toggle(targetScript.VersionSystem,"Disable Version Control",disabledStyle);
            }
            else
            {
                if (targetScript.VersionConfigured)
                {
                    DestroyImmediate(FindObjectOfType<QuickVersion>().gameObject);
                    targetScript.VersionConfigured = false;
                }
                targetScript.VersionSystem = GUILayout.Toggle(targetScript.VersionSystem,"Enable Version Control",Centered);
            }
            
            GUILayout.Label(arrowDown, GUILayout.Width(15), GUILayout.Height(15));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            // Draw the description
            if (targetScript.VersionSystem)
            {
                var style = new GUIStyle(Centered);
                var buttonStyle = new GUIStyle(Centered);
                style.normal.textColor = Color.green;
                buttonStyle.normal.background= Texture2D.blackTexture;
                buttonStyle.normal.textColor = Color.white;
                buttonStyle.border = new RectOffset(15,15,15,15);
                EditorGUILayout.LabelField("Version control activated",style);

                if (!targetScript.VersionConfigured)
                {
                    if (GUILayout.Button("Configure Version System"))
                    {
                        targetScript.VersionConfigured = true;
                        var obj= Instantiate(VersionPrefab,GetParent());
                        obj.name = "Version System - Instance";
                    }
                }
                
            }
        }

        Transform GetParent()
        {
            var parent = GameObject.Find("CG Systems");

            if (parent == null)
            {
                GameObject newParent = new GameObject("CG Systems");
                newParent.AddComponent<CGMain>();
                return newParent.transform;
            }
            else
            {
                return parent.transform;
            }
        }

        private void ProcessStyles()
        {
            Centered = new GUIStyle();
            Centered.alignment = TextAnchor.MiddleCenter;
            Centered.fontStyle = FontStyle.Bold;
            Centered.normal.textColor = Color.white;

            Important = new GUIStyle();
            Important.normal.textColor = Color.red;
            Important.alignment = TextAnchor.MiddleCenter;
        }
        
    }
 
}

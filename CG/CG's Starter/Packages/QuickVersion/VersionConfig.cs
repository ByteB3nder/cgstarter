using System;
using UnityEngine;

namespace CG.VersionControl
{
    [CreateAssetMenu(menuName = "Version Control", fileName = "Version Config")]
    public class VersionConfig : ScriptableObject
    {
        public Version CurrentVersion;
        public string APIUrl = "http://polygunstudios.com/versioncontrol.php";
        public string APIKey;
    }
}
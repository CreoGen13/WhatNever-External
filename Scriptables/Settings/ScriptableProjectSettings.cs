using UnityEngine;

namespace Scriptables.Settings
{
    [CreateAssetMenu(fileName = "Project Settings", menuName = "Scriptables/Project Settings", order = 0)]
    public class ScriptableProjectSettings : ScriptableObject
    {
        [Header("Editor Settings")]
        public string containersFolderPath;
        
        [Header("Application Settings")]
        public int targetFrameRate;
    }
}
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MoonlitSystem
{
    public class ImmediateStyleSettings : ScriptableObject
    {
        private const string ImmediateStyleSettingsResDir = "Assets/ImmediateStyle/Resources";

        public static ImmediateStyleSettings LoadInstance()
        {
            var instance = Resources.Load<ImmediateStyleSettings>(nameof(ImmediateStyleSettings));

            if (instance == null) {
                Directory.CreateDirectory(ImmediateStyleSettingsResDir);
                var assetPath = Path.Combine(ImmediateStyleSettingsResDir, $"{nameof(ImmediateStyleSettings)}.asset");
                instance = CreateInstance<ImmediateStyleSettings>();
                instance.removeElementAutomatically = false;
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(instance, assetPath);
                AssetDatabase.SaveAssetIfDirty(instance);
#endif
            }

            return instance;
        }

        // We have this as false by default
        // Reasons why you might have this set to 'false'?
        // Because you find the component you meant to add not actually being the one you added?
        // A new user (to Unity) might get tripped on this. But in reality you should probably just set this to 'true'.
        public bool removeElementAutomatically;
    }
}
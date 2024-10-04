using System.IO;
using UnityEditor;
using UnityEngine;

namespace MoonlitSystem
{
    public class ImmediateStyleProjectSettings : ScriptableObject
    {
        private const string ImmediateStyleSettingsResDir = "Assets/ImmediateStyle/Resources";
        private const string ImmediateStyleSettingsFile = "ImmediateStyleSettings";
        private const string ImmediateStyleSettingsFileExtension = ".asset";

        public static ImmediateStyleProjectSettings LoadInstance()
        {
            var instance = Resources.Load<ImmediateStyleProjectSettings>(ImmediateStyleSettingsFile);

            if (instance == null) {
                Directory.CreateDirectory(ImmediateStyleSettingsResDir);
                var assetPath = Path.Combine(ImmediateStyleSettingsResDir, ImmediateStyleSettingsFile + ImmediateStyleSettingsFileExtension);
                instance = CreateInstance<ImmediateStyleProjectSettings>();
                instance.unused = "Here temporarily until real settings get added.";
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(instance, assetPath);
                AssetDatabase.SaveAssets();
#endif
            }

            return instance;
        }

        public string unused; // @placeholder. Here temporarily until real settings get added.
    }
}
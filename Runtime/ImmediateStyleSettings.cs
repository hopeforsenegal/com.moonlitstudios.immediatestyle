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
                instance.unused = "Here temporarily until real settings get added.";
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(instance, assetPath);
                AssetDatabase.SaveAssetIfDirty(instance);
#endif
            }

            return instance;
        }

        public string unused; // @placeholder. Here temporarily until real settings get added.
    }
}
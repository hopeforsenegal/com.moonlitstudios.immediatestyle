using System.IO;
using UnityEditor;
using UnityEngine;

namespace MoonlitSystem
{
    public class ImmediateStyleProjectSettings : ScriptableObject
    {
        private const string MenuLocationInProjectSettings = "Project/ImmediateStyle"; //@copied manually

        private const string ImmediateStyleSettingsResDir = "Assets/ImmediateStyle/Resources";
        private const string ImmediateStyleSettingsFile = "ImmediateStyleSettings";
        private const string ImmediateStyleSettingsFileExtension = ".asset";

        [MenuItem("Moonlit/ImmediateStyle/Settings", priority = 0)]
        private static void SendToProjectSettings()
        {
            SettingsService.OpenProjectSettings(MenuLocationInProjectSettings);
        }

        internal static ImmediateStyleProjectSettings LoadInstance()
        {
            var instance = Resources.Load<ImmediateStyleProjectSettings>(ImmediateStyleSettingsFile);

            if (instance == null) {
                Directory.CreateDirectory(ImmediateStyleSettingsResDir);
                var assetPath = Path.Combine(ImmediateStyleSettingsResDir, ImmediateStyleSettingsFile + ImmediateStyleSettingsFileExtension);
                instance = CreateInstance<ImmediateStyleProjectSettings>();
                AssetDatabase.CreateAsset(instance, assetPath);
                instance.followCursorRetained = FollowCursorRetained.OverrideNoFollowCursor; // @default
                AssetDatabase.SaveAssets();
            }

            return instance;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(LoadInstance());
        }

        public enum FollowCursorRetained { NoOverride, OverrideFollowCursor, OverrideNoFollowCursor }

        // DragAndDrop components by default follow cursor (in order to be consistent with a typical Unity UI Component). 
        // However if that component is engaged with the ImmediateStyle we override them to all not follow cursor. (so you have to explicitly call ImmediateStyle.FollowCursor)
        // But of course here you can change that in case you want to prescribe a different default

        // NOTE: I strongly suspect that both ways work without an override setting.
        // However we'll keep this for now and will possibly remove it once there are more settings (So we have a place to easily add new settings)
        public FollowCursorRetained followCursorRetained = FollowCursorRetained.OverrideNoFollowCursor; // @default
    }
}
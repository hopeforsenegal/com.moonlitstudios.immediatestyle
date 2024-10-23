using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MoonlitSystem
{
    public class ImmediateStyleSettingsProvider : SettingsProvider
    {
        [MenuItem("Moonlit/ImmediateStyle/Settings", priority = 0)]
        private static void SendToProjectSettings()
        {
            SettingsService.OpenProjectSettings(MenuLocationInProjectSettings);
        }

        [SettingsProvider]
        public static SettingsProvider CreateImmediateStyleSettingsProvider()
        {
            return new ImmediateStyleSettingsProvider(MenuLocationInProjectSettings);
        }

        public const string MenuLocationInProjectSettings = "Project/ImmediateStyle";

        private SerializedObject m_ProjectSettings;
        private SerializedProperty m_Unused;

        private ImmediateStyleSettingsProvider(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            m_ProjectSettings = new SerializedObject(ImmediateStyleSettings.LoadInstance());  // GetSerializedSettings();
            m_Unused = m_ProjectSettings.FindProperty(nameof(ImmediateStyleSettings.unused));
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(m_Unused, new GUIContent("-Placeholder-"));
            EditorGUI.EndDisabledGroup();

            m_ProjectSettings.ApplyModifiedProperties();
        }
    }
}
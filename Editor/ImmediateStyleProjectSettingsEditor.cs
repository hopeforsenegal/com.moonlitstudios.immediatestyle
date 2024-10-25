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
        private SerializedProperty m_RemoveElementAutomatically;

        private ImmediateStyleSettingsProvider(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            m_ProjectSettings = new SerializedObject(ImmediateStyleSettings.LoadInstance());  // GetSerializedSettings();
            m_RemoveElementAutomatically = m_ProjectSettings.FindProperty(nameof(ImmediateStyleSettings.removeElementAutomatically));
        }

        public override void OnGUI(string searchContext)
        {
            GUILayout.Label("The Element Component is actually inert (its there to save users time on selecting multiple components of different types):", EditorStyles.boldLabel);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(m_RemoveElementAutomatically);

            m_ProjectSettings.ApplyModifiedProperties();
        }
    }
}
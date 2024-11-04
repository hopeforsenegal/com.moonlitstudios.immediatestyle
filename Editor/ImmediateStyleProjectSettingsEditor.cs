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

        private const string MenuLocationInProjectSettings = "Project/ImmediateStyle";

        private SerializedObject m_ProjectSettings;
        private SerializedProperty m_RemoveElementAutomatically;
        private SerializedProperty m_InlineClipboardGUIDs;

        private ImmediateStyleSettingsProvider(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            m_ProjectSettings = new SerializedObject(ImmediateStyleSettings.LoadInstance());  // GetSerializedSettings();
            m_RemoveElementAutomatically = m_ProjectSettings.FindProperty(nameof(ImmediateStyleSettings.removeElementAutomatically));
            m_InlineClipboardGUIDs = m_ProjectSettings.FindProperty(nameof(ImmediateStyleSettings.inlineClipboardGUIDS));
        }

        public override void OnGUI(string searchContext)
        {
            GUILayout.Label("The Element Component is actually inert (its there to save users time on selecting multiple components of different types):", EditorStyles.boldLabel);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(m_RemoveElementAutomatically);

            GUILayout.Space(20);
            GUILayout.Label("Stop creating a separate const for GUIDs (instead of having them separately from being on the ImmediateStyle._Action_() call for ex.)", EditorStyles.boldLabel);
            GUILayout.Label("ImmediateStyle.Text(\"guid_as_string\") vs ImmediateStyle.Text(guid_as_const_var)", EditorStyles.boldLabel);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(m_InlineClipboardGUIDs);

            m_ProjectSettings.ApplyModifiedProperties();
        }
    }
}
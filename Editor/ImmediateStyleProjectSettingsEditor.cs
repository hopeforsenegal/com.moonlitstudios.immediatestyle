using MoonlitSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class ImmediateStyleSettingsProvider : SettingsProvider
    {
        public const string MenuLocationInProjectSettings = "Project/ImmediateStyle";

        private SerializedObject m_ProjectSettings;
        private SerializedProperty m_FollowCursorRetained;

        private ImmediateStyleSettingsProvider(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            m_ProjectSettings = new SerializedObject(ImmediateStyleProjectSettings.LoadInstance());  // GetSerializedSettings();
            m_FollowCursorRetained = m_ProjectSettings.FindProperty(nameof(ImmediateStyleProjectSettings.followCursorRetained));
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.PropertyField(m_FollowCursorRetained, new GUIContent("Change ImmediateStyle Follow cursor default behaviour for DragAndDrop."));

            m_ProjectSettings.ApplyModifiedProperties();
        }

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
    }
}
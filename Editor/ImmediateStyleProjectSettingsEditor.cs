using MoonlitSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class ImmediateStyleSettingsProvider : SettingsProvider
    {
        public const string MenuLocationInProjectSettings = "Project/ImmediateStyle"; //@copied manually

        private SerializedObject _projectSettings;
        private SerializedProperty _followCursorRetained;

        private ImmediateStyleSettingsProvider(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _projectSettings = ImmediateStyleProjectSettings.GetSerializedSettings();
            _followCursorRetained = _projectSettings.FindProperty(nameof(ImmediateStyleProjectSettings.followCursorRetained));
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.PropertyField(_followCursorRetained, new GUIContent("Change ImmediateStyle Follow cursor default behaviour for DragAndDrop."));

            _projectSettings.ApplyModifiedProperties();
        }

        [SettingsProvider]
        public static SettingsProvider CreateImmediateStyleSettingsProvider()
        {
            return new ImmediateStyleSettingsProvider(MenuLocationInProjectSettings);
        }
    }
}
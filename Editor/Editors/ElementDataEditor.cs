using System.Collections.Generic;
using MoonlitSystem.UI.Immediate;
using UnityEditor;
using UnityEngine;

namespace MoonlitSystem.Editors
{
    public static class ElementDataEditor
    {
        private static (string[], string[]) GetIDSFromTargets(Object[] targets)
        {
            var elementIds = new List<string>();
            var rootMappingIDs = new HashSet<string>();
            foreach (var t in targets) {
                if (!(t is BaseEditorData ed)) continue;

                var rootMapping = RootMapping.GetFirstParentOrAssert(ed);
                if (rootMapping != null) {
                    if (!rootMappingIDs.Contains(rootMapping.ID)) rootMappingIDs.Add(rootMapping.ID);
                }
                elementIds.Add(ed.ElementData.ID);
            }
            return (elementIds.ToArray(), new List<string>(rootMappingIDs).ToArray());
        }

        private static bool IsAllUniqueGuid(string[] guids)
        {
            var hash = new HashSet<string>();
            foreach (var guid in guids) {
                if (hash.Contains(guid)) return false;
                hash.Add(guid);
            }
            return true;
        }

        // if single just show guid (and red on empty)
        // if multiple show guid if it matches
        //   otherwise show a dash to show they differ
        public static void Render(Object[] targets)
        {
            var (elementIds, rootMappingIDs) = GetIDSFromTargets(targets);
            var isSingleElement = elementIds.Length <= 1;
            var isSingleRootMapping = rootMappingIDs.Length <= 1;
            var hasNoSingleRootMapping = rootMappingIDs.Length == 0;
            var elementValue = "-";
            var rootMappingValue = "-";
            if (isSingleElement) elementValue = elementIds[0];
            if (!isSingleElement && !IsAllUniqueGuid(elementIds)) elementValue = elementIds[0];

            if (!hasNoSingleRootMapping) {
                if (isSingleRootMapping) rootMappingValue = rootMappingIDs[0];
                if (!isSingleRootMapping && !IsAllUniqueGuid(rootMappingIDs)) rootMappingValue = rootMappingIDs[0];
                using (new HorizontalScope()) {
                    GUILayout.Label("[RM]", new GUIStyle(EditorStyles.boldLabel) { fixedWidth = 40 });
                    EditorGUI.BeginDisabledGroup(true);
                    GUILayout.TextField(rootMappingValue, GUI.skin.textField);
                    EditorGUI.EndDisabledGroup();
                }
            }
            using (new HorizontalScope()) {
                var s = new GUIStyle(EditorStyles.boldLabel);
                if (string.IsNullOrWhiteSpace(elementValue)) s.normal.textColor = Color.red;

                s.fixedWidth = 40;
                GUILayout.Label("[GUID]", s);
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.TextField(elementValue, GUI.skin.textField);
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.Space();
        }
    }
}
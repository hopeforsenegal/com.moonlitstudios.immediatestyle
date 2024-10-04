using System.Collections.Generic;
using MoonlitSystem.UI.Immediate;
using UnityEditor;
using UnityEngine;

namespace Editor.Editors
{
    public static class ElementDataEditor
    {
        public static string[] GetIDSFromTargets(Object[] targets)
        {
            var ids = new List<string>();
            foreach (var t in targets) {
                if (t is Reference r) {
                    ids.Add(r.ElementData.ID);
                }
                if (t is ElementButton eb) {
                    ids.Add(eb.ElementData.ID);
                }
                if (t is ElementText et) {
                    ids.Add(et.ElementData.ID);
                }
                if (t is ElementToggle et2) {
                    ids.Add(et2.ElementData.ID);
                }
                if (t is ElementDragDrop edd) {
                    ids.Add(edd.ElementData.ID);
                }
                if (t is ElementInputField eif) {
                    ids.Add(eif.ElementData.ID);
                }
                if (t is ElementSlider es) {
                    ids.Add(es.ElementData.ID);
                }
                if (t is ElementDropdown ed) {
                    ids.Add(ed.ElementData.ID);
                }
                if (t is ElementCanvasGroup ecg) {
                    ids.Add(ecg.ElementData.ID);
                }
                if (t is ElementImage ei) {
                    ids.Add(ei.ElementData.ID);
                }
                // This will break when we add a new element and forget to place it here
            }
            return ids.ToArray();
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
            var ids = GetIDSFromTargets(targets);
            var isSingle = ids.Length <= 1;
            var value = "-";
            if (isSingle) value = ids[0];
            if (!isSingle && !IsAllUniqueGuid(ids)) value = ids[0];

            using (new HorizontalScope()) {
                var s = new GUIStyle(EditorStyles.boldLabel);
                if (string.IsNullOrWhiteSpace(value)) s.normal.textColor = Color.red;

                GUILayout.Label("[GUID]", s);
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.TextField(value, GUI.skin.textField, GUILayout.Width(200));
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.Space();
        }
    }
}
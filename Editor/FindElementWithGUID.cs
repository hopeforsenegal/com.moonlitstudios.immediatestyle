using UnityEngine;
using UnityEditor;
using System.Linq;
using MoonlitSystem.UI.Immediate;

namespace MoonlitSystem
{
    public class FindElementWithGUID : EditorWindow
    {
        [MenuItem("Moonlit/ImmediateStyle/Find Element By GUID", priority = 100)]
        public static void ShowWindow()
        {
            GetWindow<FindElementWithGUID>("Find Element");
        }

        private string searchString = "";

        protected void OnGUI()
        {
            GUILayout.Label("Search for Element with....", EditorStyles.boldLabel);
            searchString = EditorGUILayout.TextField("GUID", searchString);

            if (GUILayout.Button("Find and Ping")) {
                var objectsWithCustomComponent = FindObjectsOfType<BaseEditorData>();
                var foundObject = objectsWithCustomComponent
                    .FirstOrDefault(predicate: obj => obj.ElementData.ID == searchString);

                if (foundObject != null) {
                    var foundGO = foundObject.gameObject;
                    Selection.activeGameObject = foundGO;

                    EditorGUIUtility.PingObject(foundGO);

                    Debug.Log($"Found and pinged: {foundGO.name}");
                } else {
                    Debug.Log("No matching object found.");
                }
            }
        }
    }
}
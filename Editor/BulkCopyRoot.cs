using MoonlitSystem.UI.Immediate;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MoonlitSystem
{
    public class BulkCopyRoot : ScriptableWizard
    {
        public int numberOfCopies;

        [MenuItem("Moonlit/ImmediateStyle/Bulk Copy + Add RootMap")]
        protected static void CreateWizard()
        {
            DisplayWizard("Bulk Copy + RootMap", typeof(BulkCopyRoot));
        }
        void OnWizardUpdate()
        {
            helpString = "Duplicates the currently selected GameObject.";

            isValid = Selection.activeGameObject != null && numberOfCopies > 0;

            if (Selection.activeGameObject == null)
            {
                errorString = "Select a GameObject in the Hierarchy.";
            }
            else if (numberOfCopies <= 0)
            {
                errorString = "Number of copies must be greater than zero.";
            }
            else
            {
                errorString = "";
            }
        }

        void OnWizardCreate()
        {
            GameObject original = Selection.activeGameObject;
            original.name = $"({original.transform.GetSiblingIndex() + 1})";
            var parentName = original.transform.parent.name;
            var originalRootMapping = original.GetComponent<RootMapping>();
            if (originalRootMapping == null)
            {
                originalRootMapping = Undo.AddComponent<RootMapping>(original);
            }
            Undo.RecordObject(originalRootMapping, "Set RootMapping ID");
            originalRootMapping.ID = $"{parentName}_{originalRootMapping.transform.GetSiblingIndex()}";
            EditorUtility.SetDirty(originalRootMapping);

            for (int i = 0; i < numberOfCopies; i++)
            {
                GameObject copy = (GameObject)PrefabUtility.InstantiatePrefab(original);

                if (copy == null)
                {
                    copy = Instantiate(original);
                }

                copy.transform.SetParent(original.transform.parent, true);
                copy.transform.SetSiblingIndex(original.transform.GetSiblingIndex() + i + 1);
                copy.transform.localScale = Vector3.one;

                var map = copy.GetComponent<RootMapping>();
                if (map == null)
                {
                    map = Undo.AddComponent<RootMapping>(copy);
                }

                Undo.RecordObject(map, "Set RootMapping ID");
                map.ID = $"{parentName}_{copy.transform.GetSiblingIndex()}";
                EditorUtility.SetDirty(map);


                copy.name = $"({copy.transform.GetSiblingIndex() + 1})";

                Undo.RegisterCreatedObjectUndo(copy, "Duplicate GameObject");
            }

            Selection.activeGameObject = original;
            EditorSceneManager.MarkSceneDirty(original.scene);
        }
    }
}

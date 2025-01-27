using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonlitSystem.TemplateGenerators;
using MoonlitSystem.UI.Immediate;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using static MoonlitSystem.Editors.UI;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;

namespace MoonlitSystem.Editors
{
    public static class Builder
    {
        public static void HandleUserChoice(Choice choice, Object[] targets, Type type)
        {
            switch (choice) {
                case Choice.CopyID: Helper.ClipboardText = BuildConstantStatements(targets); break;
                case Choice.RegenerateRandomID: RegenerateRandomIDsForTargets(targets); break;
                case Choice.UseGameObjectNameID: UseGameObjectNameIDsForTargets(targets); break;
                case Choice.CopyCode: {
                        if (type == typeof(ElementButtonEditor)) Helper.ClipboardText = CopyCodeButtonStatement(targets);
                        if (type == typeof(ElementImageEditor)) Helper.ClipboardText = CopyCodeImageStatement(targets);
                        if (type == typeof(ElementToggleEditor)) Helper.ClipboardText = CopyCodeToggleStatement(targets);
                        if (type == typeof(ElementDragDropEditor)) Helper.ClipboardText = CopyCodeDragDropStatement(targets);
                        if (type == typeof(ElementTextEditor)) Helper.ClipboardText = CopyCodeTextStatement(targets);
                        if (type == typeof(ElementInputFieldEditor)) Helper.ClipboardText = CopyCodeInputFieldStatement(targets);
                        if (type == typeof(ElementSliderEditor)) Helper.ClipboardText = CopyCodeSliderStatement(targets);
                        if (type == typeof(ElementDropdownEditor)) Helper.ClipboardText = CopyCodeDropdownStatement(targets);
                        if (type == typeof(ElementCanvasGroupEditor)) Helper.ClipboardText = CopyCodeCanvasGroupStatement(targets);
                        if (type == typeof(ElementCanvasEditor)) Helper.ClipboardText = CopyCodeCanvasGroupStatement(targets);
                        if (type == typeof(ReferenceEditor)) Helper.ClipboardText = CopyCodeReferenceStatement(targets);
                        break;
                    }
                    // These choice only apply to CanvasGroup
            }
        }

        private static string CopyCodeButtonStatement(Object[] targets)
        {
            var constantStatements = string.Empty;
            var elementStatements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            foreach (var t in targets) {
                var element = (ElementButton)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (!settings.inlineClipboardGUIDS) {
                    var elementStatement = BuildButtonStatement(constant);
                    constantStatements += constantStatement + Environment.NewLine;
                    elementStatements += elementStatement + Environment.NewLine;
                } else {
                    var elementStatement = BuildButtonStatement($"\"{id}\"");
                    elementStatements += elementStatement + Environment.NewLine;
                }
            }

            return constantStatements + elementStatements;
        }

        private static string CopyCodeImageStatement(Object[] targets)
        {
            var constantStatements = string.Empty;
            var elementStatements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            foreach (var t in targets) {
                var element = (ElementImage)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (!settings.inlineClipboardGUIDS) {
                    var elementStatement = BuildImageStatement(constant);
                    constantStatements += constantStatement + Environment.NewLine;
                    elementStatements += elementStatement + Environment.NewLine;
                } else {
                    var elementStatement = BuildImageStatement($"\"{id}\"");
                    elementStatements += elementStatement + Environment.NewLine;
                }
            }

            return constantStatements + elementStatements;
        }

        private static string CopyCodeToggleStatement(Object[] targets)
        {
            var constantStatements = string.Empty;
            var elementStatements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            foreach (var t in targets) {
                var element = (ElementToggle)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (!settings.inlineClipboardGUIDS) {
                    var elementStatement = BuildToggleStatement(constant);
                    constantStatements += constantStatement + Environment.NewLine;
                    elementStatements += elementStatement + Environment.NewLine;
                } else {
                    var elementStatement = BuildToggleStatement($"\"{id}\"");
                    elementStatements += elementStatement + Environment.NewLine;
                }
            }

            return constantStatements + elementStatements;
        }

        private static string CopyCodeDragDropStatement(Object[] targets)
        {
            var constantStatements = string.Empty;
            var elementStatements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            foreach (var t in targets) {
                var element = (ElementDragDrop)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (!settings.inlineClipboardGUIDS) {
                    var elementStatement = BuildDragDropStatement(constant);
                    constantStatements += constantStatement + Environment.NewLine;
                    elementStatements += elementStatement + Environment.NewLine;
                } else {
                    var elementStatement = BuildDragDropStatement($"\"{id}\"");
                    elementStatements += elementStatement + Environment.NewLine;
                }
            }

            return constantStatements + elementStatements;
        }

        private static string CopyCodeTextStatement(Object[] targets)
        {
            var constantStatements = string.Empty;
            var elementStatements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            foreach (var t in targets) {
                var element = (ElementText)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (!settings.inlineClipboardGUIDS) {
                    var elementStatement = BuildTextStatement(constant);
                    constantStatements += constantStatement + Environment.NewLine;
                    elementStatements += elementStatement + Environment.NewLine;
                } else {
                    var elementStatement = BuildTextStatement($"\"{id}\"");
                    elementStatements += elementStatement + Environment.NewLine;
                }
            }

            return constantStatements + elementStatements;
        }

        private static string CopyCodeInputFieldStatement(Object[] targets)
        {
            var constantStatements = string.Empty;
            var elementStatements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            foreach (var t in targets) {
                var element = (ElementInputField)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (!settings.inlineClipboardGUIDS) {
                    var elementStatement = BuildInputFieldStatement(constant);
                    constantStatements += constantStatement + Environment.NewLine;
                    elementStatements += elementStatement + Environment.NewLine;
                } else {
                    var elementStatement = BuildInputFieldStatement($"\"{id}\"");
                    elementStatements += elementStatement + Environment.NewLine;
                }
            }

            return constantStatements + elementStatements;
        }

        private static string CopyCodeSliderStatement(Object[] targets)
        {
            var constantStatements = string.Empty;
            var elementStatements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            foreach (var t in targets) {
                var element = (ElementSlider)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (!settings.inlineClipboardGUIDS) {
                    var elementStatement = BuildSliderStatement(constant);
                    constantStatements += constantStatement + Environment.NewLine;
                    elementStatements += elementStatement + Environment.NewLine;
                } else {
                    var elementStatement = BuildSliderStatement($"\"{id}\"");
                    elementStatements += elementStatement + Environment.NewLine;
                }
            }

            return constantStatements + elementStatements;
        }

        private static string CopyCodeDropdownStatement(Object[] targets)
        {
            var constantStatements = string.Empty;
            var elementStatements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            foreach (var t in targets) {
                var element = (ElementDropdown)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (!settings.inlineClipboardGUIDS) {
                    var elementStatement = BuildDropdownStatement(constant);
                    constantStatements += constantStatement + Environment.NewLine;
                    elementStatements += elementStatement + Environment.NewLine;
                } else {
                    var elementStatement = BuildDropdownStatement($"\"{id}\"");
                    elementStatements += elementStatement + Environment.NewLine;
                }
            }

            return constantStatements + elementStatements;
        }

        private static string CopyCodeCanvasGroupStatement(Object[] targets)
        {
            var constantStatements = string.Empty;
            var elementStatements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            foreach (var t in targets) {
                var element = (ElementCanvasGroup)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (!settings.inlineClipboardGUIDS) {
                    var elementStatement = BuildCanvasGroupStatement(constant);
                    constantStatements += constantStatement + Environment.NewLine;
                    elementStatements += elementStatement + Environment.NewLine;
                } else {
                    var elementStatement = BuildCanvasGroupStatement($"\"{id}\"");
                    elementStatements += elementStatement + Environment.NewLine;
                }
            }

            return constantStatements + elementStatements;
        }

        private static string CopyCodeReferenceStatement(Object[] targets)
        {
            var statements = string.Empty;
            var settings = ImmediateStyleSettings.LoadInstance();
            var resultStatement = string.Empty;
            foreach (var t in targets) {
                var element = (Reference)t;
                var id = element.ElementData.ID;
                var (constantStatement, constant) = BuildConstantStatement(id);
                if (settings.inlineClipboardGUIDS) {
                    constantStatement = string.Empty;
                    constant = $"\"{id}\"";
                }
                if (element.GetComponent<CanvasGroup>() != null) {
                    var elementStatement = BuildReferenceStatement(constant, nameof(CanvasGroup));
                    statements += constantStatement + Environment.NewLine + elementStatement;
                } else if (element.GetComponent<Button>() != null) {
                    var elementStatement = BuildReferenceStatement(constant, nameof(Button));
                    statements += constantStatement + Environment.NewLine + elementStatement;
                } else {
                    // Default to the first other component we come across
                    var components = element.gameObject.GetComponents<Component>();
                    Debug.Assert(components.Length >= 2, "Have reference and at least Transform");
                    var result = 0;
                    for (var i = components.Length - 1; i >= 0; i--) {
                        if (components[i] == element) continue;
                        if (components[i].GetType().Name == nameof(CanvasRenderer)) continue;
                        if (components[i].GetType().Name == nameof(Element)) continue;
                        if (components[i].GetType().Name == nameof(ElementButton)) continue;
                        if (components[i].GetType().Name == nameof(ElementCanvasGroup)) continue;
                        if (components[i].GetType().Name == nameof(ElementDragDrop)) continue;
                        if (components[i].GetType().Name == nameof(ElementDropdown)) continue;
                        if (components[i].GetType().Name == nameof(ElementImage)) continue;
                        if (components[i].GetType().Name == nameof(ElementInputField)) continue;
                        if (components[i].GetType().Name == nameof(ElementSlider)) continue;
                        if (components[i].GetType().Name == nameof(ElementText)) continue;
                        if (components[i].GetType().Name == nameof(ElementToggle)) continue;

                        result = i;
                        break;
                    }
                    var elementStatement = BuildReferenceStatement(constant, components[result].GetType().Name);
                    statements += constantStatement + Environment.NewLine + elementStatement;
                }

                resultStatement = statements + Environment.NewLine;
            }

            return resultStatement;
        }

        private static string BuildConstantStatements(Object[] targets)
        {
            var statements = string.Empty;
            foreach (var t in targets) {
                var element = (IEditorData)t;
                var id = element.Data.ID;
                var (constantStatement, _) = BuildConstantStatement(id);
                statements += constantStatement + Environment.NewLine;
            }

            return statements;
        }

        private static string RemoveChars(this string s, IEnumerable<char> separators)
        {
            var sb = new StringBuilder(s);
            foreach (var c in separators) { sb.Replace(c.ToString(), ""); }
            return sb.ToString();
        }
        private static string CleanString(string str)
        {
            return str.RemoveChars(new[] { '/', '(', ')', ' ' });
        }
        private static (string, string) BuildConstantStatement(string id)
        {
            return ($"const string {CleanString(id)} = \"{id}\";", CleanString(id));
        }
        private static string BuildButtonStatement(string constant)
        {
            return $"ImmediateStyle.Button({constant});";
        }
        private static string BuildCanvasGroupStatement(string constant)
        {
            return $"ImmediateStyle.CanvasGroup({constant});";
        }
        private static string BuildImageStatement(string constant)
        {
            return $"ImmediateStyle.Image({constant}, null);";
        }
        private static string BuildToggleStatement(string constant)
        {
            return $"ImmediateStyle.Toggle({constant}, false);";
        }
        private static string BuildDragDropStatement(string constant)
        {
            return $"ImmediateStyle.DragDrop({constant}, out _);";
        }
        private static string BuildInputFieldStatement(string constant)
        {
            return $"ImmediateStyle.InputField({constant}, new []{{ KeyCode.Return, KeyCode.KeypadEnter }}, ref text);";
        }
        private static string BuildTextStatement(string constant)
        {
            return $"ImmediateStyle.Text({constant}, \"Placeholder\");";
        }
        private static string BuildSliderStatement(string constant)
        {
            return $"ImmediateStyle.Slider({constant});";
        }
        private static string BuildDropdownStatement(string constant)
        {
            return $"ImmediateStyle.Dropdown({constant}, new UnityEngine.UI.Dropdown.OptionData[] {{ new(\"Thing\")}});";
        }
        private static string BuildReferenceStatement(string constant, string theType = "")
        {
            return $"Reference.Find<{theType}>(this, {constant});";
        }

        private static void RegenerateRandomIDsForTargets(Object[] targets)
        {
            foreach (var t in targets) {
                var element = (IEditorData)t;
                ElementData.SetupElementData(element.Data, element.transform);
                EditorUtility.SetDirty(t);
            }
        }

        private static void UseGameObjectNameIDsForTargets(Object[] targets)
        {
            foreach (var t in targets) {
                var element = (IEditorData)t;
                ElementData.SetupElementDataGameObjectName(element.Data, element.transform);
                EditorUtility.SetDirty(t);
            }
        }
    }

    public static class UI
    {
        public enum Choice
        {
            CopyID = 1,
            CopyCode,
            RegenerateRandomID,
            UseGameObjectNameID,
            FileTemplate,
            ElementTemplate,
            ForLoopTemplate,
        }

        private static readonly Color HighlightButton = new Color(235 / 255f, 235 / 255f, 255 / 255f);

        [MenuItem("GameObject/ImmediateStyle/Regenerate selected GUIDs", false, 20000)]
        private static void RegenerateSelectedGUID(MenuCommand _)
        {
            foreach (var go in Selection.gameObjects) {
                if (go.TryGetComponent<BaseEditorData>(out var c)) {
                    ElementData.SetupElementData(c.ElementData, c.transform);
                    EditorUtility.SetDirty(go);
                }
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        [MenuItem("GameObject/ImmediateStyle/Regenerate children GUIDs", false, 20000)]
        private static void RegenerateChildrenGUIDs(MenuCommand _)
        {
            foreach (var go in Selection.gameObjects) {
                foreach (var c in go.GetComponentsInChildren<BaseEditorData>()) {
                    if (c == null) continue;
                    ElementData.SetupElementData(c.ElementData, c.transform);
                    EditorUtility.SetDirty(go);
                }
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        [MenuItem("GameObject/ImmediateStyle/Add Element to all UI children", false, 20000)]
        private static void AddElementsToAllChildren(MenuCommand _)
        {
            foreach (var go in Selection.gameObjects) {
                AddElementToUIGameObjects(go);
                TraverseHierarchy(go.transform);
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        private static void TraverseHierarchy(Transform root)
        {
            foreach (Transform child in root) {
                AddElementToUIGameObjects(child.gameObject);
                TraverseHierarchy(child);
            }
        }

        private static void AddElementToUIGameObjects(GameObject gameObject)
        {
            if (!gameObject.GetComponent<UIBehaviour>() && !gameObject.GetComponent<CanvasGroup>()) return;
            if (gameObject.GetComponent<BaseEditorData>()) return;
            gameObject.AddComponent<Element>();
        }

        [MenuItem("GameObject/ImmediateStyle/Regenerate selected GUIDs", true)]
        private static bool ValidateRegenerateSelectedGUID() => HasAllElements();

        [MenuItem("GameObject/ImmediateStyle/Regenerate children GUIDs", true)]
        private static bool ValidateRegenerateChildrenGUIDs() => HasAllElements();

        private static bool HasAllElements()
        {
            foreach (var go in Selection.gameObjects) {
                if (go.GetComponent<BaseEditorData>() == null) return false;
            }
            return true;
        }

        public static Choice RenderUserSelections(Object[] targets, ref bool isIDOptionsExpanded)
        {
            var choice = RenderCodeSnippet(targets);
            RenderGUIDOptions(ref isIDOptionsExpanded, ref choice);
            return choice;
        }

        private static Choice RenderCodeSnippet(Object[] targets)
        {
            Choice choice = default;
            ElementDataEditor.Render(targets);
            var originalColor = GUI.backgroundColor;
            GUI.backgroundColor = HighlightButton;
            if (GUILayout.Button("Clipboard Copy Code Snippet")) choice = Choice.CopyCode;
            GUI.backgroundColor = originalColor;
            return choice;
        }

        private static void RenderGUIDOptions(ref bool isIDOptionsExpanded, ref Choice choice)
        {
            using (new EditorGUILayout.HorizontalScope()) {
                isIDOptionsExpanded = EditorGUILayout.Foldout(isIDOptionsExpanded, string.Empty, true);
                GUILayout.Label("GUID Options", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }
            if (isIDOptionsExpanded) {
                if (GUILayout.Button("Copy ID")) choice = Choice.CopyID;
                if (GUILayout.Button("Regenerate ID")) choice = Choice.RegenerateRandomID;
                if (GUILayout.Button("Use Game Object Name as ID")) choice = Choice.UseGameObjectNameID;
            }
        }

        public static Choice RenderCanvasGroupUserSelections(Object[] targets, ref bool isIDOptionsExpanded, ref bool isChildrenExpanded)
        {
            var choice = RenderCodeSnippet(targets);
            RenderCanvasGroupUserSelectionsInner(ref isChildrenExpanded, ref choice);
            RenderGUIDOptions(ref isIDOptionsExpanded, ref choice);
            return choice;
        }

        public static Choice RenderCanvas_JustCanvas_UserSelections(ref bool isIDOptionsExpanded, ref bool isChildrenExpanded)
        {
            Choice choice = default;
            RenderCanvasGroupUserSelectionsInner(ref isChildrenExpanded, ref choice);
            RenderGUIDOptions(ref isIDOptionsExpanded, ref choice);
            return choice;
        }

        private static void RenderCanvasGroupUserSelectionsInner(ref bool isChildrenExpanded, ref Choice choice)
        {
            using (new EditorGUILayout.HorizontalScope()) {
                isChildrenExpanded = EditorGUILayout.Foldout(isChildrenExpanded, string.Empty, true);
                GUILayout.Label("Code Generate using Children", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }
            if (isChildrenExpanded) {
                using (new EditorGUILayout.HorizontalScope()) {
                    var originalColor = GUI.backgroundColor;
                    GUI.backgroundColor = HighlightButton;
                    if (GUILayout.Button("Clipboard Individual")) choice = Choice.ElementTemplate;
                    GUI.backgroundColor = originalColor;
                    if (GUILayout.Button("Clipboard For-Loop")) choice = Choice.ForLoopTemplate;
                }
                if (GUILayout.Button("Create Full Code File")) choice = Choice.FileTemplate;
            }
        }

        public static void ElementDestroy(GameObject gameObject, Object o)
        {
            if (EditorApplication.isPlaying) return;
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return;

            // The component was removed but not the gameobject
            if (o == null && gameObject != null) {
                // Remove Dependents
                Object.DestroyImmediate(gameObject.GetComponent<Element>());
                Debug.Log($"The component and dependent ({nameof(Element)}) was removed.");
            } else if (o == null && gameObject == null) {
                // The gameobject was deleted
                Debug.Log("The GameObject was deleted from the scene.");
            }
        }
    }

    [CustomEditor(typeof(ElementButton), true)]
    [CanEditMultipleObjects]
    public class ElementButtonEditor : Editor
    {
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementButton)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);
        public override void OnInspectorGUI() => Builder.HandleUserChoice(RenderUserSelections(targets, ref isIDOptionsExpanded), targets, GetType());
    }

    [CustomEditor(typeof(ElementImage), true)]
    [CanEditMultipleObjects]
    public class ElementImageEditor : Editor
    {
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementImage)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);
        public override void OnInspectorGUI() => Builder.HandleUserChoice(RenderUserSelections(targets, ref isIDOptionsExpanded), targets, GetType());
    }

    [CustomEditor(typeof(ElementToggle), true)]
    [CanEditMultipleObjects]
    public class ElementToggleEditor : Editor
    {
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementToggle)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);
        public override void OnInspectorGUI() => Builder.HandleUserChoice(RenderUserSelections(targets, ref isIDOptionsExpanded), targets, GetType());
    }

    [CustomEditor(typeof(ElementDragDrop), true)]
    [CanEditMultipleObjects]
    public class ElementDragDropEditor : Editor
    {
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementDragDrop)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);
        public override void OnInspectorGUI() => Builder.HandleUserChoice(RenderUserSelections(targets, ref isIDOptionsExpanded), targets, GetType());
    }

    [CustomEditor(typeof(ElementText), true)]
    [CanEditMultipleObjects]
    public class ElementTextEditor : Editor
    {
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementText)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);
        public override void OnInspectorGUI() => Builder.HandleUserChoice(RenderUserSelections(targets, ref isIDOptionsExpanded), targets, GetType());
    }

    [CustomEditor(typeof(ElementInputField), true)]
    [CanEditMultipleObjects]
    public class ElementInputFieldEditor : Editor
    {
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementInputField)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);
        public override void OnInspectorGUI() => Builder.HandleUserChoice(RenderUserSelections(targets, ref isIDOptionsExpanded), targets, GetType());
    }

    [CustomEditor(typeof(ElementSlider), true)]
    [CanEditMultipleObjects]
    public class ElementSliderEditor : Editor
    {
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementSlider)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);
        public override void OnInspectorGUI() => Builder.HandleUserChoice(RenderUserSelections(targets, ref isIDOptionsExpanded), targets, GetType());
    }

    [CustomEditor(typeof(ElementDropdown), true)]
    [CanEditMultipleObjects]
    public class ElementDropdownEditor : Editor
    {
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementDropdown)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);
        public override void OnInspectorGUI() => Builder.HandleUserChoice(RenderUserSelections(targets, ref isIDOptionsExpanded), targets, GetType());
    }

    [CustomEditor(typeof(ElementCanvasGroup), true)]
    [CanEditMultipleObjects]
    public class ElementCanvasGroupEditor : Editor
    {
        private bool isChildrenExpanded = true;
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementCanvasGroup)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);

        public override void OnInspectorGUI()
        {
            var choice = RenderCanvasGroupUserSelections(targets, ref isIDOptionsExpanded, ref isChildrenExpanded);
            Builder.HandleUserChoice(choice, targets, GetType());
            // Falls out and handles additional CanvasGroup only choices here (yea got lazy)
            switch (choice) {
                case Choice.FileTemplate: CreateFileTemplate(); break;
                case Choice.ElementTemplate: CreateTemplate(false); break;
                case Choice.ForLoopTemplate: CreateTemplate(true); break;
            }
        }

        private void CreateTemplate(bool isForLoop)
        {
            var element = (ElementCanvasGroup)target;
            ImmediateUITemplate.BuildParams buildParams = default;
            buildParams.RootCanvasGroup.GameObject_Name = element.name;
            buildParams.RootCanvasGroup.Element_ID = element.ElementData.ID;
            buildParams.RootMapping_ID = element.GetComponent<RootMapping>() != null ? element.GetComponent<RootMapping>().ID : string.Empty;
            buildParams.ForLoop = isForLoop;
            {
                var elements = element.GetComponentsInChildren<ElementText>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Texts = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementImage>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Images = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementCanvasGroup>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();

                var items = new HashSet<ElementCanvasGroup>(elements);
                items.Remove(element);
                elements = items.ToArray();

                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.CanvasGroups = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementButton>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Buttons = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementToggle>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Toggles = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementSlider>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Sliders = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementDragDrop>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.DragDrops = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementDropdown>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Dropdowns = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementInputField>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.InputFields = elementInfo.ToArray();
            }

            var code = ImmediateUITemplate.BuildString(buildParams, ImmediateUITemplate.Name.ElementsExtension);
            code = ImmediateUITemplate.RemoveExtraNewLines(code);
            Helper.ClipboardText = code;
        }

        private void CreateFileTemplate()
        {
            var element = (ElementCanvasGroup)target;
            ImmediateUITemplate.BuildParams buildParams = default;
            buildParams.RootCanvasGroup.GameObject_Name = element.name;
            buildParams.RootCanvasGroup.Element_ID = element.ElementData.ID;
            buildParams.RootMapping_ID = element.GetComponent<RootMapping>() != null ? element.GetComponent<RootMapping>().ID : string.Empty;
            {
                var elements = element.GetComponentsInChildren<ElementText>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Texts = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementImage>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Images = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementCanvasGroup>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();

                var items = new HashSet<ElementCanvasGroup>(elements);
                items.Remove(element);
                elements = items.ToArray();

                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.CanvasGroups = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementButton>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Buttons = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementToggle>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Toggles = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementSlider>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Sliders = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementDragDrop>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.DragDrops = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementDropdown>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Dropdowns = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementInputField>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.InputFields = elementInfo.ToArray();
            }

            ImmediateUITemplate.Build(buildParams);
        }
    }


    [CustomEditor(typeof(ElementCanvas), true)]
    [CanEditMultipleObjects]
    public class ElementCanvasEditor : Editor
    {
        private bool isChildrenExpanded = true;
        private bool isIDOptionsExpanded = false;
        private GameObject m_ThisObjRef;
        private void OnEnable() => m_ThisObjRef = ((ElementCanvas)target).gameObject;
        private void OnDestroy() => ElementDestroy(m_ThisObjRef, target);

        public override void OnInspectorGUI()
        {
            var choice = RenderCanvas_JustCanvas_UserSelections(ref isIDOptionsExpanded, ref isChildrenExpanded);
            Builder.HandleUserChoice(choice, targets, GetType());
            // Falls out and handles additional CanvasGroup only choices here (yea got lazy)
            switch (choice) {
                case Choice.FileTemplate: CreateFileTemplate(); break;
                case Choice.ElementTemplate: CreateTemplate(false); break;
                case Choice.ForLoopTemplate: CreateTemplate(true); break;
            }
        }

        private void CreateTemplate(bool isForLoop)
        {
            var element = (ElementCanvas)target;
            ImmediateUITemplate.BuildParams buildParams = default;
            buildParams.RootCanvasGroup.GameObject_Name = element.name;
            buildParams.RootCanvasGroup.Element_ID = element.ElementData.ID;
            buildParams.RootMapping_ID = element.GetComponent<RootMapping>() != null ? element.GetComponent<RootMapping>().ID : string.Empty;
            buildParams.ForLoop = isForLoop;
            {
                var elements = element.GetComponentsInChildren<ElementText>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Texts = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementImage>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Images = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementCanvasGroup>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.CanvasGroups = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementButton>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Buttons = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementToggle>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Toggles = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementSlider>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Sliders = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementDragDrop>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.DragDrops = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementDropdown>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Dropdowns = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementInputField>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.InputFields = elementInfo.ToArray();
            }

            var code = ImmediateUITemplate.BuildString(buildParams, ImmediateUITemplate.Name.ElementsExtension);
            code = ImmediateUITemplate.RemoveExtraNewLines(code);
            Helper.ClipboardText = code;
        }

        private void CreateFileTemplate()
        {
            var element = (ElementCanvasGroup)target;
            ImmediateUITemplate.BuildParams buildParams = default;
            buildParams.RootCanvasGroup.GameObject_Name = element.name;
            buildParams.RootCanvasGroup.Element_ID = element.ElementData.ID;
            buildParams.RootMapping_ID = element.GetComponent<RootMapping>() != null ? element.GetComponent<RootMapping>().ID : string.Empty;
            {
                var elements = element.GetComponentsInChildren<ElementText>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Texts = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementImage>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Images = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementCanvasGroup>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();

                var items = new HashSet<ElementCanvasGroup>(elements);
                items.Remove(element);
                elements = items.ToArray();

                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.CanvasGroups = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementButton>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Buttons = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementToggle>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Toggles = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementSlider>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Sliders = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementDragDrop>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.DragDrops = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementDropdown>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.Dropdowns = elementInfo.ToArray();
            }
            {
                var elements = element.GetComponentsInChildren<ElementInputField>();
                var elementInfo = new List<ImmediateUITemplate.ElementInfo>();
                foreach (var t in elements) {
                    elementInfo.Add(new ImmediateUITemplate.ElementInfo { GameObject_Name = t.name, Element_ID = t.ElementData.ID });
                }
                buildParams.InputFields = elementInfo.ToArray();
            }

            ImmediateUITemplate.Build(buildParams);
        }
    }

    [CustomEditor(typeof(Reference), true)]
    [CanEditMultipleObjects]
    public class ReferenceEditor : Editor
    {
        private bool isIDOptionsExpanded = false;
        public override void OnInspectorGUI() => Builder.HandleUserChoice(RenderUserSelections(targets, ref isIDOptionsExpanded), targets, GetType());
    }

    public enum RootMappingChoice { UseRandomForID = 1, UseGameObjectNameForID, UseSiblingIndexForID }
    [CustomEditor(typeof(RootMapping), true)]
    [CanEditMultipleObjects]
    public class RootMappingEditor : Editor
    {
        private string prefix_go, prefix_si;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RootMappingChoice choice = default;
            if (GUILayout.Button("Use Random for ID")) choice = RootMappingChoice.UseRandomForID;
            using (new EditorGUILayout.HorizontalScope())
            using (new LabelWidthScope(40)) {
                prefix_go = EditorGUILayout.TextField("Prefix", prefix_go);

                if (GUILayout.Button("Use GameObject Name for ID")) choice = RootMappingChoice.UseGameObjectNameForID;
            }
            using (new EditorGUILayout.HorizontalScope())
            using (new LabelWidthScope(40)) {
                prefix_si = EditorGUILayout.TextField("Prefix", prefix_si);

                if (GUILayout.Button("Use Sibling Index for ID")) choice = RootMappingChoice.UseSiblingIndexForID;
            }

            foreach (var t in targets) {
                var element = (RootMapping)t;
                if (choice == RootMappingChoice.UseRandomForID) { element.Reset(); }
                if (choice == RootMappingChoice.UseGameObjectNameForID) { element.ID = $"{prefix_go}{t.name}"; }
                if (choice == RootMappingChoice.UseSiblingIndexForID) { element.ID = $"{prefix_si}{element.transform.GetSiblingIndex()}"; }

                if (choice != default) {
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }
}
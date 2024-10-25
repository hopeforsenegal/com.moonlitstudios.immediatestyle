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

namespace MoonlitSystem.Editors
{
    public static class Builder
    {
        public static string BuildConstantStatements(Object[] targets)
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

        public static string RemoveChars(this string s, IEnumerable<char> separators)
        {
            var sb = new StringBuilder(s);
            foreach (var c in separators) { sb.Replace(c.ToString(), ""); }
            return sb.ToString();
        }
        private static string CleanString(string str)
        {
            return str.RemoveChars(new[] { '/', '(', ')', ' ' });
        }
        public static (string, string) BuildConstantStatement(string id)
        {
            return ($"const string {CleanString(id)} = \"{id}\";", CleanString(id));
        }
        public static string BuildButtonStatement(string constant)
        {
            return $"ImmediateStyle.Button({constant});";
        }
        public static string BuildCanvasGroupStatement(string constant)
        {
            return $"ImmediateStyle.CanvasGroup({constant});";
        }
        public static string BuildImageStatement(string constant)
        {
            return $"ImmediateStyle.Image({constant}, null);";
        }
        public static string BuildToggleStatement(string constant)
        {
            return $"ImmediateStyle.Toggle({constant}, false);";
        }
        public static string BuildSwappableStatement(string constant)
        {
            return $"ImmediateStyle.Swappable({constant});";
        }
        public static string BuildDragDropStatement(string constant)
        {
            return $"ImmediateStyle.DragDrop({constant}, out _);";
        }
        public static string BuildInputFieldStatement(string constant)
        {
            return $"ImmediateStyle.InputField({constant}, new []{{ KeyCode.Return, KeyCode.KeypadEnter }}, ref text);";
        }
        public static string BuildTextStatement(string constant)
        {
            return $"ImmediateStyle.Text({constant}, \"Placeholder\");";
        }
        public static string BuildSliderStatement(string constant)
        {
            return $"ImmediateStyle.Slider({constant});";
        }
        public static string BuildDropdownStatement(string constant)
        {
            return $"ImmediateStyle.Dropdown({constant}, new UnityEngine.UI.Dropdown.OptionData[] {{ new(\"Thing\")}});";
        }
        public static string BuildReferenceStatement(string constant, string theType = "")
        {
            return $"Reference.Find<{theType}>(this, {constant});";
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

        public static Choice RenderButtons()
        {
            if (GUILayout.Button("Copy ID")) return Choice.CopyID;
            if (GUILayout.Button("Regenerate ID")) return Choice.RegenerateRandomID;
            if (GUILayout.Button("Use Game Object Name as ID")) return Choice.UseGameObjectNameID;
            EditorGUILayout.Space();
            if (GUILayout.Button("Clipboard Code Snippet")) return Choice.CopyCode;
            return default;
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
        private GameObject m_ThisObjRef;
        private void OnEnable()
        {
            m_ThisObjRef = ((ElementButton)target).gameObject;
        }

        private void OnDestroy()
        {
            UI.ElementDestroy(m_ThisObjRef, target);
        }

        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            var choice = RenderButtons();
            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var constantStatements = string.Empty;
                var elementStatements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (ElementButton)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (!settings.inlineClipboardGUIDS) {
                        var elementStatement = Builder.BuildButtonStatement(constant);
                        constantStatements += constantStatement + Environment.NewLine;
                        elementStatements += elementStatement + Environment.NewLine;
                    } else {
                        var elementStatement = Builder.BuildButtonStatement($"\"{id}\"");
                        elementStatements += elementStatement + Environment.NewLine;
                    }
                }
                Helper.ClipboardText = constantStatements + elementStatements;
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (ElementButton)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            } else if (choice == Choice.UseGameObjectNameID) {
                foreach (var t in targets) {
                    var element = (ElementButton)t;
                    ElementData.SetupElementDataGameObjectName(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            }
        }

    }

    [CustomEditor(typeof(ElementCanvasGroup), true)]
    [CanEditMultipleObjects]
    public class ElementCanvasGroupEditor : Editor
    {
        private GameObject m_ThisObjRef;
        private void OnEnable()
        {
            m_ThisObjRef = ((ElementCanvasGroup)target).gameObject;
        }

        private void OnDestroy()
        {
            UI.ElementDestroy(m_ThisObjRef, target);
        }

        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            Choice choice = default;
            if (GUILayout.Button("Copy ID")) choice = Choice.CopyID;
            if (GUILayout.Button("Regenerate ID")) choice = Choice.RegenerateRandomID;
            EditorGUILayout.Space();
            if (GUILayout.Button("Clipboard Code Snippet")) choice = Choice.CopyCode;
            GUILayout.Label("w/ Children");
            using (new HorizontalScope()) {
                if (GUILayout.Button("Code For-Loop")) choice = Choice.ForLoopTemplate;
                if (GUILayout.Button("Code Individual")) choice = Choice.ElementTemplate;
            }
            if (GUILayout.Button("Create Full Code File")) choice = Choice.FileTemplate;

            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var constantStatements = string.Empty;
                var elementStatements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (ElementCanvasGroup)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (!settings.inlineClipboardGUIDS) {
                        var elementStatement = Builder.BuildButtonStatement(constant);
                        constantStatements += constantStatement + Environment.NewLine;
                        elementStatements += elementStatement + Environment.NewLine;
                    } else {
                        var elementStatement = Builder.BuildButtonStatement($"\"{id}\"");
                        elementStatements += elementStatement + Environment.NewLine;
                    }
                }
                Helper.ClipboardText = constantStatements + elementStatements;
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (ElementCanvasGroup)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            } else if (choice == Choice.FileTemplate) {
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
            } else if (choice == Choice.ElementTemplate) {
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

                var code = ImmediateUITemplate.BuildString(buildParams, ImmediateUITemplate.Name.ElementsExtension);
                Helper.ClipboardText = code;
            } else if (choice == Choice.ForLoopTemplate) {
                var element = (ElementCanvasGroup)target;
                ImmediateUITemplate.BuildParams buildParams;
                buildParams.RootCanvasGroup.GameObject_Name = element.name;
                buildParams.RootCanvasGroup.Element_ID = element.ElementData.ID;
                buildParams.RootMapping_ID = element.GetComponent<RootMapping>() != null ? element.GetComponent<RootMapping>().ID : string.Empty;
                buildParams.ForLoop = true;
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
                Helper.ClipboardText = code;
            }
        }
    }

    [CustomEditor(typeof(ElementImage), true)]
    [CanEditMultipleObjects]
    public class ElementImageEditor : Editor
    {
        private GameObject m_ThisObjRef;
        private void OnEnable()
        {
            m_ThisObjRef = ((ElementImage)target).gameObject;
        }

        private void OnDestroy()
        {
            UI.ElementDestroy(m_ThisObjRef, target);
        }

        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            var choice = RenderButtons();
            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var constantStatements = string.Empty;
                var elementStatements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (ElementImage)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (!settings.inlineClipboardGUIDS) {
                        var elementStatement = Builder.BuildButtonStatement(constant);
                        constantStatements += constantStatement + Environment.NewLine;
                        elementStatements += elementStatement + Environment.NewLine;
                    } else {
                        var elementStatement = Builder.BuildButtonStatement($"\"{id}\"");
                        elementStatements += elementStatement + Environment.NewLine;
                    }
                }
                Helper.ClipboardText = constantStatements + elementStatements;
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (ElementImage)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            } else if (choice == Choice.UseGameObjectNameID) {
                foreach (var t in targets) {
                    var element = (ElementImage)t;
                    ElementData.SetupElementDataGameObjectName(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }

    [CustomEditor(typeof(ElementToggle), true)]
    [CanEditMultipleObjects]
    public class ElementToggleEditor : Editor
    {
        private GameObject m_ThisObjRef;
        private void OnEnable()
        {
            m_ThisObjRef = ((ElementToggle)target).gameObject;
        }

        private void OnDestroy()
        {
            UI.ElementDestroy(m_ThisObjRef, target);
        }

        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            var choice = RenderButtons();
            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var constantStatements = string.Empty;
                var elementStatements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (ElementToggle)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (!settings.inlineClipboardGUIDS) {
                        var elementStatement = Builder.BuildButtonStatement(constant);
                        constantStatements += constantStatement + Environment.NewLine;
                        elementStatements += elementStatement + Environment.NewLine;
                    } else {
                        var elementStatement = Builder.BuildButtonStatement($"\"{id}\"");
                        elementStatements += elementStatement + Environment.NewLine;
                    }
                }
                Helper.ClipboardText = constantStatements + elementStatements;
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (ElementToggle)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            } else if (choice == Choice.UseGameObjectNameID) {
                foreach (var t in targets) {
                    var element = (ElementToggle)t;
                    ElementData.SetupElementDataGameObjectName(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }

    [CustomEditor(typeof(ElementDragDrop), true)]
    [CanEditMultipleObjects]
    public class ElementDragDropEditor : Editor
    {
        private GameObject m_ThisObjRef;
        private void OnEnable()
        {
            m_ThisObjRef = ((ElementDragDrop)target).gameObject;
        }

        private void OnDestroy()
        {
            UI.ElementDestroy(m_ThisObjRef, target);
        }

        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            var choice = RenderButtons();
            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var constantStatements = string.Empty;
                var elementStatements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (ElementDragDrop)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (!settings.inlineClipboardGUIDS) {
                        var elementStatement = Builder.BuildButtonStatement(constant);
                        constantStatements += constantStatement + Environment.NewLine;
                        elementStatements += elementStatement + Environment.NewLine;
                    } else {
                        var elementStatement = Builder.BuildButtonStatement($"\"{id}\"");
                        elementStatements += elementStatement + Environment.NewLine;
                    }
                }
                Helper.ClipboardText = constantStatements + elementStatements;
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (ElementDragDrop)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            } else if (choice == Choice.UseGameObjectNameID) {
                foreach (var t in targets) {
                    var element = (ElementDragDrop)t;
                    ElementData.SetupElementDataGameObjectName(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }

    [CustomEditor(typeof(ElementText), true)]
    [CanEditMultipleObjects]
    public class ElementTextEditor : Editor
    {
        private GameObject m_ThisObjRef;
        private void OnEnable()
        {
            m_ThisObjRef = ((ElementText)target).gameObject;
        }

        private void OnDestroy()
        {
            UI.ElementDestroy(m_ThisObjRef, target);
        }

        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            var choice = RenderButtons();
            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var constantStatements = string.Empty;
                var elementStatements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (ElementText)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (!settings.inlineClipboardGUIDS) {
                        var elementStatement = Builder.BuildButtonStatement(constant);
                        constantStatements += constantStatement + Environment.NewLine;
                        elementStatements += elementStatement + Environment.NewLine;
                    } else {
                        var elementStatement = Builder.BuildButtonStatement($"\"{id}\"");
                        elementStatements += elementStatement + Environment.NewLine;
                    }
                }
                Helper.ClipboardText = constantStatements + elementStatements;
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (ElementText)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            } else if (choice == Choice.UseGameObjectNameID) {
                foreach (var t in targets) {
                    var element = (ElementText)t;
                    ElementData.SetupElementDataGameObjectName(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }


    [CustomEditor(typeof(ElementInputField), true)]
    [CanEditMultipleObjects]
    public class ElementInputFieldEditor : Editor
    {
        private GameObject m_ThisObjRef;
        private void OnEnable()
        {
            m_ThisObjRef = ((ElementInputField)target).gameObject;
        }

        private void OnDestroy()
        {
            UI.ElementDestroy(m_ThisObjRef, target);
        }

        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            var choice = RenderButtons();
            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var constantStatements = string.Empty;
                var elementStatements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (ElementInputField)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (!settings.inlineClipboardGUIDS) {
                        var elementStatement = Builder.BuildButtonStatement(constant);
                        constantStatements += constantStatement + Environment.NewLine;
                        elementStatements += elementStatement + Environment.NewLine;
                    } else {
                        var elementStatement = Builder.BuildButtonStatement($"\"{id}\"");
                        elementStatements += elementStatement + Environment.NewLine;
                    }
                }
                Helper.ClipboardText = constantStatements + elementStatements;
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (ElementInputField)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            } else if (choice == Choice.UseGameObjectNameID) {
                foreach (var t in targets) {
                    var element = (ElementInputField)t;
                    ElementData.SetupElementDataGameObjectName(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }

    [CustomEditor(typeof(ElementSlider), true)]
    [CanEditMultipleObjects]
    public class ElementSliderEditor : Editor
    {
        private GameObject m_ThisObjRef;
        private void OnEnable()
        {
            m_ThisObjRef = ((ElementSlider)target).gameObject;
        }

        private void OnDestroy()
        {
            UI.ElementDestroy(m_ThisObjRef, target);
        }

        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            var choice = RenderButtons();
            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var constantStatements = string.Empty;
                var elementStatements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (ElementSlider)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (!settings.inlineClipboardGUIDS) {
                        var elementStatement = Builder.BuildButtonStatement(constant);
                        constantStatements += constantStatement + Environment.NewLine;
                        elementStatements += elementStatement + Environment.NewLine;
                    } else {
                        var elementStatement = Builder.BuildButtonStatement($"\"{id}\"");
                        elementStatements += elementStatement + Environment.NewLine;
                    }
                }
                Helper.ClipboardText = constantStatements + elementStatements;
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (ElementSlider)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            } else if (choice == Choice.UseGameObjectNameID) {
                foreach (var t in targets) {
                    var element = (ElementSlider)t;
                    ElementData.SetupElementDataGameObjectName(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }


    [CustomEditor(typeof(ElementDropdown), true)]
    [CanEditMultipleObjects]
    public class ElementDropdownEditor : Editor
    {
        private GameObject m_ThisObjRef;
        private void OnEnable()
        {
            m_ThisObjRef = ((ElementDropdown)target).gameObject;
        }

        private void OnDestroy()
        {
            UI.ElementDestroy(m_ThisObjRef, target);
        }

        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            var choice = RenderButtons();
            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var constantStatements = string.Empty;
                var elementStatements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (ElementDropdown)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (!settings.inlineClipboardGUIDS) {
                        var elementStatement = Builder.BuildButtonStatement(constant);
                        constantStatements += constantStatement + Environment.NewLine;
                        elementStatements += elementStatement + Environment.NewLine;
                    } else {
                        var elementStatement = Builder.BuildButtonStatement($"\"{id}\"");
                        elementStatements += elementStatement + Environment.NewLine;
                    }
                }
                Helper.ClipboardText = constantStatements + elementStatements;
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (ElementDropdown)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            } else if (choice == Choice.UseGameObjectNameID) {
                foreach (var t in targets) {
                    var element = (ElementDropdown)t;
                    ElementData.SetupElementDataGameObjectName(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }


    [CustomEditor(typeof(Reference), true)]
    [CanEditMultipleObjects]
    public class ReferenceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ElementDataEditor.Render(targets);

            var choice = RenderButtons();
            if (choice == Choice.CopyID) {
                Helper.ClipboardText = Builder.BuildConstantStatements(targets);
            } else if (choice == Choice.CopyCode) {
                var statements = string.Empty;
                var settings = ImmediateStyleSettings.LoadInstance();
                foreach (var t in targets) {
                    var element = (Reference)t;
                    var id = element.ElementData.ID;
                    var (constantStatement, constant) = Builder.BuildConstantStatement(id);
                    if (settings.inlineClipboardGUIDS) {
                        constantStatement = string.Empty;
                        constant = $"\"{id}\"";
                    }
                    if (element.GetComponent<CanvasGroup>() != null) {
                        var elementStatement = Builder.BuildReferenceStatement(constant, nameof(CanvasGroup));
                        statements += constantStatement + Environment.NewLine + elementStatement;
                    } else if (element.GetComponent<Button>() != null) {
                        var elementStatement = Builder.BuildReferenceStatement(constant, nameof(Button));
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
                        var elementStatement = Builder.BuildReferenceStatement(constant, components[result].GetType().Name);
                        statements += constantStatement + Environment.NewLine + elementStatement;
                    }
                    Helper.ClipboardText = statements + Environment.NewLine;
                }
            } else if (choice == Choice.RegenerateRandomID) {
                foreach (var t in targets) {
                    var element = (Reference)t;
                    ElementData.SetupElementData(element.ElementData, element.transform);
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }

    public enum RootMappingChoice { UseRandomForID = 1, UseGameObjectNameforID, UseSiblingIndexforID }
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
            using (new HorizontalScope())
            using (new LabelWidthScope(40)) {
                prefix_go = EditorGUILayout.TextField("Prefix", prefix_go);

                if (GUILayout.Button("Use GameObject Name for ID")) choice = RootMappingChoice.UseGameObjectNameforID;
            }
            using (new HorizontalScope())
            using (new LabelWidthScope(40)) {
                prefix_si = EditorGUILayout.TextField("Prefix", prefix_si);

                if (GUILayout.Button("Use Sibling Index for ID")) choice = RootMappingChoice.UseSiblingIndexforID;
            }

            foreach (var t in targets) {
                var element = (RootMapping)t;
                if (choice == RootMappingChoice.UseRandomForID) { element.Reset(); }
                if (choice == RootMappingChoice.UseGameObjectNameforID) { element.ID = $"{prefix_go}{t.name}"; }
                if (choice == RootMappingChoice.UseSiblingIndexforID) { element.ID = $"{prefix_si}{element.transform.GetSiblingIndex()}"; }

                if (choice != default) {
                    EditorUtility.SetDirty(element);
                }
            }
        }
    }
}
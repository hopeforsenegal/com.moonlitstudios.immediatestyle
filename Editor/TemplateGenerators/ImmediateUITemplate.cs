using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MoonlitSystem.TemplateGenerators
{
    public static class ImmediateUITemplate
    {
        private static string ToTitleCase(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (str.Length <= 1) return str;
            return char.ToUpperInvariant(str[0]) + str.Substring(1);
        }

        private static string RemoveChars(this string s, IEnumerable<char> separators)
        {
            var sb = new StringBuilder(s);
            foreach (var c in separators) { sb.Replace(c.ToString(), ""); }
            return sb.ToString();
        }

        public static class Name
        {
            private const string PackageName = "com.moonlitstudios.immediatestyle";
            private static string TemplatePackagePath { get; } = $"Packages/{PackageName}/Editor/Template";
            internal const string ScreenExtension = "ImmediateUI";
            internal const string ElementsExtension = "ImmediateUIElement";
            internal const string AssetsPath = "Assets";

            public static string GetTemplatePackagePath(string filename) => Path.GetFullPath($"{TemplatePackagePath}/{filename}");
        }

        private static class Builder
        {
            public static string CleanString(string str)
            {
                return str.RemoveChars(new[] { '-', '<', '/', '(', ')', ' ' });
            }
            public static string NameWithoutTypeOtherwiseJustType(string str, string type)
            {
                var gameObjectName = str.Replace(type, "");
                gameObjectName = CleanString(gameObjectName);
                if (string.IsNullOrWhiteSpace(gameObjectName)) gameObjectName = type;
                if (int.TryParse(gameObjectName, out var number)) gameObjectName = $"{type}{number}";
                return gameObjectName;
            }
            public static string BuildConstantStatement(string extra, string id)
            {
                var settings = ImmediateStyleSettings.LoadInstance(); // load instance again for the 15th time :-/
                if (settings.inlineClipboardGUIDS) return extra + $"\"{id}\"";
                return CleanString(extra + id);
            }
            public static string BuildPrivateConstantStatement(string id)
            {
                return $"private const string {CleanString(id)} = \"{id}\";";
            }
            public static string BuildNonPrivateConstantStatement(string id)
            {
                return $"const string {CleanString(id)} = \"{id}\";";
            }
            public static string BuildButtonStatement(string constant)
            {
                return $"ImmediateStyle.Button({constant});";
            }
            public static string BuildToggleStatement(string constant)
            {
                return $"ImmediateStyle.Toggle({constant}, false);";
            }
            public static string BuildCanvasGroupStatement(string constant)
            {
                return $"ImmediateStyle.CanvasGroup({constant});";
            }
            public static string BuildImageStatement(string constant)
            {
                return $"ImmediateStyle.Image({constant}, null);";
            }
            public static string BuildTextStatement(string constant)
            {
                return $"ImmediateStyle.Text({constant}, \"Placeholder\");";    // Use placeholder so we can see the text right away
            }
            internal static string BuildSliderStatement(string constant)
            {
                return $"ImmediateStyle.Slider({constant});";
            }
            internal static object BuildDragDropStatement(string constant)
            {
                return @$"ImmediateStyle.DragDrop({constant}, out var component);
if(component.IsDragging) ImmediateStyle.FollowCursor(component.transform);
";
            }
            internal static object BuildDropdownStatement(string constant)
            {
                return $"ImmediateStyle.Dropdown({constant}, new[] {{ new UnityEngine.UI.Dropdown.OptionData(\"\")}});";
            }
            internal static string BuildInputFieldStatement(string constant)
            {
                return $"ImmediateStyle.InputField({constant}, new[] {{KeyCode.Return, KeyCode.KeypadEnter}}, ref text);";
            }
            public static string BuildEvent(string eventName)
            {
                return $"internal bool {eventName.ToTitleCase()};";
            }
            public static string BuildButtonReadEvent(string name, string buttonID, string eventName)
            {
                var constant = BuildConstantStatement(string.Empty, buttonID);
                var buttonStatement = BuildButtonStatement(constant);
                buttonStatement = buttonStatement.Replace(";", "");
                name = CleanString(name);
                eventName = CleanString(eventName);
                return $"m_Events.{name}.{eventName} = {buttonStatement}.IsMouseDown || m_Events.{name}.{eventName};";
            }
            public static string BuildToggleReadEvent(string name, string buttonID, string eventName)
            {
                var constant = BuildConstantStatement(string.Empty, buttonID);
                var buttonStatement = BuildToggleStatement(constant);
                buttonStatement = buttonStatement.Replace(";", "");
                name = CleanString(name);
                eventName = CleanString(eventName);
                return $"m_Events.{name}.{eventName} = {buttonStatement}.IsClicked || m_Events.{name}.{eventName};";
            }
            internal static string BuildInputFieldsReadEvent(string name, string buttonID, string eventName)
            {
                var constant = BuildConstantStatement(string.Empty, buttonID);
                var statement = BuildInputFieldStatement(constant);
                statement = statement.Replace(";", "");
                name = CleanString(name);
                eventName = CleanString(eventName);
                return $"m_Events.{name}.{eventName} = {statement}.HasSubmitted || m_Events.{name}.{eventName};";
            }
            public static string BuildUseEvent(string name, string eventName)
            {
                name = CleanString(name);
                eventName = CleanString(eventName);
                return $@"        if (m_Events.{name}.{eventName}) {{
        }}";
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct ElementInfo
        {
            public string GameObject_Name, Element_ID;
        }

        public struct BuildParams
        {
            public string RootMapping_ID;
            public ElementInfo RootCanvasGroup;
            public ElementInfo[] Buttons;
            public ElementInfo[] Toggles;
            public ElementInfo[] Texts;
            public ElementInfo[] Images;
            public ElementInfo[] Sliders;
            public ElementInfo[] InputFields;
            public ElementInfo[] DragDrops;
            public ElementInfo[] Dropdowns;
            public ElementInfo[] CanvasGroups;
            internal bool ForLoop;
        }

        public static string RemoveExtraNewLines(string code)
        {
            var codeSplit = code.Split(Environment.NewLine);
            var trimmedEndingSpaces = string.Empty;
            foreach (var line in codeSplit) trimmedEndingSpaces += line.TrimEnd(' ') + Environment.NewLine;
            code = trimmedEndingSpaces;
            code = code.Replace("\r\n", "\n").Replace("\r", "\n");
            while (code.Contains("\n\n")) code = code.Replace("\n\n", "\n");
            code = code.Replace("\n", Environment.NewLine);
            return code;
        }

        public static string BuildString(BuildParams build, string extension)
        {
            var templatePath = Name.GetTemplatePackagePath($"{extension}.cs.tmpl");
            var code = File.ReadAllText(templatePath);
            {
                code = code.Replace("{name}", Builder.CleanString(build.RootCanvasGroup.GameObject_Name));
            }
            {
                var constantIds = "";
                var constantIdsNoConst = "";
                var settings = ImmediateStyleSettings.LoadInstance(); // load instance again for the 15th time :-/
                var allData = new List<ElementInfo>();
                allData.AddRange(build.Buttons);
                allData.AddRange(build.Toggles);
                allData.AddRange(build.Texts);
                allData.AddRange(build.Images);
                allData.AddRange(build.Sliders);
                allData.AddRange(build.InputFields);
                allData.AddRange(build.DragDrops);
                allData.AddRange(build.Dropdowns);
                allData.AddRange(build.CanvasGroups);
                allData.Add(build.RootCanvasGroup);

                if (!settings.inlineClipboardGUIDS) {
                    foreach (var elementInfo in allData) {
                        var constantStatement = Builder.BuildPrivateConstantStatement(elementInfo.Element_ID);
                        constantIds += constantStatement + Environment.NewLine;
                    }
                    foreach (var elementInfo in allData) {
                        var constantStatement = Builder.BuildNonPrivateConstantStatement(elementInfo.Element_ID);
                        constantIdsNoConst += constantStatement + Environment.NewLine;
                    }
                }
                code = code.Replace("{id_names_and_values}", constantIds);
                code = code.Replace("{id_names_and_values_no_const}", constantIdsNoConst);
            }
            {
                var buttonNames = "";
                buttonNames += Builder.BuildEvent("Open") + Environment.NewLine;
                buttonNames += Builder.BuildEvent("Close") + Environment.NewLine;
                foreach (var button in build.Buttons) {
                    var gameObjectName = Builder.NameWithoutTypeOtherwiseJustType(button.GameObject_Name, "Button");
                    buttonNames += Builder.BuildEvent(gameObjectName) + Environment.NewLine;
                }
                code = code.Replace("{button_events}", buttonNames);

                var toggleNames = "";
                foreach (var toggle in build.Toggles) {
                    var gameObjectName = Builder.NameWithoutTypeOtherwiseJustType(toggle.GameObject_Name, "Toggle");
                    toggleNames += Builder.BuildEvent(gameObjectName) + Environment.NewLine;
                }
                code = code.Replace("{toggle_events}", toggleNames);

                var inputFieldNames = "";
                foreach (var inputField in build.InputFields) {
                    var gameObjectName = Builder.NameWithoutTypeOtherwiseJustType(inputField.GameObject_Name, "InputField");
                    inputFieldNames += Builder.BuildEvent(gameObjectName) + Environment.NewLine;
                }
                code = code.Replace("{inputfield_events}", inputFieldNames);
            }

            if (build.ForLoop) {
                code = code.Replace("{forloopstart}", "for (int i = 0; i < max; i++) {");
                code = code.Replace("{forloopend}", "}");
            } else {
                code = code.Replace("{forloopstart}", string.Empty);
                code = code.Replace("{forloopend}", string.Empty);
            }

            var extraConstant = "";
            if (!string.IsNullOrWhiteSpace(build.RootMapping_ID)) {
                var elementRootMappingID = build.RootMapping_ID;
                while (elementRootMappingID.Length > 0 && char.IsDigit(elementRootMappingID[elementRootMappingID.Length - 1])) {
                    elementRootMappingID = elementRootMappingID.Remove(elementRootMappingID.Length - 1);
                }
                code = code.Replace("{has_root_mapping}", $"const string elementrootmapping =  \"{elementRootMappingID}\";");

                extraConstant = "elementrootmapping + ";
                if (build.ForLoop) extraConstant += "i + ";
            } else {
                code = code.Replace("{has_root_mapping}", string.Empty);
            }

            {
                var constant = Builder.BuildConstantStatement(extraConstant, build.RootCanvasGroup.Element_ID);
                var elementStatement = Builder.BuildCanvasGroupStatement(constant);
                code = code.Replace("{root_canvas_group}", elementStatement + Environment.NewLine);
            }

            {
                var texts = "";
                foreach (var text in build.CanvasGroups) {
                    var constant = Builder.BuildConstantStatement(extraConstant, text.Element_ID);
                    var elementStatement = Builder.BuildCanvasGroupStatement(constant);
                    texts += elementStatement + Environment.NewLine;
                }
                code = code.Replace("{canvas_groups}", texts);
            }
            {
                var texts = "";
                foreach (var text in build.Texts) {
                    var constant = Builder.BuildConstantStatement(extraConstant, text.Element_ID);
                    var elementStatement = Builder.BuildTextStatement(constant);
                    texts += elementStatement + Environment.NewLine;
                }
                code = code.Replace("{texts}", texts);
            }
            {
                var images = "";
                foreach (var image in build.Images) {
                    var constant = Builder.BuildConstantStatement(extraConstant, image.Element_ID);
                    var elementStatement = Builder.BuildImageStatement(constant);
                    images += elementStatement + Environment.NewLine;
                }
                code = code.Replace("{images}", images);
            }
            {
                var buttons = "";
                foreach (var button in build.Buttons) {
                    var gameObjectName = Builder.NameWithoutTypeOtherwiseJustType(button.GameObject_Name, "Button");
                    buttons += Builder.BuildButtonReadEvent(Builder.CleanString(build.RootCanvasGroup.GameObject_Name), button.Element_ID, gameObjectName) + Environment.NewLine;
                }
                code = code.Replace("{buttons}", buttons);

                var buttonsNoEvent = "";
                foreach (var button in build.Buttons) {
                    var constant = Builder.BuildConstantStatement(extraConstant, button.Element_ID);
                    var buttonStatement = Builder.BuildButtonStatement(constant);
                    buttonStatement = buttonStatement.Replace(";", "");
                    buttonStatement = $"{ buttonStatement}.IsMouseDown;";

                    buttonsNoEvent += buttonStatement + Environment.NewLine;
                }
                code = code.Replace("{buttons_no_event}", buttonsNoEvent);
            }
            {
                var toggles = "";
                foreach (var toggle in build.Toggles) {
                    var gameObjectName = Builder.NameWithoutTypeOtherwiseJustType(toggle.GameObject_Name, "Toggle");
                    toggles += Builder.BuildToggleReadEvent(Builder.CleanString(build.RootCanvasGroup.GameObject_Name), toggle.Element_ID, gameObjectName) + Environment.NewLine;
                }
                code = code.Replace("{toggles}", toggles);

                var buttonsNoEvent = "";
                foreach (var button in build.Toggles) {
                    var constant = Builder.BuildConstantStatement(extraConstant, button.Element_ID);
                    var buttonStatement = Builder.BuildToggleStatement(constant);
                    buttonStatement = buttonStatement.Replace(";", "");
                    buttonStatement = $"{ buttonStatement}.IsClicked;";

                    buttonsNoEvent += buttonStatement + Environment.NewLine;
                }
                code = code.Replace("{toggles_no_event}", buttonsNoEvent);
            }
            {
                var components = "";
                foreach (var inputField in build.InputFields) {
                    var gameObjectName = Builder.NameWithoutTypeOtherwiseJustType(inputField.GameObject_Name, "InputField");
                    components += Builder.BuildInputFieldsReadEvent(Builder.CleanString(build.RootCanvasGroup.GameObject_Name), inputField.Element_ID, gameObjectName) + Environment.NewLine;
                }
                code = code.Replace("{inputfields}", components);

                var buttonsNoEvent = "";
                foreach (var button in build.InputFields) {
                    var constant = Builder.BuildConstantStatement(extraConstant, button.Element_ID);
                    var buttonStatement = Builder.BuildInputFieldStatement(constant);
                    buttonStatement = buttonStatement.Replace(";", "");
                    buttonStatement = $"{ buttonStatement}.HasSubmitted;";

                    buttonsNoEvent += buttonStatement + Environment.NewLine;
                }
                code = code.Replace("{inputfields_no_event}", buttonsNoEvent);
            }
            {
                var additionalEvents = "";
                foreach (var button in build.Buttons) {
                    if (button.GameObject_Name.Contains("Close")) continue;
                    additionalEvents += Builder.BuildUseEvent(Builder.CleanString(build.RootCanvasGroup.GameObject_Name), button.GameObject_Name.Replace("Button", ""));
                }
                foreach (var inputField in build.InputFields) {
                    var gameObjectName = Builder.NameWithoutTypeOtherwiseJustType(inputField.GameObject_Name, "InputField");
                    additionalEvents += Builder.BuildUseEvent(Builder.CleanString(build.RootCanvasGroup.GameObject_Name), gameObjectName);
                }
                code = code.Replace("{additional_events}", additionalEvents);
            }
            {
                var components = "";
                foreach (var text in build.Sliders) {
                    var constant = Builder.BuildConstantStatement(extraConstant, text.Element_ID);
                    var elementStatement = Builder.BuildSliderStatement(constant);
                    components += elementStatement + Environment.NewLine;
                }
                code = code.Replace("{sliders}", components);
            }
            {
                var components = "";
                foreach (var text in build.DragDrops) {
                    var constant = Builder.BuildConstantStatement(extraConstant, text.Element_ID);
                    var elementStatement = Builder.BuildDragDropStatement(constant);
                    components += elementStatement + Environment.NewLine;
                }
                code = code.Replace("{dragdrop}", components);
            }
            {
                var components = "";
                foreach (var text in build.Dropdowns) {
                    var constant = Builder.BuildConstantStatement(extraConstant, text.Element_ID);
                    var elementStatement = Builder.BuildDropdownStatement(constant);
                    components += elementStatement + Environment.NewLine;
                }
                code = code.Replace("{dropdown}", components);
            }

            return code;
        }

        public static void Build(BuildParams build)
        {
            var code = BuildString(build, Name.ScreenExtension);
            var path = Path.Combine(Application.dataPath, Name.AssetsPath);
            path = path.Substring(0, path.Length - Path.GetFileName(path).Length);
            path = path + Builder.CleanString(build.RootCanvasGroup.GameObject_Name) + ".cs";
            code = RemoveExtraNewLines(code);

            File.WriteAllText(path, code);
            AssetDatabase.Refresh();
        }
    }
}
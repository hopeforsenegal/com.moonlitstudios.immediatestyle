using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using MoonlitSystem.Strings;
using UnityEditor;
using UnityEngine;

namespace Editor.TemplateGenerators
{
    public static class ImmediateUITemplate
    {
        public static string ToTitleCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1) {
                return char.ToUpperInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }

        public static class Name
        {
            static readonly string packageName = "com.moonlitstudios.immediatestyle";
            private static string TemplatePackagePath => $"Packages/{packageName}/Editor/Template";
            internal const string ScreenExtension = "ImmediateUI";
            internal const string ElementsExtension = "ImmediateUIElement";
            internal const string AssetsPath = "Assets";

            public static string GetTemplatePackagePath(string filename) => Path.GetFullPath($"{TemplatePackagePath}/{filename}");
        }

        private static class Builder
        {
            private static string CleanString(string str)
            {
                return str.RemoveChars(new[] { '/', '(', ')', ' ' });
            }
            public static string BuildConstantStatement(string id)
            {
                return CleanString(id);
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
            public static string BuildEvent(string eventName)
            {
                return $"internal bool {eventName.ToTitleCase()};";
            }
            public static string BuildButtonReadEvent(string name, string buttonID, string eventName)
            {
                var constant = BuildConstantStatement(buttonID);
                var buttonStatement = BuildButtonStatement(constant);
                buttonStatement = buttonStatement.Replace(";", "");
                return $"m_Events.{name}.{eventName} = {buttonStatement}.IsMouseDown || m_Events.{name}.{eventName};";
            }
            public static string BuildToggleReadEvent(string name, string buttonID, string eventName)
            {
                var constant = BuildConstantStatement(buttonID);
                var buttonStatement = BuildToggleStatement(constant);
                buttonStatement = buttonStatement.Replace(";", "");
                return $"m_Events.{name}.{eventName} = {buttonStatement}.IsClicked || m_Events.{name}.{eventName};";
            }
            public static string BuildUseEvent(string name, string eventName)
            {
                return $@"        if (m_Events.{name}.{eventName}) {{
        }}";
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct ElementInfo
        {
            public string GameObject_Name;
            public string Element_ID;
        }

        public struct BuildParams
        {
            public string ElementRootMapping_ID;
            public ElementInfo RootCanvasGroup;
            public ElementInfo[] Buttons;
            public ElementInfo[] Toggles;
            public ElementInfo[] Texts;
            public ElementInfo[] Images;
            public ElementInfo[] CanvasGroups;
            internal bool ForLoop;

            // No DragDrops
            // We don't file or element create DragDrop because anything using it
            // typically does so in a foor loop with a root mapping. Meaning that it must be done seperately anyways.
            // we might handle this in the future the day we come up on this issue.
        }


        public static string BuildString(BuildParams build, string extension)
        {
            var templatePath = Name.GetTemplatePackagePath($"{extension}.cs.tmpl");
            var code = File.ReadAllText(templatePath);

            {
                code = code.Replace("{name}", build.RootCanvasGroup.GameObject_Name);
            }
            {
                var allData = new List<ElementInfo>();
                allData.AddRange(build.Buttons);
                allData.AddRange(build.Toggles);
                allData.AddRange(build.Texts);
                allData.AddRange(build.Images);
                allData.AddRange(build.CanvasGroups);
                allData.Add(build.RootCanvasGroup);

                var constantIds = "";
                foreach (var elementInfo in allData) {
                    var constantStatement = Builder.BuildPrivateConstantStatement(elementInfo.Element_ID);
                    constantIds += constantStatement + Environment.NewLine;
                }
                code = code.Replace("{id_names_and_values}", constantIds);
                var constantIdsNoConst = "";
                foreach (var elementInfo in allData) {
                    var constantStatement = Builder.BuildNonPrivateConstantStatement(elementInfo.Element_ID);
                    constantIdsNoConst += constantStatement + Environment.NewLine;
                }
                code = code.Replace("{id_names_and_values_no_const}", constantIdsNoConst);
            }
            {
                var buttonNames = "";
                buttonNames += Builder.BuildEvent("Open") + Environment.NewLine;
                foreach (var button in build.Buttons) {
                    buttonNames += Builder.BuildEvent(button.GameObject_Name.Replace("Button", "")) + Environment.NewLine;
                }
                code = code.Replace("{button_events}", buttonNames);
            }

            if (build.ForLoop) {
                code = code.Replace("{forloopstart}", "for (int i = 0; i < max; i++) {");
                code = code.Replace("{forloopend}", "}");
            } else {
                code = code.Replace("{forloopstart}", string.Empty);
                code = code.Replace("{forloopend}", string.Empty);
            }

            var extraConstant = "";
            if (!string.IsNullOrWhiteSpace(build.ElementRootMapping_ID)) {

                var elementRootMappingID = build.ElementRootMapping_ID;
                while (char.IsDigit(elementRootMappingID[elementRootMappingID.Length - 1])) {
                    elementRootMappingID = elementRootMappingID.Remove(elementRootMappingID.Length - 1);
                }
                code = code.Replace("{has_root_mapping}", $"const string elementrootmapping =  \"{elementRootMappingID}\";");

                extraConstant = "elementrootmapping + ";
                if (build.ForLoop) extraConstant += "i + ";
            } else {
                code = code.Replace("{has_root_mapping}", string.Empty);
            }

            {
                var constant = Builder.BuildConstantStatement(extraConstant + build.RootCanvasGroup.Element_ID);
                var elementStatement = Builder.BuildCanvasGroupStatement(constant);
                code = code.Replace("{root_canvas_group}", elementStatement + Environment.NewLine);
            }

            {
                var texts = "";
                foreach (var text in build.CanvasGroups) {
                    var constant = Builder.BuildConstantStatement(extraConstant + text.Element_ID);
                    var elementStatement = Builder.BuildCanvasGroupStatement(constant);
                    texts += elementStatement + Environment.NewLine;
                }
                code = code.Replace("{canvas_groups}", texts);
            }
            {
                var texts = "";
                foreach (var text in build.Texts) {
                    var constant = Builder.BuildConstantStatement(extraConstant + text.Element_ID);
                    var elementStatement = Builder.BuildTextStatement(constant);
                    texts += elementStatement + Environment.NewLine;
                }
                code = code.Replace("{texts}", texts);
            }
            {
                var images = "";
                foreach (var image in build.Images) {
                    var constant = Builder.BuildConstantStatement(extraConstant + image.Element_ID);
                    var elementStatement = Builder.BuildImageStatement(constant);
                    images += elementStatement + Environment.NewLine;
                }
                code = code.Replace("{images}", images);
            }
            {
                var buttons = "";
                foreach (var button in build.Buttons) {
                    buttons += Builder.BuildButtonReadEvent(build.RootCanvasGroup.GameObject_Name, button.Element_ID, button.GameObject_Name.Replace("Button", "")) + Environment.NewLine;
                }
                code = code.Replace("{buttons}", buttons);

                var buttonsNoEvent = "";
                foreach (var button in build.Buttons) {

                    var constant = Builder.BuildConstantStatement(extraConstant + button.Element_ID);
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
                    toggles += Builder.BuildToggleReadEvent(build.RootCanvasGroup.GameObject_Name, toggle.Element_ID, toggle.GameObject_Name.Replace("Toggle", "")) + Environment.NewLine;
                }
                code = code.Replace("{toggles}", toggles);

                var buttonsNoEvent = "";
                foreach (var button in build.Toggles) {

                    var constant = Builder.BuildConstantStatement(extraConstant + button.Element_ID);
                    var buttonStatement = Builder.BuildToggleStatement(constant);
                    buttonStatement = buttonStatement.Replace(";", "");
                    buttonStatement = $"{ buttonStatement}.IsClicked;";

                    buttonsNoEvent += buttonStatement + Environment.NewLine;
                }
                code = code.Replace("{toggles_no_event}", buttonsNoEvent);
            }
            {
                var additionalEvents = "";
                foreach (var button in build.Buttons) {
                    if (button.GameObject_Name.Contains("Close")) continue;
                    additionalEvents += Builder.BuildUseEvent(build.RootCanvasGroup.GameObject_Name, button.GameObject_Name.Replace("Button", ""));
                }
                code = code.Replace("{additional_events}", additionalEvents);
            }

            return code;
        }

        public static void Build(BuildParams build)
        {
            var path = Path.Combine(Application.dataPath, Name.AssetsPath);
            path = path.Substring(0, path.Length - Path.GetFileName(path).Length);
            path = path + build.RootCanvasGroup.GameObject_Name + ".cs";

            var code = BuildString(build, Name.ScreenExtension);

            File.WriteAllText(path, code);
            AssetDatabase.ImportAsset(path);
        }
    }
}
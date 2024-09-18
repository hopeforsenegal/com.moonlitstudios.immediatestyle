using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class PathUtil
    {
        private static string TemplatePackagePath => "Packages/com.moonlitstudios.system/Editor/Template";
        private const string AssetsPath = "Assets/";

        public static string GetTemplatePackagePath(string filename) => Path.GetFullPath($"{TemplatePackagePath}/{filename}");

        public static string CurrentAssetPath()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrWhiteSpace(path)) {
                path = Path.Combine(Application.dataPath, AssetsPath);
            }
            return path;
        }

        public static string ClipboardText
        {
            get
            {
                var textEditor = new TextEditor();
                textEditor.Paste();
                return textEditor.text;
            }
            set
            {
                var textEditor = new TextEditor { text = value };
                textEditor.OnFocus();
                textEditor.Copy();
            }
        }
    }
}

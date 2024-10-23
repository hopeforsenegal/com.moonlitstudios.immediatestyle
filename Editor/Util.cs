using UnityEngine;

namespace MoonlitSystem
{
    public static class Util
    {
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

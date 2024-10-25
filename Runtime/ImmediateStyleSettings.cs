using System.IO;
using UnityEditor;
using UnityEngine;

namespace MoonlitSystem
{
    public class ImmediateStyleSettings : ScriptableObject
    {
        private const string ImmediateStyleSettingsResDir = "Assets/ImmediateStyle/Resources";

        public static ImmediateStyleSettings LoadInstance()
        {
            var instance = Resources.Load<ImmediateStyleSettings>(nameof(ImmediateStyleSettings));

            if (instance == null) {
                Directory.CreateDirectory(ImmediateStyleSettingsResDir);
                var assetPath = Path.Combine(ImmediateStyleSettingsResDir, $"{nameof(ImmediateStyleSettings)}.asset");
                instance = CreateInstance<ImmediateStyleSettings>();
                instance.removeElementAutomatically = false;
                instance.inlineClipboardGUIDS = true;
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(instance, assetPath);
                AssetDatabase.SaveAssetIfDirty(instance);
#endif
            }

            return instance;
        }

        // We have this as false by default
        // Reasons why you might have this set to 'false'?
        // Because you find the component you meant to add not actually being the one you added?
        // A new user (to Unity) might get tripped on this. But in reality you should probably just set this to 'true'.
        public bool removeElementAutomatically;
        // I personally prefer having the GUIDs as a const that can be referenced from one central file somewhere
        // But it seems that alot of people would prefer that this type of functionality were inlined by default so they can
        // truly just focus on the code that they need to write and not worry about managing GUIDs
        // I'm just too paranoid that i'll accidentally type a character in the guid setion on accident (but if we ever did that we alert you so maybe im just in my head too much)
        // NOTE: This only works on the clipboard (and NOT files) for now
        public bool inlineClipboardGUIDS;
    }
}
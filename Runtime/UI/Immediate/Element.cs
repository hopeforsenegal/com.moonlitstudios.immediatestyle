using System;
using UnityEngine;
using UnityEngine.UI;
#if TMP_PRESENT
using TMPro;
#endif

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class Element : MonoBehaviour
    {
        protected void Reset()
        {
            var settings = ImmediateStyleSettings.LoadInstance();
            Transform t = null;
            ElementData ed = null;
            if (t == null && GetComponent<Button>() != null && GetComponent<ElementButton>() == null) {
                var e = gameObject.AddComponent<ElementButton>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (t == null && GetComponent<Text>() != null && GetComponent<ElementText>() == null) {
                var e = gameObject.AddComponent<ElementText>();
                t = e.transform;
                ed = e.ElementData;
            }
#if TMP_PRESENT
            if (t == null && GetComponent<TMP_Text>() != null && GetComponent<ElementText>() == null) {
                var e = gameObject.AddComponent<ElementText>();
                t = e.transform;
                ed = e.ElementData;
            }
#endif
            if (t == null && GetComponent<Toggle>() != null && GetComponent<ElementToggle>() == null) {
                var e = gameObject.AddComponent<ElementToggle>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (t == null && GetComponent<DragDrop>() != null && GetComponent<ElementDragDrop>() == null) {
                var e = gameObject.AddComponent<ElementDragDrop>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (t == null && GetComponent<InputField>() != null && GetComponent<ElementInputField>() == null) {
                var e = gameObject.AddComponent<ElementInputField>();
                t = e.transform;
                ed = e.ElementData;
            }
#if TMP_PRESENT
            if (t == null && GetComponent<TMP_InputField>() != null && GetComponent<ElementInputField>() == null) {
                var e = gameObject.AddComponent<ElementInputField>();
                t = e.transform;
                ed = e.ElementData;
            }
#endif
            if (t == null && GetComponent<Slider>() != null && GetComponent<ElementSlider>() == null) {
                var e = gameObject.AddComponent<ElementSlider>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (t == null && GetComponent<Dropdown>() != null && GetComponent<ElementDropdown>() == null) {
                var e = gameObject.AddComponent<ElementDropdown>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (t == null && GetComponent<CanvasGroup>() != null && GetComponent<ElementCanvasGroup>() == null) {
                var e = gameObject.AddComponent<ElementCanvasGroup>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (t == null && GetComponent<Image>() != null && GetComponent<ElementImage>() == null) {
                var e = gameObject.AddComponent<ElementImage>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (t != null) {
                ElementData.SetupElementData(ed, t);
                if (settings.removeElementAutomatically) {
                    DestroyImmediate(this);
                }
            }
        }
    }

    [Serializable]
    public class ElementData
    {
        public string ID;
        internal bool MarkedForDisplay;
        internal ImmediateStyle.UpdateCanvasGroupVisibilityFields UpdateCanvasGroupInLateUpdate;

        public static void SetupElementData(ElementData elementData, Transform transform)
        {
            var randomId = RandomUtil.RandomString(new[] { 'a', 'b', 'c', 'd', 'e', 'f', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, 4);
            elementData.ID = GetGameObjectFullPathHierarchy(transform) + randomId;
        }

        public static void SetupElementDataGameObjectName(ElementData elementData, Transform transform)
        {
            elementData.ID = transform.name;
        }

        private static string GetGameObjectFullPathHierarchy(Transform current)
        {
            if (current.parent == null)
                return "/" + current.name.Replace("-", "").Replace("<","");
            return GetGameObjectFullPathHierarchy(current.parent) + "/" + current.name.Replace("-", "").Replace("<", ""); // remove "-" because it messes with constants that get created
        }
    }

    public interface IEditorData
    {   // Unity doesn't have type information on Object (which is returned by target)
        // So since we have to do runtime dispatch anyways, we might as well
        // make the type information return what we want
        public Transform transform { get; }
        public ElementData Data { get; }
    }
    public abstract class BaseEditorData : MonoBehaviour, IEditorData
    {   // If it wasn't obvious already. I am not a big fan of OOP, but again we are forced to do so here. This is both Unity and C# knocking us down a few pegs
        public ElementData ElementData = new ElementData(); // NOTE: The 'readonly' modifier makes things unserializable
        public ElementData Data => ElementData;
    }
}
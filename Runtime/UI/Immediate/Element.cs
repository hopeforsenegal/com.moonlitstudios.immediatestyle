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
            if (GetComponent<Text>() != null && GetComponent<ElementText>() == null) {
                var e = gameObject.AddComponent<ElementText>();
                t = e.transform;
                ed = e.ElementData;
            }
#if TMP_PRESENT
            if (GetComponent<TMP_Text>() != null && GetComponent<ElementText>() == null) {
                var e = gameObject.AddComponent<ElementText>();
                t = e.transform;
                ed = e.ElementData;
            }
#endif
            if (GetComponent<Toggle>() != null && GetComponent<ElementToggle>() == null) {
                var e = gameObject.AddComponent<ElementToggle>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (GetComponent<DragDrop>() != null && GetComponent<ElementDragDrop>() == null) {
                var e = gameObject.AddComponent<ElementDragDrop>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (GetComponent<InputField>() != null && GetComponent<ElementInputField>() == null) {
                var e = gameObject.AddComponent<ElementInputField>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (GetComponent<Slider>() != null && GetComponent<ElementSlider>() == null) {
                var e = gameObject.AddComponent<ElementSlider>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (GetComponent<Dropdown>() != null && GetComponent<ElementDropdown>() == null) {
                var e = gameObject.AddComponent<ElementDropdown>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (GetComponent<CanvasGroup>() != null && GetComponent<ElementCanvasGroup>() == null) {
                var e = gameObject.AddComponent<ElementCanvasGroup>();
                t = e.transform;
                ed = e.ElementData;
            }
            if (GetComponent<Image>() != null && GetComponent<ElementImage>() == null) {
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
        internal ImmediateStyle.UpdateCanvasGroupVisibilityFields UpdateCGInLateUpdate;

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
}
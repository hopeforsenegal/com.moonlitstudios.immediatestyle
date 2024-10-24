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
            if (GetComponent<Button>() != null && GetComponent<ElementButton>() == null) {
                var e = gameObject.AddComponent<ElementButton>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                return;
            }
            if (GetComponent<Text>() != null && GetComponent<ElementText>() == null) {
                var e = gameObject.AddComponent<ElementText>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                return;
            }
#if TMP_PRESENT
            if (GetComponent<TMP_Text>() != null && GetComponent<ElementText>() == null) {
                var e = gameObject.AddComponent<ElementText>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                return;
            }
#endif
            if (GetComponent<Toggle>() != null && GetComponent<ElementToggle>() == null) {
                var e = gameObject.AddComponent<ElementToggle>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                return;
            }
            if (GetComponent<DragDrop>() != null && GetComponent<ElementDragDrop>() == null) {
                var e = gameObject.AddComponent<ElementDragDrop>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                return;
            }
            if (GetComponent<InputField>() != null && GetComponent<ElementInputField>() == null) {
                var e = gameObject.AddComponent<ElementInputField>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                return;
            }
            if (GetComponent<Slider>() != null && GetComponent<ElementSlider>() == null) {
                var e = gameObject.AddComponent<ElementSlider>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                return;
            }
            if (GetComponent<Dropdown>() != null && GetComponent<ElementDropdown>() == null) {
                var e = gameObject.AddComponent<ElementDropdown>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                return;
            }
            if (GetComponent<CanvasGroup>() != null && GetComponent<ElementCanvasGroup>() == null) {
                var e = gameObject.AddComponent<ElementCanvasGroup>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                return;
            }
            if (GetComponent<Image>() != null && GetComponent<ElementImage>() == null) {
                var e = gameObject.AddComponent<ElementImage>();
                ElementData.SetupElementData(e.ElementData, e.transform);
                // ReSharper disable once RedundantJumpStatement
                return;
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
                return "/" + current.name.Replace("-", "");
            return GetGameObjectFullPathHierarchy(current.parent) + "/" + current.name.Replace("-", ""); // remove "-" because it messes with constants that get created
        }
    }
}
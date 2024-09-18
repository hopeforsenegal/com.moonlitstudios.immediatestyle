using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ISelectHandler, IDeselectHandler
    {
        public ElementData ElementData = new ElementData();
        public bool IsClicked { get; internal set; }
        public bool IsHovering { get; private set; }
        public bool IsSelect { get; private set; }
        public bool IsPressed { get; private set; }

        public Button Button { get; private set; }
        public Image Image { get; private set; } // Can only have one graphic
        private ElementRootMapping ElementRootMapping { get; set; }

        protected void Awake()
        {
            Button = GetComponent<Button>();
            Image = GetComponent<Image>();
            ElementRootMapping = ElementRootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Button>();
            Debug.Assert(behaviour != null, $"{nameof(behaviour)} was not set", this);
        }

        protected void Start()
        {
            ImmediateStyle.Register(this, ElementRootMapping);
        }

        protected void OnDestroy()
        {
            ImmediateStyle.Unregister(this, ElementRootMapping);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsClicked = eventData.button == PointerEventData.InputButton.Left;
            IsPressed = eventData.button == PointerEventData.InputButton.Left;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovering = false;
            IsPressed = false;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            IsClicked = true;
        }

        public void OnSelect(BaseEventData eventData)
        {
            IsSelect = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            IsSelect = false;
        }
    }
}
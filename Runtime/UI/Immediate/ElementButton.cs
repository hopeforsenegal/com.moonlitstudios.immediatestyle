using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler, ISelectHandler, IDeselectHandler
    {
        public ElementData ElementData = new ElementData(); // NOTE: The 'readonly' modifier makes things unserializable

        // onClick fires on OnPointerClick.
        // However for ImmediateStyle and responsive GUIs we operate on OnPointerDown
        // https://x.com/ID_AA_Carmack/status/1787850053912064005  
        //
        // Therefore  IsMouseDown is true on OnPointerDown and NOT OnPointerClick... 
        // Conversely IsMouseUp   is true on OnPointerUp, OnPointerClick, AND OnPointerExit (only when IsPressed is true)
        //
        // In any case the most important thing here is that we don't force you to just be IsMouseDown (like we suggest).
        // You have the choice to do things the way Unity already does it if you want with IsMouseUp.
        public bool IsMouseDown { get; internal set; }
        public bool IsMousePressed { get; private set; }
        public bool IsMouseUp { get; internal set; }
        public bool IsHovering { get; private set; }
        public bool IsSelect { get; private set; }

        public Button Button { get; private set; }
        public Image Image { get; private set; } // Can only have one graphic
        private RootMapping RootMapping { get; set; }

        protected void Awake()
        {
            Button = GetComponent<Button>();
            Image = GetComponent<Image>();
            RootMapping = RootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            Debug.Assert(GetComponent<Button>() != null, "UIBehaviour was not set", this);
        }

        protected void Start()
        {
            ImmediateStyle.Register(this, RootMapping);
        }

        protected void OnDestroy()
        {
            ImmediateStyle.Unregister(this, RootMapping);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsMouseDown = eventData.button == PointerEventData.InputButton.Left;
            IsMousePressed = eventData.button == PointerEventData.InputButton.Left;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsMousePressed = false;
            IsMouseUp = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsMousePressed) {
                IsMouseUp = true;
            }
            IsHovering = false;
            IsMousePressed = false;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            IsMouseDown = true;
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
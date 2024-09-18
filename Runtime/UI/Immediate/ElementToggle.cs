using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementToggle : MonoBehaviour, IPointerDownHandler, ISubmitHandler
    {
        public Toggle UIBehaviour { get; private set; }
        private ElementRootMapping ElementRootMapping { get; set; }

        public ElementData ElementData = new ElementData();
        public bool IsClicked { get; internal set; }

        protected void Awake()
        {
            UIBehaviour = GetComponent<Toggle>();
            ElementRootMapping = ElementRootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Toggle>();
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
        }

        public void OnSubmit(BaseEventData eventData)
        {
            IsClicked = true;
        }
    }
}
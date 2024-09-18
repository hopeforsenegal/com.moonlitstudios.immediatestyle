using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementInputField : MonoBehaviour, ISubmitHandler, IPointerClickHandler
    {
        public Image Image { get; private set; }
        public InputField UIBehaviour { get; private set; }
        private ElementRootMapping ElementRootMapping { get; set; }

        public ElementData ElementData = new ElementData();
        internal bool WasFocused;

        public bool HasSubmitted { get; internal set; }
        public bool HasClicked { get; internal set; }

        protected void Awake()
        {
            Image = GetComponent<Image>();
            UIBehaviour = GetComponent<InputField>();
            ElementRootMapping = ElementRootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<InputField>();
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

        public void OnSubmit(BaseEventData eventData)
        {
            HasSubmitted = true; // @reset in ImmediateStyle.LateUpdate
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            HasClicked = true;
        }
    }
}
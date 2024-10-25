using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementToggle : BaseEditorData, IPointerDownHandler, ISubmitHandler
    {
        public Toggle UIBehaviour { get; private set; }
        private RootMapping RootMapping { get; set; }

        public bool IsClicked { get; internal set; }

        protected void Awake()
        {
            UIBehaviour = GetComponent<Toggle>();
            RootMapping = RootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Toggle>();
            Debug.Assert(behaviour != null, $"{nameof(behaviour)} was not set", this);
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
            IsClicked = eventData.button == PointerEventData.InputButton.Left;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            IsClicked = true;
        }
    }
}
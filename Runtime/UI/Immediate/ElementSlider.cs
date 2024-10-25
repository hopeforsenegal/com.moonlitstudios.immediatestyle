using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementSlider : BaseEditorData, IPointerUpHandler
    {
        public Slider UIBehaviour { get; private set; }
        public bool IsMouseUp { get; internal set; }
        private RootMapping RootMapping { get; set; }

        protected void Awake()
        {
            UIBehaviour = GetComponent<Slider>();
            Debug.Assert(UIBehaviour != null, $"{nameof(UIBehaviour)} was not set", this);
            RootMapping = RootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Slider>();
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

        public void OnPointerUp(PointerEventData eventData)
        {
            IsMouseUp = true;
        }
    }
}
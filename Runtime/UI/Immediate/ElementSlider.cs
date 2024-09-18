using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementSlider : MonoBehaviour, IPointerUpHandler
    {
        public Slider UIBehaviour { get; private set; }
        public bool IsMouseUp { get; internal set; }
        private ElementRootMapping ElementRootMapping { get; set; }

        public ElementData ElementData = new ElementData();

        protected void Awake()
        {
            UIBehaviour = GetComponent<Slider>();
            Debug.Assert(UIBehaviour != null, $"{nameof(UIBehaviour)} was not set", this);
            ElementRootMapping = ElementRootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Slider>();
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

        public void OnPointerUp(PointerEventData eventData)
        {
            IsMouseUp = true;
        }
    }
}
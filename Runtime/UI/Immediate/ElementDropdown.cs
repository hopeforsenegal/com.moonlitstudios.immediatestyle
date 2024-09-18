using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementDropdown : MonoBehaviour, ISelectHandler
    {
        public Dropdown UIBehaviour { get; private set; }
        private ElementRootMapping ElementRootMapping { get; set; }

        public ElementData ElementData = new ElementData();
        public bool HasSubmitted { get; internal set; }
        public int Index { get; internal set; }

        protected void Awake()
        {
            UIBehaviour = GetComponent<Dropdown>();
            ElementRootMapping = ElementRootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Dropdown>();
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

        public void OnSelect(BaseEventData eventData)
        {
            HasSubmitted = true; // @reset in ImmediateStyle.LateUpdate
        }
    }
}
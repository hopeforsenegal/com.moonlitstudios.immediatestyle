using UnityEngine;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementDragDrop : MonoBehaviour
    {
        public DragDrop UIBehaviour { get; private set; }
        private ElementRootMapping ElementRootMapping { get; set; }

        public ElementData ElementData = new ElementData();

        protected void Awake()
        {
            UIBehaviour = GetComponent<DragDrop>();
            Debug.Assert(UIBehaviour != null, $"{nameof(UIBehaviour)} was not set", this);
            ElementRootMapping = ElementRootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<DragDrop>();
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
    }
}
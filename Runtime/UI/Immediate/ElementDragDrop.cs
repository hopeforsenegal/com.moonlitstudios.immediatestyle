using UnityEngine;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementDragDrop : MonoBehaviour
    {
        public DragDrop UIBehaviour { get; private set; }
        private RootMapping RootMapping { get; set; }

        public ElementData ElementData = new ElementData();

        protected void Awake()
        {
            UIBehaviour = GetComponent<DragDrop>();
            Debug.Assert(UIBehaviour != null, $"{nameof(UIBehaviour)} was not set", this);
            RootMapping = RootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<DragDrop>();
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
    }
}
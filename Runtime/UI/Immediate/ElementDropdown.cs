using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementDropdown : BaseEditorData, ISelectHandler
    {
        public Dropdown UIBehaviour { get; private set; }
        private RootMapping RootMapping { get; set; }

        public bool HasSubmitted { get; internal set; }
        public int Index { get; internal set; }

        protected void Awake()
        {
            UIBehaviour = GetComponent<Dropdown>();
            RootMapping = RootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Dropdown>();
            Debug.Assert(behaviour != null, $"{nameof(Dropdown)} was not set on {name}", this);
        }

        protected void Start()
        {
            ImmediateStyle.Register(this, RootMapping);
        }

        protected void OnDestroy()
        {
            ImmediateStyle.Unregister(this, RootMapping);
        }

        public void OnSelect(BaseEventData eventData)
        {
            HasSubmitted = true; // @reset in ImmediateStyle.LateUpdate
        }
    }
}
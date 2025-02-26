using UnityEngine;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementSlider : BaseEditorData
    {
        public Slider UIBehaviour { get; private set; }
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
            Debug.Assert(behaviour != null, $"{nameof(Slider)} was not set on {name}", this);
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
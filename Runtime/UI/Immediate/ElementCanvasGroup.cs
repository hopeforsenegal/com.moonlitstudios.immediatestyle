using UnityEngine;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementCanvasGroup : BaseEditorData
    {
        public CanvasGroup CanvasGroup { get; private set; }
        private RootMapping RootMapping { get; set; }

        protected void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            RootMapping = RootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<CanvasGroup>();
            Debug.Assert(behaviour != null, $"{nameof(CanvasGroup)} was not set on {name}", this);
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
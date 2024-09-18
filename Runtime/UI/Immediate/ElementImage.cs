using UnityEngine;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementImage : MonoBehaviour
    {
        public Image Image { get; private set; }
        private ElementRootMapping ElementRootMapping { get; set; }

        public ElementData ElementData = new ElementData();

        // NOTE: to my future self who might randomly see this one day...
        // its completely trivial to move all this functionality into Immediate Style...
        // Doing so would save on overhead of these expensive mono behaviour methods
        // Or at the very least only use Awake or Start but not both
        protected void Awake()
        {
            Image = GetComponent<Image>();
            ElementRootMapping = ElementRootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Image>();
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
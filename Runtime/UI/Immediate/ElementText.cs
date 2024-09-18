using UnityEngine;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementText : MonoBehaviour
    {
        public Text Text { get; private set; }   // Can only have one graphic
        private ElementRootMapping ElementRootMapping { get; set; }

        public ElementData ElementData = new ElementData();

        protected void Awake()
        {
            Text = GetComponent<Text>();
            ElementRootMapping = ElementRootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Text>();
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
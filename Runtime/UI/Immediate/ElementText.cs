using UnityEngine;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementText : MonoBehaviour
    {
        public Text Text { get; private set; }   // Can only have one graphic
        private RootMapping RootMapping { get; set; }

        public ElementData ElementData = new ElementData();

        protected void Awake()
        {
            Text = GetComponent<Text>();
            RootMapping = RootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
            var behaviour = GetComponent<Text>();
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
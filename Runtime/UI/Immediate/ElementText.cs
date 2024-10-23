using UnityEngine;
using UnityEngine.UI;
#if TMP_PRESENT
using TMPro;
#endif

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementText : MonoBehaviour
    {
        public Text Text { get; private set; }   // Can only have one graphic
#if TMP_PRESENT
        public TMP_Text TextPro { get; private set; }   // Can only have one graphic
#endif
        private RootMapping RootMapping { get; set; }

        public ElementData ElementData = new ElementData();

        protected void Awake()
        {
            Text = GetComponent<Text>();
#if TMP_PRESENT
            TextPro = GetComponent<TMP_Text>();
#endif
            RootMapping = RootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
#if TMP_PRESENT
            var behaviour = GetComponent<Text>();
            var behaviour2 = GetComponent<TMP_Text>();
            Debug.Assert(behaviour != null || behaviour2 != null, $"{nameof(behaviour)} was not set", this);
#else
            var behaviour = GetComponent<Text>();
            Debug.Assert(behaviour != null, $"{nameof(behaviour)} was not set", this);
#endif
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
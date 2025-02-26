using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if TMP_PRESENT
using TMPro;
#endif

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementInputField : BaseEditorData, ISubmitHandler, IPointerClickHandler
    {
        public Image Image { get; private set; }
        public InputField UIBehaviour { get; private set; }
#if TMP_PRESENT
        public TMP_InputField UIBehaviourPro { get; private set; }
#endif
        private RootMapping RootMapping { get; set; }

        internal bool WasFocused;

        public bool HasSubmitted { get; internal set; }
        public bool HasClicked { get; internal set; }

        protected void Awake()
        {
            Image = GetComponent<Image>();
            UIBehaviour = GetComponent<InputField>();
#if TMP_PRESENT
            UIBehaviourPro = GetComponent<TMP_InputField>();
#endif
            RootMapping = RootMapping.GetFirstParentOrAssert(this);
        }

        protected void OnValidate()
        {
#if TMP_PRESENT
            var behaviour = GetComponent<InputField>();
            var behaviour2 = GetComponent<TMP_InputField>();
            Debug.Assert(behaviour != null || behaviour2 != null, $"{nameof(behaviour)} was not set", this);
#else
            var behaviour = GetComponent<InputField>();
            Debug.Assert(behaviour != null, $"{nameof(InputField)} was not set on {name}", this);
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

        public void OnSubmit(BaseEventData eventData)
        {
            HasSubmitted = true; // @reset in ImmediateStyle.LateUpdate
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            HasClicked = true;
        }
    }
}
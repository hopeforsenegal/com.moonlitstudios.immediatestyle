using UnityEngine;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementCanvas : BaseEditorData
    {
        protected void OnValidate()
        {
            var behaviour = GetComponent<Canvas>();
            Debug.Assert(behaviour != null, $"{nameof(behaviour)} was not set", this);
        }
    }
}
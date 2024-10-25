using System.Linq;
using UnityEngine;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class Reference : BaseEditorData
    {
        protected void Reset()
        {
            ElementData.SetupElementData(ElementData, transform);
        }

        public static T Find<T>(Object context, Reference[] references, string id) where T : Component
        {
            var reference = references.FirstOrDefault(predicate: prospect =>
            {
                var rootMapping = prospect.GetComponentInParent<RootMapping>();
                if (rootMapping != null) return (rootMapping.ID + prospect.ElementData.ID) == id;
                else return prospect.ElementData.ID == id;
            });
            Debug.Assert(reference != null, $"Unable to find a reference to '{id}'. Does the caller need a root id?", context);
            return reference.GetComponent<T>();
        }

        public static T Find<T>(Object context, string id) where T : Component
        {
            var references = FindObjectsOfType<Reference>(true);
            return Find<T>(context, references, id);
        }
    }
}
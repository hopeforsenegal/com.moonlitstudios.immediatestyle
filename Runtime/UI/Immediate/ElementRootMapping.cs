using System.Collections.Generic;
using UnityEngine;

namespace MoonlitSystem.UI.Immediate
{
    [DisallowMultipleComponent]
    public class ElementRootMapping : MonoBehaviour
    {
        public string ID;

        public void Reset()
        {
            ID = RandomUtil.RandomString(new[] { 'a', 's', 'd', 'f' }, 4);
        }

        public static ElementRootMapping GetFirstParentOrAssert(MonoBehaviour component)
        {
            var mappings = component.GetComponentsInParent<ElementRootMapping>();
            Debug.Assert(mappings.Length <= 1, "We found multiple root mappings for this element", component);
            return mappings.Length > 0 ? mappings[0] : null;
        }

        /*
         * 
            var parents = ElementRootMapping.Find(this, "Line", "Line (1)", "Line (2)", "Line (3)", "Line (4)");

            foreach (var p in parents) {
                ImmediateStyle.Text(p.ID + CanvasOverlayPlayGameModalsPayoutsModalLineOn36, "Placeholder");
                ImmediateStyle.Text(p.ID + CanvasOverlayPlayGameModalsPayoutsModalLineTw51, "Placeholder");
                ImmediateStyle.Text(p.ID + CanvasOverlayPlayGameModalsPayoutsModalLineThe53a, "Placeholder");
                ImmediateStyle.Text(p.ID + CanvasOverlayPlayGameModalsPayoutsModalLineFo496, "Placeholder");
            }
         */
        public static ElementRootMapping[] Find(Object context, params string[] ids)
        {
            var results = new List<ElementRootMapping>();
            var elementRootMappings = FindObjectsOfType<ElementRootMapping>(true);
            foreach (var elementRootMapping in elementRootMappings) {
                foreach (var id in ids) {
                    if (id != elementRootMapping.ID) continue;
                    results.Add(elementRootMapping);
                    break;
                }
            }
            Debug.Assert(results != null && results.Count > 0, $"Unable to find a reference. '{string.Join("|", ids)}'", context);
            return results.ToArray();
        }
    }
}
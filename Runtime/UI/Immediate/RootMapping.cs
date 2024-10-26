using System.Collections.Generic;
using UnityEngine;

namespace MoonlitSystem.UI.Immediate
{
    // This class is useful for declaring a parent root game object where each Reference or Element underneath it can have duplicated GUID
    // This is super useful when using for loops (so think listing things in a for loop like a character select where the elements are the same).
    // If you are using Prefabs for instance you might make the parent of the prefab have this component so you don't have to manually change each member of the prefab to have a unique GUID/
    // Please look at GuessingGame.cs (and its corresponding scene) and its for loop to understand exactly how to use this!
    [DisallowMultipleComponent]
    public class RootMapping : MonoBehaviour
    {
        public string ID;

        public void Reset()
        {
            ID = RandomUtil.RandomString(new[] { 'a', 's', 'd', 'f' }, 4);
        }

        public static RootMapping GetFirstParentOrAssert(MonoBehaviour component)
        {
            var mappings = component.GetComponentsInParent<RootMapping>();
            Debug.Assert(mappings.Length <= 1, "We found multiple root mappings for this component", component);
            return mappings.Length > 0 ? mappings[0] : null;
        }

        /*
         * NOTE: Need to think of a way to explain this. (It might actually not be that useful and even I have only used it once this way)
         *  
         * 
            var parents = RootMapping.Find(this, "Line", "Line (1)", "Line (2)", "Line (3)", "Line (4)");

            foreach (var p in parents) {
                ImmediateStyle.Text(p.ID + CanvasOverlayPlayGameModalsPayoutsModalLineOn36, "Placeholder");
                ImmediateStyle.Text(p.ID + CanvasOverlayPlayGameModalsPayoutsModalLineTw51, "Placeholder");
                ImmediateStyle.Text(p.ID + CanvasOverlayPlayGameModalsPayoutsModalLineThe53a, "Placeholder");
                ImmediateStyle.Text(p.ID + CanvasOverlayPlayGameModalsPayoutsModalLineFo496, "Placeholder");
            }
         */
        public static RootMapping[] Find(Object context, params string[] ids)
        {
            var results = new List<RootMapping>();
            var elementRootMappings = FindObjectsOfType<RootMapping>(true);
            foreach (var elementRootMapping in elementRootMappings) {
                foreach (var id in ids) {
                    if (id != elementRootMapping.ID) continue;
                    results.Add(elementRootMapping); break;
                }
            }
            Debug.Assert(results != null && results.Count > 0, $"Unable to find a reference. '{string.Join("|", ids)}'", context);
            return results.ToArray();
        }
    }
}
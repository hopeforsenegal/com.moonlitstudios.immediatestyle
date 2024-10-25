using UnityEngine;
using UnityEngine.UI;

namespace Editing.BotEditor.StyleComparisons
{
    // This follows the design way (Inspector Callbacks on Game objects) of handling UI (and exists mostly inside the Unity Scene with calling the methods in `On Click`)
    // For the ImmediateStyle UI code with the same functionality see 'ImmediateExample.cs'
    // For the standard way people write UI code with the same functionality see 'StandardExample.cs'
    // Compare and contrast to see what works best and is faster for iteration

    public class DesignExample : MonoBehaviour
    {
        public Text text;
        int value;

        protected void Start()
        {
            Debug.Assert(text != null, "Fancy message", gameObject);
            text.text = $"-> {value}";
        }
        public void RaiseValue()
        {
            value = value + 1;
            text.text = $"-> {value}";
        }
        public void LowerValue()
        {
            value = value - 1;
            text.text = $"-> {value}";
        }
    }
}
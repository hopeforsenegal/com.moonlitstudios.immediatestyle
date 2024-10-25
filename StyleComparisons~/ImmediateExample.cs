using MoonlitSystem.UI.Immediate;
using UnityEngine;

namespace Editing.BotEditor.StyleComparisons
{
    // This follows the ImmediateStyle of handling UI. 
    // For the standard way people write UI code with the same functionality see 'StandardExample.cs'
    // For the design way people write UI code with the same functionality see 'DesignExample.cs'
    // Compare and contrast to see what works best and is faster for iteration

    public class ImmediateExample : MonoBehaviour
    {
        int value;
        protected void Update()
        {
            ImmediateStyle.CanvasGroup("/Canvas/CanvasGroup03b0");
            ImmediateStyle.Text("/Canvas/Value6b89", $"-> {value}");
            if (ImmediateStyle.Button("/Canvas/Leftd414").IsMouseDown) {
                value = value - 1;
            }
            if (ImmediateStyle.Button("/Canvas/Right3c3e").IsMouseDown) {
                value = value + 1;
            }
        }
    }
}
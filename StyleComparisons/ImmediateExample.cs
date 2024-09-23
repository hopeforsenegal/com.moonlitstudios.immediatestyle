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
        void Update()
        {
            const string CanvasLeftd414 = "/Canvas/Leftd414";
            const string CanvasRight3c3e = "/Canvas/Right3c3e";
            const string CanvasValue6b89 = "/Canvas/Value6b89";
            const string CanvasCanvasGroup03b0 = "/Canvas/CanvasGroup03b0";
            ImmediateStyle.CanvasGroup(CanvasCanvasGroup03b0);
            ImmediateStyle.Text(CanvasValue6b89, $"-> {value}");
            if (ImmediateStyle.Button(CanvasLeftd414).IsMouseDown) {
                value = value - 1;
            }
            if (ImmediateStyle.Button(CanvasRight3c3e).IsMouseDown) {
                value = value + 1;
            }
        }
    }
}
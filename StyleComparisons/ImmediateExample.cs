using MoonlitSystem.UI.Immediate;
using UnityEngine;

namespace Editing.BotEditor.StyleComparisons
{
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
using System.Collections.Generic;
using MoonlitSystem.UI;
using MoonlitSystem.UI.Immediate;
using UnityEngine;

namespace Editing.BotEditor.StyleComparisons
{
    // This follows the ImmediateStyle of handling UI. For the standard way people write UI code with the same functionality see 'X_TypicalStyle.cs'
    // Compare and contrast to see what works best and is faster for iteration

    public class A_ImmediateStyle : MonoBehaviour
    {
        public Sprite left;
        public Sprite right;
        int value;
        Sprite valueSprite;
        bool valueToggle;
        int radioIndex;
        string tempInputText;
        string resultText;
        private readonly Color[] colors = { Color.red, Color.black, Color.green };
        private int dropdownIndex;
        protected void Update()
        {
            if (ImmediateStyle.Toggle("/Canvas/CanvasGroup/Toggle74f8", valueToggle).IsClicked) { valueToggle = !valueToggle; } // @toggle
            if (!valueToggle) {
                var color = Color.white;
                if (radioIndex == 1) color = Color.green;
                if (radioIndex == 2) color = Color.red;

                ImmediateStyle.CanvasGroup("/Canvas/CanvasGroup03b0"); // @canvas
                var sliderAlpha = ImmediateStyle.Slider("/Canvas/CanvasGroup/Slider863e").Value;
                ImmediateStyle.SetColor(color);
                ImmediateStyle.Text("/Canvas/Value6b89", $"-> {value}"); // @text
                ImmediateStyle.Text("/Canvas/CanvasGroup/Value (1)ab1e", resultText); // input @text
                if (valueSprite != null) {
                    ImmediateStyle.SetColor(new Color(1, 1, 1, sliderAlpha)); // only update when the mouse raises
                    ImmediateStyle.Image("/Canvas/CanvasGroup/Imagead3b", valueSprite); // @dynamic image
                }
                ImmediateStyle.ClearColor();

                if (radioIndex != 2) { // @dragdrop
                    var hasDropped1 = ImmediateStyle.DragDrop("/Canvas/CanvasGroup/DragAndDrop0e99", out var component).IsMouseUp;
                    var hasDropped2 = ImmediateStyle.DragDrop("/Canvas/CanvasGroup/DragAndDrop (1)2173", out var component2).IsMouseUp;

                    if (component.IsDragging) ImmediateStyle.FollowCursor(component.transform);
                    if (component2.IsDragging) ImmediateStyle.FollowCursor(component2.transform);

                    if (radioIndex == 0) {
                        if (hasDropped1) {
                            component.transform.position = component.PinnedPosition;
                        }
                        if (hasDropped2) {
                            component2.transform.position = component2.PinnedPosition;
                        }
                    }
                }

                var components = new List<DragDrop>(); // @swappable dragdrop
                for (var i = 0; i < 4; i++) {
                    ImmediateStyle.DragDrop($"swap{i}" + "/Canvas/CanvasGroup/Swappables/Swap1be93", out var swappable);
                    components.Add(swappable);
                    if (swappable.IsDragging) ImmediateStyle.FollowCursor(swappable.transform);
                }
                for (var i = 0; i < 4; i++) {
                    var swappable = components[i];
                    if (!swappable.IsMouseUp) continue; // We can only do one of these in a frame anyways
                    for (var j = 0; j < 4; j++) {
                        if (i == j) continue;
                        var swappable2 = components[j];
                        if (RectTransformUtility.RectangleContainsScreenPoint(swappable2.RectTransform, swappable.transform.position)) {
                            var tempPinnedPosition = swappable.PinnedPosition; // swap the pinned positions between the two
                            swappable.PinnedPosition = swappable2.PinnedPosition;
                            swappable2.PinnedPosition = tempPinnedPosition;
                        }
                    }
                    foreach (var s in components) s.transform.position = s.PinnedPosition;
                    break; // We can only do one of these in a frame anyways
                }
                if (ImmediateStyle.InputField("/Canvas/CanvasGroup/InputField (Legacy)5474", new[] { KeyCode.Return, KeyCode.KeypadEnter }, ref tempInputText).HasSubmitted) {
                    resultText = tempInputText; //@inputfield
                }
                for (var i = 0; i < 3; i++) {   // @radiobuttons @rootmapping
                    radioIndex = ImmediateStyle.Toggle($"{i}/Canvas/CanvasGroup/RadioButtons/Toggle0a56", radioIndex == i).IsClicked ? i : radioIndex;
                }
                var dropdownData = ImmediateStyle.Dropdown("/Canvas/CanvasGroup/Dropdown (Legacy)55fc", new[] { new UnityEngine.UI.Dropdown.OptionData($"Color: {colors[0]}"), new UnityEngine.UI.Dropdown.OptionData($"Color: {colors[1]}"), new UnityEngine.UI.Dropdown.OptionData($"Color: {colors[2]}") });
                dropdownIndex = dropdownData.HasSubmitted ? dropdownData.Index : dropdownIndex;
                ImmediateStyle.SetColor(colors[dropdownIndex]);
                ImmediateStyle.Image("/Canvas/CanvasGroup/DropDownImage40d6");
                ImmediateStyle.ClearColor();
                var leftClicked = ImmediateStyle.Button("/Canvas/Leftd414").IsMouseDown; // @buttons
                var rightClicked = ImmediateStyle.Button("/Canvas/Right3c3e").IsMouseDown;
                value = leftClicked ? value - 1 : value;
                value = rightClicked ? value + 1 : value;
                valueSprite = leftClicked ? left : valueSprite;
                valueSprite = rightClicked ? right : valueSprite;
            }
        }
    }
}

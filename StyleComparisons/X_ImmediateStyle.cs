using System.Collections.Generic;
using MoonlitSystem.UI;
using MoonlitSystem.UI.Immediate;
using UnityEngine;
using UnityEngine.UI;

namespace Editing.BotEditor.StyleComparisons
{
    public class X_ImmediateStyle : MonoBehaviour
    {
        public Sprite left;
        public Sprite right;
        int value;
        Sprite valueSprite;
        bool valueToggle;
        int radioIndex;
        string tempInputText;
        string resultText;
        private Color[] colors = { Color.red, Color.black, Color.green };
        private int dropdownIndex;
        void Update()
        {
            const string CanvasCanvasGroupDropdownLegacy55fc = "/Canvas/CanvasGroup/Dropdown (Legacy)55fc";
            const string CanvasCanvasGroupDropDownImage40d6 = "/Canvas/CanvasGroup/DropDownImage40d6";
            const string CanvasCanvasGroupSlider863e = "/Canvas/CanvasGroup/Slider863e";
            const string CanvasCanvasGroupSwappablesSwap1be93 = "/Canvas/CanvasGroup/Swappables/Swap1be93";
            const string CanvasLeftd414 = "/Canvas/Leftd414";
            const string CanvasRight3c3e = "/Canvas/Right3c3e";
            const string CanvasValue6b89 = "/Canvas/Value6b89";
            const string CanvasCanvasGroup03b0 = "/Canvas/CanvasGroup03b0";
            const string CanvasCanvasGroupImagead3b = "/Canvas/CanvasGroup/Imagead3b";
            const string CanvasCanvasGroupToggle74f8 = "/Canvas/CanvasGroup/Toggle74f8";
            const string CanvasCanvasGroupRadioButtonsToggle0a56 = "/Canvas/CanvasGroup/RadioButtons/Toggle0a56";
            const string CanvasCanvasGroupValue1ab1e = "/Canvas/CanvasGroup/Value (1)ab1e";
            const string CanvasCanvasGroupInputFieldLegacy5474 = "/Canvas/CanvasGroup/InputField (Legacy)5474";
            const string CanvasCanvasGroupDragAndDrop0e99 = "/Canvas/CanvasGroup/DragAndDrop0e99";
            const string CanvasCanvasGroupDragAndDrop12173 = "/Canvas/CanvasGroup/DragAndDrop (1)2173";
            if (ImmediateStyle.Toggle(CanvasCanvasGroupToggle74f8, valueToggle).IsClicked) { valueToggle = !valueToggle; } // @toggle
            if (!valueToggle) {
                var color = Color.white;
                if (radioIndex == 1) color = Color.green;
                if (radioIndex == 2) color = Color.red;

                ImmediateStyle.CanvasGroup(CanvasCanvasGroup03b0); // @canvas
                var sliderAlpha = ImmediateStyle.Slider(CanvasCanvasGroupSlider863e).Value;
                ImmediateStyle.SetColor(color);
                ImmediateStyle.Text(CanvasValue6b89, $"-> {value}"); // @text
                ImmediateStyle.Text(CanvasCanvasGroupValue1ab1e, resultText); // input @text
                if (valueSprite != null) {
                    ImmediateStyle.SetColor(new Color(1, 1, 1, sliderAlpha)); // only update when the mouse raises
                    ImmediateStyle.Image(CanvasCanvasGroupImagead3b, valueSprite); // @dynamic image
                }
                ImmediateStyle.ClearColor();

                if (radioIndex != 2) { // @dragdrop
                    var hasDropped1 = ImmediateStyle.DragDrop(CanvasCanvasGroupDragAndDrop0e99, out var component).IsMouseUp;
                    var hasDropped2 = ImmediateStyle.DragDrop(CanvasCanvasGroupDragAndDrop12173, out var component2).IsMouseUp;

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
                for (int i = 0; i < 4; i++) {
                    ImmediateStyle.DragDrop($"swap{i}" + CanvasCanvasGroupSwappablesSwap1be93, out var swappable);
                    components.Add(swappable);
                    if (swappable.IsDragging) ImmediateStyle.FollowCursor(swappable.transform);
                }
                for (int i = 0; i < 4; i++) {
                    var swappable = components[i];
                    if (!swappable.IsMouseUp) continue; // We can only do one of these in a frame anyways
                    for (int j = 0; j < 4; j++) {
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
                if (ImmediateStyle.InputField(CanvasCanvasGroupInputFieldLegacy5474, new[] { KeyCode.Return, KeyCode.KeypadEnter }, ref tempInputText).HasSubmitted) {
                    resultText = tempInputText; //@inputfield
                }
                for (var i = 0; i < 3; i++) {   // @radiobuttons
                    radioIndex = ImmediateStyle.Toggle($"{i}{CanvasCanvasGroupRadioButtonsToggle0a56}", radioIndex == i).IsClicked ? i : radioIndex;
                }
                var dropdownData = ImmediateStyle.Dropdown(CanvasCanvasGroupDropdownLegacy55fc, new[] { new UnityEngine.UI.Dropdown.OptionData($"Color: {colors[0]}"), new UnityEngine.UI.Dropdown.OptionData($"Color: {colors[1]}"), new UnityEngine.UI.Dropdown.OptionData($"Color: {colors[2]}") });
                dropdownIndex = dropdownData.HasSubmitted ? dropdownData.Index : dropdownIndex;
                ImmediateStyle.SetColor(colors[dropdownIndex]);
                ImmediateStyle.Image(CanvasCanvasGroupDropDownImage40d6);
                ImmediateStyle.ClearColor();
                var leftClicked = ImmediateStyle.Button(CanvasLeftd414).IsMouseDown; // @buttons
                var rightClicked = ImmediateStyle.Button(CanvasRight3c3e).IsMouseDown;
                value = leftClicked ? value - 1 : value;
                value = rightClicked ? value + 1 : value;
                valueSprite = leftClicked ? left : valueSprite;
                valueSprite = rightClicked ? right : valueSprite;
            }
        }
    }
}
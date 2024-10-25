using System.Collections.Generic;
using MoonlitSystem.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Editing.BotEditor.StyleComparisons
{
    // This follows the standard way of handling UI. For the ImmediateStyle UI code with the same functionality see 'X_ImmediateStyle.cs'
    // Compare and contrast to see what works best and is faster for iteration

    public class A_TypicalStyle : MonoBehaviour
    {
        public Toggle canvasToggle;
        public CanvasGroup canvasGroup;
        public Button leftButton, rightButton;
        public Text text;
        public Image image;
        public Text inputFieldResult;
        public Toggle[] toggles;
        public InputField inputField;
        public DragDrop dragDrop, dragDrop2;
        public DragDrop s1, s2, s3, s4;
        public Sprite left;
        public Sprite right;
        public Slider slider;
        public Dropdown dropdown;
        public Image dropdownImage;
        int value;
        private readonly List<DragDrop> swappables = new List<DragDrop>();
        private readonly Color[] colors = { Color.red, Color.black, Color.green };
        protected void Start()
        {
            canvasToggle.onValueChanged.AddListener(ToggleTheCanvasGroup);
            leftButton.onClick.AddListener(LowerValue);
            rightButton.onClick.AddListener(RaiseValue);
            inputField.onSubmit.AddListener(SubmitText);
            dragDrop.OnReleased.AddListener(DragDrop_OnReleased);
            dragDrop2.OnReleased.AddListener(DragDrop_OnReleased);
            toggles[2].onValueChanged.AddListener(OnNoDragToggleClicked);
            s1.OnReleased.AddListener(Swappable_OnReleased);
            s2.OnReleased.AddListener(Swappable_OnReleased);
            s3.OnReleased.AddListener(Swappable_OnReleased);
            s4.OnReleased.AddListener(Swappable_OnReleased);
            slider.onValueChanged.AddListener(OnSliderChanged);
            dropdown.ClearOptions();
            dropdown.options.Add(new Dropdown.OptionData($"Color: {colors[0]}"));
            dropdown.options.Add(new Dropdown.OptionData($"Color: {colors[1]}"));
            dropdown.options.Add(new Dropdown.OptionData($"Color: {colors[2]}"));
            dropdown.onValueChanged.AddListener(OnDropdownSelection);
            dropdownImage.color = colors[0];
            swappables.AddRange(new[] { s1, s2, s3, s4 });
            image.enabled = false; // we want to start off as not showing anything (like the other example)
            text.text = $"-> {value}";
            inputFieldResult.text = string.Empty; // We want to clear the text that is in it on start (like the other example)
        }
        protected void Update()
        {
            var textColor = Color.white;
            if (toggles[1].isOn) textColor = Color.green;
            if (toggles[2].isOn) textColor = Color.red;
            text.color = textColor;
            inputFieldResult.color = textColor;
        }
        private void SubmitText(string val)
        {
            inputFieldResult.text = val;
        }
        private void ToggleTheCanvasGroup(bool newValue)
        {
            canvasGroup.alpha = newValue ? 1 : 0;
            canvasGroup.blocksRaycasts = newValue;
            canvasGroup.interactable = newValue;
        }
        private void RaiseValue()
        {
            value = value + 1;
            image.sprite = right;
            image.enabled = true;
            text.text = $"-> {value}";
        }
        private void LowerValue()
        {
            value = value - 1;
            image.sprite = left;
            image.enabled = true;
            text.text = $"-> {value}";
        }
        private void DragDrop_OnReleased(DragDrop released)
        {
            if (toggles[0].isOn) {
                released.transform.position = released.PinnedPosition;
            }
        }
        private void OnNoDragToggleClicked(bool isOn)
        {
            dragDrop.enabled = !isOn;
            dragDrop2.enabled = !isOn;
        }
        private void Swappable_OnReleased(DragDrop swappable)
        {
            foreach (var swappable2 in swappables) {
                if (swappable.name == swappable2.name) continue;
                if (RectTransformUtility.RectangleContainsScreenPoint(swappable2.RectTransform, swappable.transform.position)) {
                    var tempPinnedPosition = swappable.PinnedPosition; // swap the pinned positions between the two
                    swappable.PinnedPosition = swappable2.PinnedPosition;
                    swappable2.PinnedPosition = tempPinnedPosition;
                }
            }
            foreach (var s in swappables) s.transform.position = s.PinnedPosition;
        }
        private void OnSliderChanged(float sliderValue)
        {
            image.color = new Color(1, 1, 1, sliderValue);
        }
        private void OnDropdownSelection(int index)
        {
            dropdownImage.color = colors[index];
        }
    }
}
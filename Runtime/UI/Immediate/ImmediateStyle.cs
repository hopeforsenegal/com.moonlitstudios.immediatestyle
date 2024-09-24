using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitSystem.UI.Immediate
{
    // Just add a single ImmediateStyle singleton to your scene to use all the following methods

    public class ImmediateStyle : MonoBehaviour
    {
        public static ButtonData Button(string id)
        {
            var hasElement = Instance.m_InteractButtons.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped button. Did start get called? Does it need a root id?");
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                // Buttons may or may not have an image. And the image might be a sibling or it might be a child.
                // This code is specifically for sibling images. If we have a child image and not a sibling we should not crash
                // And we can assume that this child would have its own immediate call to color in that scenario
                if (element.Image && Instance.m_HasSetColor) {
                    element.Image.color = Instance.m_Color;
                }
                return new ButtonData
                {
                    IsMouseDown = element.IsClicked,
                    IsMousePressed = element.IsPressed,
                    IsMouseHovering = element.IsHovering || element.IsSelect,
                };
            }
            return default;
        }

        public static ButtonData Button(string id, out Button button)
        {
            var hasElement = Instance.m_InteractButtons.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped button. Did start get called? Does it need a root id?");
            button = null;
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                button = element.Button;
                // Buttons may or may not have an image. And the image might be a sibling or it might be a child.
                // This code is specifically for sibling images. If we have a child image and not a sibling we should not crash
                // And we can assume that this child would have its own immediate call to color in that scenario
                if (element.Image && Instance.m_HasSetColor) {
                    element.Image.color = Instance.m_Color;
                }
                return new ButtonData
                {
                    IsMouseDown = element.IsClicked,
                    IsMousePressed = element.IsPressed,
                    IsMouseHovering = element.IsHovering || element.IsSelect,
                };
            }
            return default;
        }

        public static void CanvasGroup(string id)
        {
            var hasElement = Instance.m_InteractCanvasGroups.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped canvas group. Did start get called? Does it need a root id?");
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
            }
        }

        public static void CanvasGroup(string id, out CanvasGroup canvasGroup)
        {
            var hasElement = Instance.m_InteractCanvasGroups.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped canvas group. Did start get called? Does it need a root id?");
            canvasGroup = null;
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                element.ElementData.SpecialSauce = true;
                canvasGroup = element.CanvasGroup;
            }
        }

        // Show with whatever sprite it already has or not at all
        public static void Image(string id)
        {
            var hasElement = Instance.m_InteractImages.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped image. Did start get called? Does it need a root id?");
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                if (Instance.m_HasSetColor) {
                    element.Image.color = Instance.m_Color;
                }
            }
        }

        // Show with a particular sprite or not at all
        public static void Image(string id, Sprite sprite)
        {
            var hasElement = Instance.m_InteractImages.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped image. Did start get called? Does it need a root id?");
            Debug.Assert(sprite, "sprite was null");
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                element.Image.sprite = sprite;
                if (Instance.m_HasSetColor) {
                    element.Image.color = Instance.m_Color;
                }
            }
        }

        // This better be awesome in the long run
        public static void Image(string id, out Image image)
        {
            var hasElement = Instance.m_InteractImages.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped image. Did start get called? Does it need a root id?");
            image = null;
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                image = element.Image;
                if (Instance.m_HasSetColor) {
                    element.Image.color = Instance.m_Color;
                }
            }
        }
        public static void Text(string id)
        {
            var hasElement = Instance.m_InteractTexts.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped text. Did start get called? Does it need a root id?");
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                if (Instance.m_HasSetColor) {
                    element.Text.color = Instance.m_Color;
                }
            }
        }

        public static void Text(string id, string text)
        {
            var hasElement = Instance.m_InteractTexts.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped text. Did start get called? Does it need a root id?");
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                element.Text.text = text;
                if (Instance.m_HasSetColor) {
                    element.Text.color = Instance.m_Color;
                }
            }
        }

        public static ToggleData Toggle(string id, bool isOn)
        {
            var hasElement = Instance.m_InteractToggles.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped toggle. Did start get called? Does it need a root id?");
            var isClicked = false;
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                if (element.IsClicked) {
                    isOn = !isOn;
                    isClicked = true;
                }
                element.UIBehaviour.SetIsOnWithoutNotify(isOn);
            }
            return new ToggleData { IsOn = isOn, IsClicked = isClicked };
        }

        public static DragDropData DragDrop(string id, out DragDrop dragDrop)
        {
            var hasElement = Instance.m_InteractDragDrops.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped drag drop. Did start get called? Does it need a root id?");
            dragDrop = null;
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                dragDrop = element.UIBehaviour;
                return new DragDropData
                {
                    IsMouseUp = element.UIBehaviour.IsMouseUp,
                    IsHovering = element.UIBehaviour.IsMouseHovering,
                    IsDragging = element.UIBehaviour.IsDragging,
                    Position = new Vector2(element.UIBehaviour.transform.position.x, element.UIBehaviour.transform.position.y),
                };
            }
            return default;
        }

        // In order to appear over other UI elements it must be the last sibling. So plan accordingly.
        public static void FollowCursor(Transform transform)
        {
            transform.position = CpyWithZ(Input.mousePosition, 10f);
            transform.SetAsLastSibling();
        }

        public static InputFieldData InputField(string id, IEnumerable<KeyCode> keyCodes, ref string text)
        {
            var hasElement = Instance.m_InteractInputFields.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped input field. Did start get called? Does it need a root id?");
            var hasSubmitted = false;
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                var hasPressedSubmitKeyCode = FromKeycode(keyCodes);
                var touchscreenKeyboard = element.UIBehaviour.touchScreenKeyboard;
                var hasHitSubmit = hasPressedSubmitKeyCode
                    || (touchscreenKeyboard != null && touchscreenKeyboard.status == TouchScreenKeyboard.Status.Done);
                var isFocused = element.UIBehaviour.isFocused || element.WasFocused;

                if (isFocused) {
                    text = element.UIBehaviour.text;
                } else {
                    element.UIBehaviour.text = text;
                }

                // ReSharper disable once Unity.InefficientPropertyAccess
                element.WasFocused = element.UIBehaviour.isFocused;    // store for next frame
                if (element.HasSubmitted || isFocused && hasHitSubmit) {
                    hasSubmitted = true;
                }
                if (element.Image && Instance.m_HasSetColor) {
                    element.Image.color = Instance.m_Color;
                }
            }
            return new InputFieldData { HasSubmitted = hasSubmitted, HasClicked = element.HasClicked };
        }

        public static void FocusInputField(string id)
        {
            var hasElement = Instance.m_InteractInputFields.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped input field. Did start get called? Does it need a root id?");
            if (hasElement) {
                element.UIBehaviour.Select();
            }
        }

        public static SliderData Slider(string id)
        {
            var hasElement = Instance.m_InteractSliders.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped slider. Did start get called? Does it need a root id?");
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                return new SliderData
                {
                    Value = element.UIBehaviour.value,
                };
            }
            return default;
        }

        public static SliderData Slider(string id, out Slider slider)
        {
            var hasElement = Instance.m_InteractSliders.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped slider. Did start get called? Does it need a root id?");
            slider = null;
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                slider = element.UIBehaviour;
                return new SliderData
                {
                    Value = element.UIBehaviour.value,
                };
            }
            return default;
        }

        public static DropdownData Dropdown(string id, IEnumerable<Dropdown.OptionData> options)
        {
            var hasElement = Instance.m_InteractDropdowns.TryGetValue(id, out var element);
            Debug.Assert(hasElement, $"{id} is not a mapped dropdown. Did start get called? Does it need a root id?");
            if (hasElement) {
                element.ElementData.MarkedForDisplay = true;
                // we could speed this up a lot.. I can imagine cases of drop downs with intensive options
                // first thing that comes to mind is to cache or use a hashcode of some sort (perhaps this is what the object comparison is already doing?)
                if (element.UIBehaviour.options != options) {
                    element.UIBehaviour.options = new List<Dropdown.OptionData>(options);
                }
            }
            return new DropdownData { HasSubmitted = element.HasSubmitted, Index = element.UIBehaviour.value };
        }

        public static void SetColor(Color color)
        {
            Instance.m_HasSetColor = true;
            Instance.m_Color = color;
        }

        public static void ClearColor()
        {
            Instance.m_HasSetColor = false;
            Instance.m_Color = Color.white;
        }

        // ReSharper disable NotAccessedField.Global
        public struct ButtonData
        {
            public bool IsMouseHovering { get; internal set; }
            public bool IsMouseDown { get; internal set; }
            public bool IsMousePressed { get; internal set; }
        }

        public struct ToggleData
        {
            public bool IsOn { get; internal set; }
            public bool IsClicked { get; internal set; }
        }

        public struct DragDropData
        {
            public Vector2 Position { get; internal set; }
            public bool IsDragging { get; internal set; }
            public bool IsMouseUp { get; internal set; }
            public bool IsHovering { get; internal set; }
        }

        public struct InputFieldData
        {
            public bool HasClicked { get; internal set; }
            public bool HasSubmitted { get; internal set; }
        }

        public struct SliderData
        {
            public float Value { get; internal set; }
        }

        public struct DropdownData
        {
            public bool HasSubmitted { get; internal set; }
            public int Index { get; internal set; }
        }

        // ReSharper restore NotAccessedField.Global

        private readonly Dictionary<string, ElementCanvasGroup> m_InteractCanvasGroups = new Dictionary<string, ElementCanvasGroup>();
        private readonly Dictionary<string, ElementText> m_InteractTexts = new Dictionary<string, ElementText>();
        private readonly Dictionary<string, ElementImage> m_InteractImages = new Dictionary<string, ElementImage>();
        private readonly Dictionary<string, ElementButton> m_InteractButtons = new Dictionary<string, ElementButton>();
        private readonly Dictionary<string, ElementToggle> m_InteractToggles = new Dictionary<string, ElementToggle>();
        private readonly Dictionary<string, ElementDragDrop> m_InteractDragDrops = new Dictionary<string, ElementDragDrop>();
        private readonly Dictionary<string, ElementInputField> m_InteractInputFields = new Dictionary<string, ElementInputField>();
        private readonly Dictionary<string, ElementSlider> m_InteractSliders = new Dictionary<string, ElementSlider>();
        private readonly Dictionary<string, ElementDropdown> m_InteractDropdowns = new Dictionary<string, ElementDropdown>();

        private readonly HashSet<string> m_RootMappings = new HashSet<string>();
        private bool m_HasSetColor;
        private Color m_Color;
        private static ImmediateStyleProjectSettings sImmediateStyleProjectSettings;

        private static ImmediateStyle Instance
        {
            get;
            set;
        }

        protected void Awake()
        {
            if (Instance == null) {
                Instance = this;
                Instance.m_Color = Color.white; // default color
                // Load in project settings
                sImmediateStyleProjectSettings = ImmediateStyleProjectSettings.LoadInstance();
                DontDestroyOnLoad(gameObject);
            } else if (Instance != this) {
                Destroy(gameObject);
            }
        }

        protected void OnValidate()
        {
            var allRoots = FindObjectsOfType<ElementRootMapping>();
            var uniqueIds = new HashSet<string>();
            // if we have an exception then one of our roots have the same name
            foreach (var item in allRoots) {
                if (uniqueIds.Contains(item.ID)) {
                    Debug.LogError($"You have a {nameof(ElementRootMapping)} with the same ID '{item.ID}'. Ignore if you have not made each Prefab or clone unique yet.");
                    return;
                }
                uniqueIds.Add(item.ID);
            }
        }

        protected void OnDestroy()
        {
            if (Instance != this) return;
            Instance = null;
        }

        protected void LateUpdate()
        {
            /*** 
             * For all these components
             * 1. Update the interactability of these components
             * 2. Then reset the state (marked for display)
             */
            foreach (var entry in m_InteractButtons) {
                var button = entry.Value.Button;
                if (entry.Value.ElementData.MarkedForDisplay != button.enabled) {
                    button.enabled = entry.Value.ElementData.MarkedForDisplay;
                }
                // Instead of having an issue with this... just split your button and image up (they can live on the same game object)
                var image = entry.Value.Image;
                if (image != null && entry.Value.ElementData.MarkedForDisplay != image.enabled) {
                    image.enabled = entry.Value.ElementData.MarkedForDisplay;
                }

                entry.Value.ElementData.MarkedForDisplay = false;
                entry.Value.IsClicked = false;
            }
            foreach (var entry in m_InteractCanvasGroups) {
                var behavior = entry.Value.CanvasGroup;
                var isOn = entry.Value.ElementData.MarkedForDisplay;
                if (entry.Value.ElementData.MarkedForDisplay) {
                    behavior.blocksRaycasts = isOn;
                    behavior.interactable = isOn;
                    if (!entry.Value.ElementData.SpecialSauce) {
                        behavior.alpha = isOn ? 1f : 0f;
                    }
                } else {
                    behavior.alpha = isOn ? 1f : 0f;
                    behavior.blocksRaycasts = isOn;
                    behavior.interactable = isOn;
                }

                entry.Value.ElementData.MarkedForDisplay = false;
            }
            foreach (var entry in m_InteractImages) {
                var behavior = entry.Value.Image;
                if (entry.Value.ElementData.MarkedForDisplay != behavior.enabled) {
                    behavior.enabled = entry.Value.ElementData.MarkedForDisplay;
                }

                entry.Value.ElementData.MarkedForDisplay = false;
            }
            foreach (var entry in m_InteractTexts) {
                var behavior = entry.Value.Text;
                if (entry.Value.ElementData.MarkedForDisplay != behavior.enabled) {
                    behavior.enabled = entry.Value.ElementData.MarkedForDisplay;
                }

                entry.Value.ElementData.MarkedForDisplay = false;
            }
            foreach (var entry in m_InteractToggles) {
                var behavior = entry.Value.UIBehaviour;
                if (entry.Value.ElementData.MarkedForDisplay != behavior.gameObject.activeInHierarchy) {
                    behavior.gameObject.SetActive(entry.Value.ElementData.MarkedForDisplay);
                }

                entry.Value.ElementData.MarkedForDisplay = false;
                entry.Value.IsClicked = false;
            }

            foreach (var entry in m_InteractDragDrops) {
                var behavior = entry.Value.UIBehaviour;
                if (entry.Value.ElementData.MarkedForDisplay != behavior.enabled) {
                    behavior.enabled = entry.Value.ElementData.MarkedForDisplay;
                }

                entry.Value.ElementData.MarkedForDisplay = false;
                // it handles its own isClicked
            }

            foreach (var entry in m_InteractInputFields) {
                var behavior = entry.Value.UIBehaviour;
                if (entry.Value.ElementData.MarkedForDisplay != behavior.enabled) {
                    behavior.enabled = entry.Value.ElementData.MarkedForDisplay;
                }

                entry.Value.ElementData.MarkedForDisplay = false;
                entry.Value.HasSubmitted = false;
                entry.Value.HasClicked = false;
            }

            foreach (var entry in m_InteractSliders) {
                var behavior = entry.Value.UIBehaviour;
                if (entry.Value.ElementData.MarkedForDisplay != behavior.enabled) {
                    behavior.enabled = entry.Value.ElementData.MarkedForDisplay;
                }

                entry.Value.ElementData.MarkedForDisplay = false;
                entry.Value.IsMouseUp = false;
            }
            foreach (var entry in m_InteractDropdowns) {
                var behavior = entry.Value.UIBehaviour;
                if (entry.Value.ElementData.MarkedForDisplay != behavior.enabled) {
                    behavior.enabled = entry.Value.ElementData.MarkedForDisplay;
                }

                entry.Value.ElementData.MarkedForDisplay = false;
                entry.Value.HasSubmitted = false;
            }
        }

        internal static void Register<T>(T element, ElementRootMapping elementRootMapping) where T : MonoBehaviour
        {
            Debug.Assert(Instance, $"No GameObject with {nameof(ImmediateStyle)} Singleton Component is within the scene!");
            if (elementRootMapping == null) {
                InnerRegister(element, string.Empty);
            } else {
                Debug.Assert(!string.IsNullOrWhiteSpace(elementRootMapping.ID), "A mapping with an empty id. This is really bad!");
                // The goal here is simply to keep a small list of these mapping ids.
                // and just use them in a brute force fashion to look up ourselves out of the existing data structures/dictionaries.
                // basically just add a simple id to the front.
                if (!Instance.m_RootMappings.Contains(elementRootMapping.ID)) {
                    Instance.m_RootMappings.Add(elementRootMapping.ID);
                }

                InnerRegister(element, elementRootMapping.ID);
            }
        }

        internal static void Unregister<T>(T element, ElementRootMapping elementRootMapping) where T : MonoBehaviour
        {
            if (Instance != null) {
                if (elementRootMapping == null) {
                    InnerUnregister(element, string.Empty);
                } else {
                    Debug.Assert(!string.IsNullOrWhiteSpace(elementRootMapping.ID), "A mapping with an empty id. This is really bad!");
                    InnerUnregister(element, elementRootMapping.ID);
                }
            }
        }

        private static void InnerRegister<T>(T element, string prefix) where T : MonoBehaviour
        {
            // Just introspect and place in the right 'bin'
            switch (element) {
                case ElementCanvasGroup cg: {
                        var id = prefix + cg.ElementData.ID;
                        Debug.Assert(!Instance.m_InteractCanvasGroups.ContainsKey(id), $"Duplicate entry found for id '{id}'");
                        Instance.m_InteractCanvasGroups[id] = cg;
                    }
                    break;
                case ElementText t: {
                        var id = prefix + t.ElementData.ID;
                        Debug.Assert(!Instance.m_InteractTexts.ContainsKey(id), $"Duplicate entry found for id '{id}'");
                        Instance.m_InteractTexts[id] = t;
                    }
                    break;
                case ElementImage i: {
                        var id = prefix + i.ElementData.ID;
                        Debug.Assert(!Instance.m_InteractImages.ContainsKey(id), $"Duplicate entry found for id '{id}'");
                        Instance.m_InteractImages[id] = i;
                    }
                    break;
                case ElementToggle tg: {
                        var id = prefix + tg.ElementData.ID;
                        Debug.Assert(!Instance.m_InteractToggles.ContainsKey(id), $"Duplicate entry found for id '{id}'");
                        Instance.m_InteractToggles[id] = tg;
                    }
                    break;
                case ElementDragDrop dd: {
                        var id = prefix + dd.ElementData.ID;
                        Debug.Assert(!Instance.m_InteractDragDrops.ContainsKey(id), $"Duplicate entry found for id '{id}'");
                        Instance.m_InteractDragDrops[id] = dd;
                        // in this special case we do some overriding to make things a bit more consistent
                        // but basically is a way of forcing all the DragAndDrop components to have a unified setting if you want.
                        // And, conversely, also allows us to default DragAndDrop to have FollowMouseCursor = true as default which makes sense inside Unity
                        if (sImmediateStyleProjectSettings.followCursorRetained != ImmediateStyleProjectSettings.FollowCursorRetained.NoOverride) {
                            dd.UIBehaviour.FollowMouseCursor = sImmediateStyleProjectSettings.followCursorRetained == ImmediateStyleProjectSettings.FollowCursorRetained.OverrideFollowCursor;
                        }
                    }
                    break;
                case ElementButton b: {
                        var id = prefix + b.ElementData.ID;
                        Debug.Assert(!Instance.m_InteractButtons.ContainsKey(id), $"Duplicate entry found for id '{id}'");
                        Instance.m_InteractButtons[id] = b;
                    }
                    break;
                case ElementInputField @if: {
                        var id = prefix + @if.ElementData.ID;
                        Debug.Assert(!Instance.m_InteractInputFields.ContainsKey(id), $"Duplicate entry found for id '{id}'");
                        Instance.m_InteractInputFields[id] = @if;
                    }
                    break;
                case ElementSlider s: {
                        var id = prefix + s.ElementData.ID;
                        Debug.Assert(!Instance.m_InteractSliders.ContainsKey(id), $"Duplicate entry found for id '{id}'");
                        Instance.m_InteractSliders[id] = s;
                    }
                    break;
                case ElementDropdown d: {
                        var id = prefix + d.ElementData.ID;
                        Debug.Assert(!Instance.m_InteractDropdowns.ContainsKey(id), $"Duplicate entry found for id '{id}'");
                        Instance.m_InteractDropdowns[id] = d;
                    }
                    break;
            }
        }

        private static void InnerUnregister<T>(T element, string prefix) where T : MonoBehaviour
        {
            switch (element) {
                case ElementCanvasGroup cg: Instance.m_InteractCanvasGroups.Remove(prefix + cg.ElementData.ID); break;
                case ElementText t: Instance.m_InteractTexts.Remove(prefix + t.ElementData.ID); break;
                case ElementImage i: Instance.m_InteractImages.Remove(prefix + i.ElementData.ID); break;
                case ElementToggle tg: Instance.m_InteractToggles.Remove(prefix + tg.ElementData.ID); break;
                case ElementDragDrop dd: Instance.m_InteractDragDrops.Remove(prefix + dd.ElementData.ID); break;
                case ElementButton b: Instance.m_InteractButtons.Remove(prefix + b.ElementData.ID); break;
                case ElementInputField @if: Instance.m_InteractInputFields.Remove(prefix + @if.ElementData.ID); break;
                case ElementSlider s: Instance.m_InteractSliders.Remove(prefix + s.ElementData.ID); break;
                case ElementDropdown d: Instance.m_InteractDropdowns.Remove(prefix + d.ElementData.ID); break;
            }
        }

        private static bool FromKeycode(IEnumerable<KeyCode> keyCodes)
        {
            foreach (var k in keyCodes)
                if (Input.GetKeyDown(k))
                    return true;
            return false;
        }

        internal static Vector3 CpyWithZ(Vector3 subject, float value)
        {
            var copy = subject;
            copy.z = value;
            return copy;
        }
    }
}
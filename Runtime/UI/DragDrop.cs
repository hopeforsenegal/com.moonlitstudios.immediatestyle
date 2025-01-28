using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI
{
    // |Special caveat compared to the other UI elements|
    // Instead of rendering, this is about simulation.
    // So frames where this isn't called means that the Gameobject doesn't follow the cursor
    //      So to make it drag and stay in place called PinnedPosition = dragged.transform.position every frame
    //      Where as to make it drag and snap back don't update the PinnedPosition
    //      To not have it drag at all simply do not call this method

    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Graphic))]
    public class DragDrop : MonoBehaviour
        , IPointerDownHandler
        , IPointerUpHandler
        , IBeginDragHandler
        , IDragHandler
    {
        public UnityEvent<DragDrop> OnReleased;
        public Vector3 PinnedPosition { get; set; }

        public RectTransform RectTransform { get; private set; }
        public bool IsMouseDown { get; private set; }
        public bool IsMouseUp { get; private set; }
        public bool IsDragging { get; private set; }
        // As long as we are enabled anytime the mouse is over us we are considered hovering. Why wouldn't we be? (this is even true with ui in front of us)
        public bool IsMouseHovering { get; private set; }

        protected void Start()
        {
            PinnedPosition = transform.position;
            RectTransform = GetComponent<RectTransform>();
        }

        protected void Update()
        {
#if ENABLE_INPUT_SYSTEM
            var mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#else
            var mousePosition = Input.mousePosition;
#endif

            if (IsDragging) {
                var v3 = Immediate.ImmediateStyle.CpyWithZ(mousePosition, 10f);
                transform.position = v3;
                // In order to appear over other UI elements it must be the last sibling. So plan accordingly                
                transform.SetAsLastSibling();
            }
            IsMouseHovering = RectTransformUtility.RectangleContainsScreenPoint(RectTransform, mousePosition);
        }

        protected void LateUpdate()
        {
            IsMouseDown = false;
            IsMouseUp = false;
        }

        public void OnPointerDown(PointerEventData _)
        {
            IsMouseDown = true;
        }

        public void OnPointerUp(PointerEventData _)
        {
            IsMouseUp = true;
            if (IsDragging) {
                OnReleased?.Invoke(this);
                IsDragging = false;
            }
        }

        public void OnBeginDrag(PointerEventData data)
        {
            IsDragging = true;
        }

        public void OnDrag(PointerEventData _)
        {
            // Needed to properly get all dragging events
        }
    }
}
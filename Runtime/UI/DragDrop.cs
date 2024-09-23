using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitSystem.UI
{
    // In order to appear over other UI elements it must be the last sibling. So plan accordingly
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
        // We expect this behavior if we were a standard Unity component. However ImmediateStyle defaults to overriding this to be off.
        // See 'ImmediateStyleProjectSettings.followCursorRetained' for more details and/or to change this for ImmediateStyle.
        internal bool FollowMouseCursor = true;
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
            if (FollowMouseCursor && IsDragging) {
                var v3 = Immediate.ImmediateStyle.CpyWithZ(Input.mousePosition, 10f);
                transform.position = v3;
                // In order to appear over other UI elements it must be the last sibling. So plan accordingly                
                transform.SetAsLastSibling();
            }
            IsMouseHovering = RectTransformUtility.RectangleContainsScreenPoint(RectTransform, Input.mousePosition);
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
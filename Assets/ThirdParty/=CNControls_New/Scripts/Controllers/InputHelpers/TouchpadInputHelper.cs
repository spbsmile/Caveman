using UnityEngine;

namespace CnControls
{
    public class TouchpadInputHelper : BaseInputHelper
    {
        /// <summary>
        /// Reference to the joystick which we help
        /// </summary>
        private Touchpad _linkedTouchpad;
        private RectTransform _touchpadTouchRect;

        protected override void Awake()
        {
            base.Awake();

            _linkedTouchpad = GetComponent<Touchpad>();
            _linkedTouchpad.CurrentEventCamera = UiRootCamera;
            _touchpadTouchRect = _linkedTouchpad.GetComponent<RectTransform>();
        }

        public void Update()
        {
            // Now this is a bit tricky
            // As we are completely REPLACING the uGUI event system for the Editor, we need to handle both Touch and Mouse inputs

            // For every touch out there
            for (int i = 0; i < CnInputManager.TouchCount; i++)
            {
                var touch = CnInputManager.GetTouch(i);
                PointerEventDataCache.position = touch.position;
                PointerEventDataCache.delta = touch.deltaPosition;

                // Check if it's inside our rectangle
                if (RectTransformUtility.RectangleContainsScreenPoint(_touchpadTouchRect, touch.position, UiRootCamera))
                {
                    // If it's inside, it's just started AND the joystick is not being tweaked yet
                    if (touch.phase == TouchPhase.Began && LastFingerId == -1)
                    {
                        // We press the joystick
                        _linkedTouchpad.OnPointerDown(PointerEventDataCache);
                        // Remember our pressed finger id
                        LastFingerId = touch.fingerId;
                        return;
                    }
                }

                // If it's just been lifted AND this is the finger that was tweaking this joystick
                if (touch.phase == TouchPhase.Ended && touch.fingerId == LastFingerId)
                {
                    // We release the joystick
                    _linkedTouchpad.OnPointerUp(PointerEventDataCache);
                    // Reset finger ID so we can Press again
                    LastFingerId = -1;
                    return;
                }

                if (touch.phase == TouchPhase.Moved && touch.fingerId == LastFingerId)
                {
                    _linkedTouchpad.OnDrag(PointerEventDataCache);
                    return;
                }
            }

            // Mouse input here
            // Same logic, but mouse is considered to be the finger with id of 255 so it's definitely won't interfere with actual fingers
            PointerEventDataCache.position = Input.mousePosition;
            PointerEventDataCache.delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            PointerEventDataCache.delta *= 10f; 
            if (RectTransformUtility.RectangleContainsScreenPoint(_touchpadTouchRect,
                PointerEventDataCache.position, UiRootCamera))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _linkedTouchpad.OnPointerDown(PointerEventDataCache);
                    LastFingerId = 255;
                    return;
                }
            }

            if (Input.GetMouseButtonUp(0) && LastFingerId == 255)
            {
                _linkedTouchpad.OnPointerUp(PointerEventDataCache);
                LastFingerId = -1;
                return;
            }

            if (Input.GetMouseButton(0) && LastFingerId == 255 && PointerEventDataCache.delta.sqrMagnitude > 0.000000001f)
            {
                _linkedTouchpad.OnDrag(PointerEventDataCache);
            }
        }
    }
}
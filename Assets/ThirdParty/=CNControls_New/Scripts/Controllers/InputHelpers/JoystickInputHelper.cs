// Make sure we don't compile it in builds
// It's only needed for the Editor and Unity Remote
#if UNITY_EDITOR

using UnityEngine;

namespace CnControls
{
    /// <summary>
    /// Helper class that contains additional logic needed for the In-Editor multitouch (Unity Remote)
    /// Can be ommited when the Unity Remote system multitouch will be fixed
    /// </summary>
    public class JoystickInputHelper : BaseInputHelper
    {
        /// <summary>
        /// Reference to the joystick which we help
        /// </summary>
        private SimpleJoystick _linkedJoystick;

        protected override void Awake()
        {
            base.Awake();

            _linkedJoystick = GetComponent<SimpleJoystick>();
            _linkedJoystick.CurrentEventCamera = UiRootCamera;
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

                // Check if it's inside our rectangle
                if (RectTransformUtility.RectangleContainsScreenPoint(_linkedJoystick.TouchZone, touch.position, UiRootCamera))
                {
                    // If it's inside, it's just started AND the joystick is not being tweaked yet
                    if (touch.phase == TouchPhase.Began && LastFingerId == -1)
                    {
                        // We press the joystick
                        _linkedJoystick.OnPointerDown(PointerEventDataCache);
                        // Remember our pressed finger id
                        LastFingerId = touch.fingerId;
                        return;
                    }
                }

                // If it's just been lifted AND this is the finger that was tweaking this joystick
                if (touch.phase == TouchPhase.Ended && touch.fingerId == LastFingerId)
                {
                    // We release the joystick
                    _linkedJoystick.OnPointerUp(PointerEventDataCache);
                    // Reset finger ID so we can Press again
                    LastFingerId = -1;
                    return;
                }

                if (touch.phase == TouchPhase.Moved && touch.fingerId == LastFingerId)
                {
                    _linkedJoystick.OnDrag(PointerEventDataCache);
                    return;
                }
            }

            // Mouse input here
            // Same logic, but mouse is considered to be the finger with id of 255 so it's definitely won't interfere with actual fingers
            PointerEventDataCache.position = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(_linkedJoystick.TouchZone,
                PointerEventDataCache.position, UiRootCamera))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _linkedJoystick.OnPointerDown(PointerEventDataCache);
                    LastFingerId = 255;
                    return;
                }
            }

            if (Input.GetMouseButtonUp(0) && LastFingerId == 255)
            {
                _linkedJoystick.OnPointerUp(PointerEventDataCache);
                LastFingerId = -1;
                return;
            }

            if (Input.GetMouseButton(0) && LastFingerId == 255)
            {
                _linkedJoystick.OnDrag(PointerEventDataCache);
            }
        }
    }
}

#endif
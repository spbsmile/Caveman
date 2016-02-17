// Make sure we don't compile it in builds
// It's only needed for the Editor and Unity Remote
#if UNITY_EDITOR

using UnityEngine;

namespace CnControls
{
    /// <summary>
    /// Helper class that contains additional logic needed for the In-Editor multitouch (Unity Remote)
    /// Can be ommited when the Unity Remote system multitouch will be fixed
    /// 
    /// It shoud REPLACE the standard uGUI event system for these controls, it's not intended to work with both event systems
    /// Again, it's only for the editor for Unity Remote to work, it should be disabled in builds
    /// </summary>
    public class ButtonInputHelper : BaseInputHelper
    {
        /// <summary>
        /// Reference to the button which we help
        /// </summary>
        private SimpleButton _linkedButton;

        /// <summary>
        /// Cached transform component of the linked button
        /// </summary>
        private RectTransform _linkedButtonRectTransform;

        protected override void Awake()
        {
            base.Awake();

            _linkedButton = GetComponent<SimpleButton>();
            _linkedButtonRectTransform = _linkedButton.GetComponent<RectTransform>();
        }

        private void Update()
        {
            // Now this is a bit tricky
            // As we are completely REPLACING the uGUI event system for the Editor, we need to handle both Touch and Mouse inputs

            // For every touch out there
            for (int i = 0; i < CnInputManager.TouchCount; i++)
            {
                var touch = CnInputManager.GetTouch(i);
                PointerEventDataCache.position = touch.position;

                // Check if it's inside our rectangle
                if (RectTransformUtility.RectangleContainsScreenPoint(_linkedButtonRectTransform, touch.position, UiRootCamera))
                {
                    // If it's inside, it's just started AND the button has not been pressed yet
                    if (touch.phase == TouchPhase.Began && LastFingerId == -1)
                    {
                        // We press the button
                        _linkedButton.OnPointerDown(PointerEventDataCache);
                        // Remember our pressed finger id
                        LastFingerId = touch.fingerId;
                        return;
                    }
                }

                // If it's just been lifted AND this is the finger that was pressing this button
                if (touch.phase == TouchPhase.Ended && touch.fingerId == LastFingerId)
                {
                    // We release the button
                    _linkedButton.OnPointerUp(PointerEventDataCache);
                    // Reset finger ID so we can Press again
                    LastFingerId = -1;
                    return;
                }
            }

            // Mouse input here
            // Same logic, but mouse is considered to be the finger with id of 255 so it's definitely won't interfere with actual fingers
            PointerEventDataCache.position = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(_linkedButtonRectTransform,
                PointerEventDataCache.position, UiRootCamera))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _linkedButton.OnPointerDown(PointerEventDataCache);
                    LastFingerId = 255;
                    return;
                }

            }

            if (Input.GetMouseButtonUp(0) && LastFingerId == 255)
            {
                _linkedButton.OnPointerUp(PointerEventDataCache);
                LastFingerId = -1;
            }
        }
    }
}

#endif
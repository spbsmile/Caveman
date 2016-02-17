using UnityEngine;

namespace CnControls
{
    /// <summary>
    /// Helper class that contains additional logic needed for the In-Editor multitouch (Unity Remote)
    /// Can be ommited when the Unity Remote system multitouch will be fixed
    /// </summary>
    public class DpadInputHelper : BaseInputHelper
    {
        private Dpad _linkedDpad;

        protected override void Awake()
        {
            base.Awake();

            _linkedDpad = GetComponent<Dpad>();
            _linkedDpad.CurrentEventCamera = UiRootCamera;
        }

        private void Update()
        {
            // For every touch out there
            for (int i = 0; i < CnInputManager.TouchCount; i++)
            {
                var touch = CnInputManager.GetTouch(i);
                PointerEventDataCache.position = touch.position;
                PointerEventDataCache.pointerId = touch.fingerId;

                if (touch.phase == TouchPhase.Began)
                {
                    _linkedDpad.OnPointerDown(PointerEventDataCache);
                    return;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    _linkedDpad.OnPointerUp(PointerEventDataCache);
                    return;
                }

            }
            // Mouse input here
            // Same logic, but mouse is considered to be the finger with id of 255 so it's definitely won't interfere with actual fingers
            PointerEventDataCache.position = Input.mousePosition;
            PointerEventDataCache.pointerId = 255;

            if (Input.GetMouseButtonDown(0))
            {
                _linkedDpad.OnPointerDown(PointerEventDataCache);
                return;                
            }
            if (Input.GetMouseButtonUp(0))
            {
                _linkedDpad.OnPointerUp(PointerEventDataCache);
                return;
            }
        }
    }
}
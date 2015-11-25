using System;
using System.Reflection;
using UnityEngine;

namespace Caveman.UI.Battle
{
    [ExecuteInEditMode]
    public class Joystick : CNJoystick
    {
        protected int SecondFingerId { get; set; }

        public override void OnEnable()
        {
            SecondFingerId = -1;
            base.OnEnable();
        }

        protected override void Update()
        {
            if (SecondFingerId == -1)
            {
                int touchCount = Input.touchCount;
                for (int i = 0; i < touchCount; i++)
                {
                    Touch currentTouch = Input.GetTouch(i);
                    if (currentTouch.phase == TouchPhase.Began &&
                        IsTouchInZone(currentTouch.position))
                    {
                        SecondFingerId = currentTouch.fingerId;
                        break;
                    }
                }
            }

            base.Update();
        }

        protected override bool TweakIfNeeded()
        {
            if (!IsCurrentlyTweaking && SecondFingerId != -1)
            {
                Touch? touch = GetTouchByFingerId(SecondFingerId);
                SecondFingerId = -1;
                if (touch == null || touch.Value.phase == TouchPhase.Ended)
                {
                    ResetControlState();
                    return false;
                }
                TweakControl(touch.Value.position);
                return true;
            }

            return base.TweakIfNeeded();
        }

        protected override bool IsTouchCaptured(out Touch capturedTouch)
        {
            int touchCount = Input.touchCount;

            for (int i = 0; i < touchCount; i++)
            {
                Touch currentTouch = Input.GetTouch(i);
                if (IsTouchInZone(currentTouch.position))
                {
                    IsCurrentlyTweaking = true;
                    CurrentFingerId = currentTouch.fingerId;
                    OnFingerTouched();

                    capturedTouch = currentTouch;
                    return true;
                }
            }

            return base.IsTouchCaptured(out capturedTouch);
        }

        private bool IsTouchInZone(Vector2 touchPosition)
        {
            return CalculatedTouchZone.Contains(ParentCamera.ScreenToWorldPoint(touchPosition), false);
        }
    }
}
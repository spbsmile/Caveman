using UnityEngine;
using UnityEngine.EventSystems;

namespace CnControls
{
    /// <summary>
    /// Simple button class
    /// Handles press, hold and release, just like a normal button
    /// </summary>
    public class SimpleButton : MonoBehaviour
        // some weird stuff here
        // we have to support Unity Remote with Multi Touch (which is not currently supported with uGUI)
        // so we just completely override the input system for the Editor, making it behave like it would normally do in builds
#if !UNITY_EDITOR
        , IPointerUpHandler, IPointerDownHandler
#endif
    {
        /// <summary>
        /// The name of the button
        /// </summary>
        public string ButtonName = "Jump";

        /// <summary>
        /// Utility object that is registered in the system
        /// </summary>
        private VirtualButton _virtualButton;

        // Again some Unity Remote supporting stuff
        // if we are in the editor, add an input helper component
#if UNITY_EDITOR
        private void Awake()
        {
            gameObject.AddComponent<ButtonInputHelper>();
        }
#endif
        
        /// <summary>
        /// It's pretty simple here
        /// When we enable, we register our button in the input system
        /// </summary>
        private void OnEnable()
        {
            _virtualButton = _virtualButton ?? new VirtualButton(ButtonName);
            CnInputManager.RegisterVirtualButton(_virtualButton);
        }

        /// <summary>
        /// When we disable, we unregister our button
        /// </summary>
        private void OnDisable()
        {
            CnInputManager.UnregisterVirtualButton(_virtualButton);
        }

        /// <summary>
        /// uGUI Event system stuff
        /// It's also utilised by the editor input helper
        /// </summary>
        /// <param name="eventData">Data of the passed event</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            _virtualButton.Release();
        }

        /// <summary>
        /// uGUI Event system stuff
        /// It's also utilised by the editor input helper
        /// </summary>
        /// <param name="eventData">Data of the passed event</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            _virtualButton.Press();
        }
    }
}
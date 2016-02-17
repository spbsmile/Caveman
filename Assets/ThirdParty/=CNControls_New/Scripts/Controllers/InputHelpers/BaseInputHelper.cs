using UnityEngine;
using UnityEngine.EventSystems;

namespace CnControls
{
    public class BaseInputHelper : MonoBehaviour
    {
        /// <summary>
        /// Cached event data object, so we don't create a new one every time
        /// </summary>
        protected PointerEventData PointerEventDataCache;

        /// <summary>
        /// Some state retaining stuff, needed to keep dragging the control
        /// </summary>
        protected int LastFingerId = -1;

        /// <summary>
        /// Link to the UI Root camera (if any), for calculating the positions of the elements
        /// </summary>
        protected Camera UiRootCamera;

        protected virtual void Awake()
        {
            // Straightforward things happening here

            PointerEventDataCache = new PointerEventData(FindObjectOfType<EventSystem>());
            UiRootCamera = GetComponentInParent<Canvas>().worldCamera;
        }
    }
}
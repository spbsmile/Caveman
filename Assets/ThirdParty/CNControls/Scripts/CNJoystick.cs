using UnityEngine;

// Common Joystick control
// There're lots of these, you know
[ExecuteInEditMode]
public class CNJoystick : CNAbstractController
{
    // ---------------------------------
    // Editor visible public properties
    // ---------------------------------
    /// <summary>
    /// Drag radius is a maximum distance on which you can drag the stick relative to the center of the base
    /// </summary>
    public float DragRadius { get { return _dragRadius; } set { _dragRadius = value; } }
    /// <summary>
    /// Indicates whether the joystick should "Snap" to the finger, placing itself on the touch position
    /// </summary>
    public bool SnapsToFinger { get { return _snapsToFinger; } set { _snapsToFinger = value; } }
    /// <summary>
    /// Indicates whether it should disappear when it's not being tweaked
    /// </summary>
    public bool IsHiddenIfNotTweaking { get { return _isHiddenIfNotTweaking; } set { _isHiddenIfNotTweaking = value; } }

    // Serialized fields (user preferences)
    // We also hide them in the inspector so it's not shown automatically
    [SerializeField]
    [HideInInspector]
    private float _dragRadius = 1.5f;
    [SerializeField]
    [HideInInspector]
    private bool _snapsToFinger = true;
    [SerializeField]
    [HideInInspector]
    private bool _isHiddenIfNotTweaking;

    // Runtime used fields
    /// <summary>
    /// Transform component of a stick
    /// </summary>
    private Transform _stickTransform;
    /// <summary>
    /// Transform component of a base
    /// </summary>
    private Transform _baseTransform;
    /// <summary>
    /// GameObject of a stick. Used for hiding
    /// </summary>
    private GameObject _stickGameObject;
    /// <summary>
    /// GameObject of a stick. Used for hiding
    /// </summary>
    private GameObject _baseGameObject;

    /// <summary>
    /// Neat initialization method
    /// </summary>
    public override void OnEnable()
    {
        base.OnEnable();

        // Getting needed components
        // Hardcoded names. We have no need of renaming these objects anyway
        _stickTransform = TransformCache.FindChild("Stick").GetComponent<Transform>();
        _baseTransform = TransformCache.FindChild("Base").GetComponent<Transform>();

        _stickGameObject = _stickTransform.gameObject;
        _baseGameObject = _baseTransform.gameObject;

        // Initial hiding of we should hide it
        if (IsHiddenIfNotTweaking)
        {
            _baseGameObject.gameObject.SetActive(false);
            _stickGameObject.gameObject.SetActive(false);
        }
        else
        {
            _baseGameObject.gameObject.SetActive(true);
            _stickGameObject.gameObject.SetActive(true);
        }
        
    }

    /// <summary>
    /// In this method we also need to set the stick and base local transforms back to zero
    /// </summary>
    protected override void ResetControlState()
    {
        base.ResetControlState();
        // Setting the stick and base local positions back to local zero
        _stickTransform.localPosition = 
            _baseTransform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// We also check if we should hide the joystick
    /// </summary>
    protected override void OnFingerLifted()
    {
        base.OnFingerLifted();
        if (!IsHiddenIfNotTweaking) return;

        _baseGameObject.gameObject.SetActive(false);
        _stickGameObject.gameObject.SetActive(false);
    }

    /// <summary>
    /// We also check if we should show the joystick
    /// </summary>
    protected override void OnFingerTouched()
    {
        base.OnFingerTouched();
        if (!IsHiddenIfNotTweaking) return;

        _baseGameObject.gameObject.SetActive(true);
        _stickGameObject.gameObject.SetActive(true);
    }

    /// <summary>
    /// Your favorite Update method where all the magic happens
    /// </summary>
    protected virtual void Update()
    {
        // Check for touches
        if (TweakIfNeeded())
                return;

        Touch currentTouch;
        if (IsTouchCaptured(out currentTouch))
            // Place joystick under the finger 
            // "No jumping" logic is also in this method
            PlaceJoystickBaseUnderTheFinger(currentTouch);
    }

    /// <summary>
    /// Function for joystick tweaking (moving with the finger)
    /// The values of the Axis are also calculated here
    /// </summary>
    /// <param name="touchPosition">Current touch position in screen cooridnates (pixels)
    /// It's recalculated in units so it's resolution-independent</param>
    protected override void TweakControl(Vector2 touchPosition)
    {
        // First, let's find our current touch position in world space
        Vector3 worldTouchPosition = ParentCamera.ScreenToWorldPoint(touchPosition);

        // Now we need to find a directional vector from the center of the joystick
        // to the touch position
        Vector3 differenceVector = (worldTouchPosition - _baseTransform.position);

        // If we're out of the drag range
        if (differenceVector.sqrMagnitude >
            DragRadius * DragRadius)
        {
            // Normalize this directional vector
            differenceVector.Normalize();

            //  And place the stick to it's extremum position
            _stickTransform.position = _baseTransform.position +
                differenceVector * DragRadius;
        }
        else
        {
            // If we're inside the drag range, just place it under the finger
            _stickTransform.position = worldTouchPosition;
        }

        // Store calculated axis values to our private variable
        CurrentAxisValues = differenceVector;

        // We also fire our event if there are subscribers
        OnControllerMoved(differenceVector);
    }

    /// <summary>
    /// Snap the joystick under the finger if it's expected
    /// </summary>
    /// <param name="touch">Current touch position in screen pixels
    /// It converts pixels to world space coordinates so it's resolution independent</param>
    protected virtual void PlaceJoystickBaseUnderTheFinger(Touch touch)
    {
        if (!_snapsToFinger) return;

        _stickTransform.position = 
            _baseTransform.position = ParentCamera.ScreenToWorldPoint(touch.position);
    }

}

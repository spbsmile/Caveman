using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

/// <summary>
/// Abstract controller class
/// It has all the functionality needed to implement your own controls
/// It also has a lot of commentary so you won't get missed
/// 
/// Also chech the CNAbstractControllerInspector.cs class in the Editor folder, as it uses a custom inspector
/// </summary>
[Serializable]
public abstract class CNAbstractController : MonoBehaviour
{
    // Constants for optimization. We don't need separate strings for every control object
    private const string AxisNameHorizontal = "Horizontal";
    private const string AxisNameVertical = "Vertical";

    // Some neat bitwise enums
    [Flags]
    protected enum AnchorsBase
    {
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8,
    }

    // Combined enums
    public enum Anchors
    {
        LeftTop = AnchorsBase.Left | AnchorsBase.Top,
        LeftBottom = AnchorsBase.Left | AnchorsBase.Bottom,
        RightTop = AnchorsBase.Right | AnchorsBase.Top,
        RightBottom = AnchorsBase.Right | AnchorsBase.Bottom
    }

    // --------------------------------
    // Editor visible public properties
    // --------------------------------

    /// <summary>
    /// Anchor is a place where the controls snap
    /// </summary>
    public Anchors Anchor { get { return _anchor; } set { _anchor = value; } }
    /// <summary>
    /// Axis name which is used with the GetAxis(..) method. It's just like Input.GetAxis(..)
    /// </summary>
    public string AxisNameX { get { return _axisNameX; } set { _axisNameX = value; } }
    /// <summary>
    /// Axis name which is used with the GetAxis(..) method. It's just like Input.GetAxis(..)
    /// </summary>
    public string AxisNameY { get { return _axisNameY; } set { _axisNameY = value; } }
    /// <summary>
    /// Margins set the distance to the screen borders in units. Resolution-independent
    /// </summary>
    public Vector2 Margins { get { return _margins; } set { _margins = value; } }
    
    // TODO: check whether different touch zones intersect (it can be avoided manually, but little things matter)
    /// <summary>
    ///  Touch zone size indicates how big is the sensitive area of the control
    /// </summary>
    public Vector2 TouchZoneSize { get { return _touchZoneSize; } set { _touchZoneSize = value; } }

    // -------------------
    // Event based control
    // -------------------

    /// <summary>
    /// Fires when the user tweaks the control
    /// </summary>
    public event Action<Vector3, CNAbstractController> ControllerMovedEvent;
    /// <summary>
    /// Fires when the user has just touched the control (the control became active)
    /// </summary>
    public event Action<CNAbstractController> FingerTouchedEvent;
    /// <summary>
    /// Fires when the user has just abandoned the control (the control became inactive)
    /// </summary>
    public event Action<CNAbstractController> FingerLiftedEvent;

    /// <summary>
    /// Simple Transform property, used in runtime, it's more fast than getting the .transform property
    /// </summary>
    protected Transform TransformCache { get; set; }
    /// <summary>
    /// Parent camera is an Orthographical camera where all CNControls are stored 
    /// </summary>
    protected Camera ParentCamera { get; set; }
    /// <summary>
    /// Runtime calculated Rect, used for touch position checks
    /// </summary>
    protected Rect CalculatedTouchZone { get; set; }
    /// <summary>
    /// Pretty self-explanatory
    /// </summary>
    protected Vector2 CurrentAxisValues { get; set; }
    /// <summary>
    /// Current captured finger ID
    /// </summary>
    protected int CurrentFingerId { get; set; }
    /// <summary>
    /// Nullable Vector3 for optimization. We can check if we've already found it
    /// </summary>
    protected Vector3? CalculatedPosition { get; set; }
    /// <summary>
    /// Whether the control is currently being tweaked
    /// </summary>
    protected bool IsCurrentlyTweaking { get; set; }

    // --------------
    // Private fields
    // We serialize them and hide them in the inspector
    // so it won't show automatically (we use custom inspectors for the CN Controls)
    // --------------

    [SerializeField]
    [HideInInspector]
    private Anchors _anchor = Anchors.LeftBottom;

    [SerializeField]
    [HideInInspector]
    private string _axisNameX = AxisNameHorizontal;

    [SerializeField]
    [HideInInspector]
    private string _axisNameY = AxisNameVertical;

    [SerializeField]
    [HideInInspector]
    private Vector2 _touchZoneSize = new Vector2(6f, 6f);

    [SerializeField]
    [HideInInspector]
    private Vector2 _margins = new Vector2(3f, 3f);

    /// <summary>
    /// Common method for getting CurrentAxisValues
    /// </summary>
    /// <param name="axisName">Name of the axis</param>
    /// <returns>Float value of a given Axis</returns>
    public virtual float GetAxis(string axisName)
    {
        // If we somehow leave axis names 
        if (AxisNameX == null || AxisNameY == null || AxisNameX == String.Empty || AxisNameY == String.Empty)
        {
            throw new UnityException("Input Axis " + axisName + " is not setup");
        }

        if (axisName == AxisNameX)
            return CurrentAxisValues.x;

        if (axisName == AxisNameY)
            return CurrentAxisValues.y;

        throw new UnityException("Input Axis " + axisName + " is not setup");
    }

    /// <summary>
    /// Call this method to temporarily disable the control
    /// It will also hide any control objects
    /// Override to change the behaviour
    /// </summary>
    public virtual void Disable()
    {
        CurrentAxisValues = Vector2.zero;

        gameObject.SetActive(false);
        // Unity defined MonoBehaviour property
        enabled = false;
    }

    /// <summary>
    /// Call this method to enable the control back
    /// It will then respond to Unity callbacks
    /// </summary>
    public virtual void Enable()
    {
        gameObject.SetActive(true);
        // Unity defined MonoBehaviour property
        enabled = true;
    }

    /// <summary>
    /// Neat initialization method
    /// </summary>
    public virtual void OnEnable()
    {
        TransformCache = GetComponent<Transform>();

#if UNITY_EDITOR
        // If we've instantiated the prefab but haven't parented it to a camera
        // Editor only issue
        if (TransformCache.parent == null) return;
#endif

        ParentCamera = TransformCache.parent.GetComponent<Camera>();

        TransformCache.localPosition = InitializePosition();
    }

    /// <summary>
    /// Utility method, finds the touch by it's fingerID, which is often different from it's index in .touches
    /// </summary>
    /// <param name="fingerId">The fingerId to find touch for</param>
    /// <returns>null if no touch found, returns a Touch if it's found</returns>
    protected virtual Touch? GetTouchByFingerId(int fingerId)
    {
#if UNITY_EDITOR
        // If we're in the editor, we also take our mouse as input
        // Let's say it's fingerId is 255;
        if (fingerId == 255)
        {
            return ConstructTouchFromMouseInput();
        }
#endif

        int touchCount = Input.touchCount;

        for (int i = 0; i < touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (touch.fingerId == fingerId) return touch;
        }

        // If there's no Touch with the specified fingerId, return null
        return null;
    }

    /// <summary>
    /// Event "callback".
    /// We can't use events in the derived classes 
    /// So we should use this method instead
    /// </summary>
    /// <param name="input">Input to pass with an event</param>
    protected virtual void OnControllerMoved(Vector2 input)
    {
        if (ControllerMovedEvent != null)
            ControllerMovedEvent(input, this);
    }

    /// <summary>
    /// Event "callback".
    /// We can't use events in the derived classes 
    /// So we should use this method instead
    /// </summary>
    protected virtual void OnFingerTouched()
    {
        if (FingerTouchedEvent != null)
            FingerTouchedEvent(this);
    }

    /// <summary>
    /// Event "callback".
    /// We can't use events in the derived classes 
    /// So we should use this method instead
    /// </summary>
    protected virtual void OnFingerLifted()
    {
        if (FingerLiftedEvent != null)
            FingerLiftedEvent(this);
    }

    /// <summary>
    /// Calculates local position based on margins and anchor
    /// </summary>
    /// <returns>Calculated position</returns>
    protected Vector3 InitializePosition()
    {
#if !UNITY_EDITOR
        // If we're not in the editor, we don't need to recalculate it every time we call this method
        if (CalculatedPosition != null)
            return CalculatedPosition.Value;
#endif

#if UNITY_EDITOR
        // Editor error "handling"
        // Happens when you duplicate the joystick in the editor window
        // causes a bit of recursion, but it's ok, it will just try to calculate joystick position twice
        if (ParentCamera == null)
            OnEnable();
#endif
        // Camera based calculations (different aspect ratios)
        float halfHeight = ParentCamera.orthographicSize;
        float halfWidth = halfHeight * ParentCamera.aspect;

        var newPosition = new Vector3(0f, 0f, 0f);

        // Bitwise checks
        // Used to simplify the if - else branching
        if (((int)Anchor & (int)AnchorsBase.Left) != 0)
            newPosition.x = -halfWidth + Margins.x;
        else
            newPosition.x = halfWidth - Margins.x;

        if (((int)Anchor & (int)AnchorsBase.Top) != 0)
            newPosition.y = halfHeight - Margins.y;
        else
            newPosition.y = -halfHeight + Margins.y;

        // Now we can calculate the TouchZone position
        // It's visualizes as green gizmo in the scene view
        CalculatedTouchZone = new Rect(
            TransformCache.position.x - TouchZoneSize.x / 2f,
            TransformCache.position.y - TouchZoneSize.y / 2f,
            TouchZoneSize.x,
            TouchZoneSize.y);

        return newPosition;
    }

    /// <summary>
    /// Common method of resetting the control state and position
    /// It means that we've just lifted our finger
    /// </summary>
    protected virtual void ResetControlState()
    {
        // It's no longer tweaking
        IsCurrentlyTweaking = false;
        // Setting our inner axis values back to zero
        CurrentAxisValues = Vector2.zero;
        // Fire our FingerLiftedEvents
        OnFingerLifted();
    }

    /// <summary>
    /// Common tweak method
    /// Used to provide more encapsulation in the derived classes
    /// Automatically calls the TweakControl method
    /// </summary>
    /// <returns>Whether the Tweak has happend</returns>
    protected virtual bool TweakIfNeeded()
    {
        // Check for touches
        if (IsCurrentlyTweaking)
        {
            // Check this method, it also returns a mouse input if we're in the editor
            Touch? touch = GetTouchByFingerId(CurrentFingerId);
            if (touch == null || touch.Value.phase == TouchPhase.Ended)
            {
                ResetControlState();
                return false;
            }
            TweakControl(touch.Value.position);
            return true;
        }
        return false;
    }

    /// <summary>
    /// If the control is not being tweaked, we need to check
    /// if there's any touch that want's to use this control
    /// </summary>
    /// <param name="capturedTouch">Captured touch. 
    /// If no touch has been captured, this param has no sence
    /// Check for return value to see whether it was captured
    /// </param>
    /// <returns>Whether any touch was captured</returns>
    protected virtual bool IsTouchCaptured(out Touch capturedTouch)
    {
        // Some optimization things
        int touchCount = Input.touchCount;

#if UNITY_EDITOR
        // If we're in the editor, we add another touch to the list - the mouse cursor
        int actualTouchCount = touchCount;
        touchCount++;
#endif

        // For every touch out there
        for (int i = 0; i < touchCount; i++)
        {
#if UNITY_EDITOR
            // If we got all touches from Input.GetTouch, we need to feed a new touch based on mouse input
            // Check ConstructTouchFromMouseInput() method for more info
            Touch currentTouch = i >= actualTouchCount ? ConstructTouchFromMouseInput() : Input.GetTouch(i);
#else
            // God bless local variables of value types
            Touch currentTouch = Input.GetTouch(i);
#endif
            // Check if we're interested in this touch
            if (currentTouch.phase == TouchPhase.Began && IsTouchInZone(currentTouch.position))
            {
                // If we are, capture the touch and make it ours
                IsCurrentlyTweaking = true;
                // Store it's finger ID so we can find it later
                CurrentFingerId = currentTouch.fingerId;
                // Fire our FingerTouchedEvent
                OnFingerTouched();

                // Initializing OUT param
                capturedTouch = currentTouch;
                // We don't need to check other touches
                // We also captured our touch, so yes, we return true
                return true;
            }
        }

        // To satisfy the compiler. It shouldn't be used
        capturedTouch = new Touch();
        // We didn't capture any touch
        return false;
    }

    /// <summary>
    /// Utility method, chechks whether the touch is inside the touch zone (green rect)
    /// </summary>
    /// <param name="touchPosition">Current touch position in screen pixels
    /// It converts these pixels to units, so it's totally resolution-independent
    /// </param>
    /// <returns>Whether it's inside of the touch zone</returns>
    private bool IsTouchInZone(Vector2 touchPosition)
    {
        return CalculatedTouchZone.Contains(ParentCamera.ScreenToWorldPoint(touchPosition), false);
    }

    /// <summary>
    /// Custom Tweaking method
    /// Implement it to achieve custom tweaking behaviour
    /// </summary>
    /// <param name="touchPosition">Current touch position in screen pixels
    /// It should convert these pixels to units, it should be totally resolution-independent</param>
    protected abstract void TweakControl(Vector2 touchPosition);

    // Some editor-only stuff. It won't compile to any of the builds
#if UNITY_EDITOR
    /// <summary>
    /// Your old DrawGizmosSelected method
    /// It allows you to change properties of the control in the inspector 
    /// - it will recalculate all needed properties
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        TransformCache = GetComponent<Transform>();
        // We don't need to recalculate the base position
        // Tweaking these things in Playmode won't persist anyway
        if (!EditorApplication.isPlaying)
            TransformCache.localPosition = InitializePosition();

        // Store the Gizmos color to restore everything back once we finish
        Color color = Gizmos.color;
        Gizmos.color = Color.green;

        // It's a local variable for more readability
        Vector3 localRectCenter = new Vector3(
                CalculatedTouchZone.x + CalculatedTouchZone.width / 2f,
                CalculatedTouchZone.y + CalculatedTouchZone.height / 2f,
                TransformCache.position.z);

        Gizmos.DrawWireCube(
            localRectCenter,
            new Vector3(TouchZoneSize.x, TouchZoneSize.y, 0f));

        // Perfect time to restore the original color back
        // It's rarely an issue though
        Gizmos.color = color;
    }

    /// <summary>
    /// Utility method. It gets current MouseInput (left mouse button) 
    /// and constructs a Touch with it
    /// </summary>
    /// <returns>Constructed touch</returns>
    private Touch ConstructTouchFromMouseInput()
    {
        // Boxing. Or any of the FieldInfo.SetValue won't work
        object mouseAsTouch = new Touch();

        // Some nasty Reflection stuff
        FieldInfo phaseFieldInfo = mouseAsTouch.GetType().
            GetField("m_Phase", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo positionFieldInfo = mouseAsTouch.GetType().
            GetField("m_Position", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo fingerIdFieldInfo = mouseAsTouch.GetType().
            GetField("m_FingerId", BindingFlags.NonPublic | BindingFlags.Instance);

        // Setting touch phase based on mouse button state
        if (Input.GetMouseButtonDown(0))
            phaseFieldInfo.SetValue(mouseAsTouch, TouchPhase.Began);
        else if (Input.GetMouseButtonUp(0))
            phaseFieldInfo.SetValue(mouseAsTouch, TouchPhase.Ended);
        else
            // We don't check if it's actually moved for simplicity, we don't use Moved / Stationary anyway
            phaseFieldInfo.SetValue(mouseAsTouch, TouchPhase.Moved);

        positionFieldInfo.SetValue(mouseAsTouch, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        // It has a hardcoded fingerId of 255
        fingerIdFieldInfo.SetValue(mouseAsTouch, 255);

        return (Touch)mouseAsTouch;
    }
#endif
}

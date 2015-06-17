#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

/// <summary>
/// Touchpad control. Much like in Dead Trigger 2 or Shadowgun
/// </summary>
public class CNTouchpad : CNAbstractController
{
    // -------------------------
    // Editor visible properties
    // -------------------------
    /// <summary>
    /// Set to true if you wan't to fully control the speed of the drag
    /// It will feel more responsive if set to FALSE
    /// </summary>
    public bool IsAlwaysNormalized { get { return _isAlwaysNormalized; } set { _isAlwaysNormalized = value; } }

    /// <summary>
    /// Indicates whether the touchpad should be stretched
    /// </summary>
    public bool IsStretched { get { return _isStretched; } set { _isStretched = value; } }

    // Serialized fields
    [SerializeField]
    [HideInInspector]
    private bool _isAlwaysNormalized = true;

    [SerializeField]
    [HideInInspector]
    private bool _isStretched = false;

    /// <summary>
    /// To find touch movement delta we need to store previous touch position
    /// It's stored in world coordinates to provide resolution invariance
    /// since different mobile devices have different DPI
    /// </summary>
    public Vector3 PreviousPosition { get; set; }

    public override void OnEnable()
    {
        base.OnEnable();

        if (IsStretched)
        {
            TransformCache.localPosition = CalculateStretchedSizeAndPosition();
        }
    }

    protected Vector3 CalculateStretchedSizeAndPosition()
    {
        float height = ParentCamera.orthographicSize * 2f;
        float width = ParentCamera.aspect * height;

        TouchZoneSize = new Vector2(width, height);

        TransformCache.localPosition = Vector3.zero;

        CalculatedTouchZone = new Rect(
            TransformCache.position.x - TouchZoneSize.x / 2f,
            TransformCache.position.y - TouchZoneSize.y / 2f,
            TouchZoneSize.x,
            TouchZoneSize.y);

        return Vector3.zero;
    }

    /// <summary>
    /// Automatically called by TweakIfNeeded
    /// </summary>
    /// <param name="touchPosition">Touch position in screen pixels</param>
    protected override void TweakControl(Vector2 touchPosition)
    {
        Vector3 worldPosition = ParentCamera.ScreenToWorldPoint(touchPosition);

        Vector3 difference = worldPosition - PreviousPosition;

        if (IsAlwaysNormalized)
            difference.Normalize();

        CurrentAxisValues = difference;

        OnControllerMoved(difference);

        PreviousPosition = worldPosition;
    }

    protected override void MoreUpdateLogic(Touch capturedTouch)
    {
        PreviousPosition = ParentCamera.ScreenToWorldPoint(capturedTouch.position);
    }

#if UNITY_EDITOR

    override protected void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (!EditorApplication.isPlaying && IsStretched)
        {
            TransformCache = GetComponent<Transform>();

            TransformCache.localPosition = CalculateStretchedSizeAndPosition();
        }
    }

#endif
}

using UnityEngine;

/// <summary>
/// Touchpad which you can "Throw" - if you release your finger in motion,
/// it will continue to fire it's OnControllerMoved event and gradually decrease it's CurrentAxisValues
/// Looks pretty cool
/// </summary>
public class CNThrowableTouchpad : CNTouchpad
{
    // -------------------------
    // Editor visible properties
    // -------------------------
    /// <summary>
    /// Sets the rate at which speed decays
    /// </summary>
    public float SpeedDecay { get { return _speedDecay; } set { _speedDecay = value; } }

    [SerializeField]
    [HideInInspector]
    private float _speedDecay = 0.9f;

    /// <summary>
    /// We override it because we don't need to set our CurrentAxisValues to zero
    /// </summary>
    protected override void ResetControlState()
    {
        IsCurrentlyTweaking = false;
        OnFingerLifted();
    }

    /// <summary>
    /// We just add this "drag" functionality to the original touchpad
    /// </summary>
    protected override void Update()
    {
        base.Update();

        // We have to cut it at some point
        if (CurrentAxisValues.sqrMagnitude <= 0.001f)
        {
            CurrentAxisValues = Vector2.zero;
            return;
        }
        
        // We reached the "Throw" code
        CurrentAxisValues *= SpeedDecay;
        OnControllerMoved(CurrentAxisValues);
    }
}

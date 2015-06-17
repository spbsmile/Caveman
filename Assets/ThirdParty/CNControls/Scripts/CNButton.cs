using System.Collections;
using UnityEngine;

public class CNButton : CNAbstractController
{
    // These flags are needed for the .GetButtonDown and .GetButtonUp functionality
    private bool _wasPressedThisFrame;
    private bool _wasReleasedThisFrame;

    /// <summary>
    /// "Clone" of Input.GetButtonDown
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public bool GetButtonDown(string buttonName)
    {
        return _wasPressedThisFrame;
    }

    /// <summary>
    /// "Clone" of Input.GetButtonUp
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public bool GetButtonUp(string buttonName)
    {
        return _wasReleasedThisFrame;
    }

    /// <summary>
    /// Additional logic for resetting the flags
    /// </summary>
    protected override void OnFingerTouched()
    {
        base.OnFingerTouched();

        CurrentAxisValues = Vector2.one;
        _wasPressedThisFrame = true;
        
        StartCoroutine(ResetFlags());
    }

    /// <summary>
    /// Some dirty magic here.
    /// Since we have our script executing before any other scripts
    /// We have to wait two frames to make sure we give any another script
    /// Our flags exactly one time
    /// 
    /// It may won't make sense at first, but if you try to grok it, it'll be fine
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetFlags()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _wasPressedThisFrame = false;
        _wasReleasedThisFrame = false;
    }

    /// <summary>
    /// Additional logic for resetting flags
    /// </summary>
    protected override void OnFingerLifted()
    {
        base.OnFingerLifted();

        CurrentAxisValues = Vector2.zero;
        _wasReleasedThisFrame = true;

        StartCoroutine(ResetFlags());
    }

    /// <summary>
    /// If we are pressing the button, it will continue to fire the event
    /// </summary>
    /// <param name="touchPosition">Input touch position. Doesn't matter, it's not used</param>
    protected override void TweakControl(Vector2 touchPosition)
    {
        OnControllerMoved(CurrentAxisValues);
    }

    protected override void MoreUpdateLogic(Touch capturedTouch)
    {
        // pass, we don't have any
    }
}

using UnityEngine;

public static class RteExtensionMethods
{
    /// <summary>
    /// With this method you can get the position of the UI object in different kind of coordinate systems.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="coordinateSystem"> The returned position will be from the coordinate system you specify in this parameter.</param>
    /// <param name="rtePivotCentered"> When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of this object, if this parameter is true then the pivot will be located at the center of this object.</param>
    public static Vector2 GetPosition(this RectTransform tr, CoordinateSystem coordinateSystem = CoordinateSystem.IgnoreAnchorsAndPivot, bool rtePivotCentered = false)
    {
        Vector2 result = RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr, rtePivotCentered);

        switch (coordinateSystem)
        {
            case CoordinateSystem.IgnoreAnchorsAndPivotNormalized:
                result = RteRectTools.GetPositionNormalizedIgnoringAnchorsAndPivot(tr, rtePivotCentered);
                break;

            case CoordinateSystem.AsChildOfCanvas:
                result = RteAnchorTools.GetCanvasAnchorCoordinateFromRectCoordinate(tr, result);
                break;

            case CoordinateSystem.AsChildOfCanvasNormalized:
                result = RteAnchorTools.GetCanvasAnchorCoordinateNormalizedFromRectCoordinate(tr, result);
                break;

            case CoordinateSystem.ScreenSpacePixels:
                result = RteAnchorTools.GetScreenSpaceCoordinateFromRectCoordinate(tr, RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr, rtePivotCentered));
                break;
        }       

        return result;
    }

    /// <summary>
    /// With this method you can set the position of the UI object in different kind of coordinate systems.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetPosition"> Your target position.</param>
    /// <param name="coordinateSystem"> The coordinate system of your target position.</param>
    /// <param name="rtePivotCentered"> When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of this object, if this parameter is true then the pivot will be located at the center of this object.</param>
    public static void SetPosition(this RectTransform tr, Vector2 targetPosition, CoordinateSystem coordinateSystem = CoordinateSystem.IgnoreAnchorsAndPivot, bool rtePivotCentered = false)
    {
        switch (coordinateSystem)
        {
            case CoordinateSystem.IgnoreAnchorsAndPivotNormalized:
                RteRectTools.SetPositionNormalizedIgnoringAnchorsAndPivot(tr, targetPosition, rtePivotCentered);
                return;

            case CoordinateSystem.AsChildOfCanvas:
                targetPosition = RteAnchorTools.GetRectCoordinateFromCanvasAnchorCoordinate(tr, targetPosition);
                break;

            case CoordinateSystem.AsChildOfCanvasNormalized:
                targetPosition = RteAnchorTools.GetRectCoordinateFromCanvasAnchorCoordinateNormalized(tr, targetPosition);
                break;

            case CoordinateSystem.ScreenSpacePixels:
                targetPosition = RteAnchorTools.GetRectCoordinateFromScreenSpaceCoordinate(tr, targetPosition);
                break;
        }

        RteRectTools.SetPositionIgnoringAnchorsAndPivot(tr, targetPosition, rtePivotCentered);
    }

    /// <summary>
    /// With this method you can set the X position of the UI object in different kind of coordinate systems.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetPositionX"></param>
    /// <param name="coordinateSystem">The coordinate system of your target position.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of this object, if this parameter is true then the pivot will be located at the center of this object.</param>
    public static void SetPositionX(this RectTransform tr, float targetPositionX, CoordinateSystem coordinateSystem = CoordinateSystem.IgnoreAnchorsAndPivot, bool rtePivotCentered = false)
    {
        Vector2 currentPos = tr.GetPosition(coordinateSystem, rtePivotCentered);
        tr.SetPosition(new Vector2(targetPositionX, currentPos.y), coordinateSystem, rtePivotCentered);
    }

    /// <summary>
    /// With this method you can set the Y position of the UI object in different kind of coordinate systems.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetPositionY"></param>
    /// <param name="coordinateSystem">The coordinate system of your target position.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of this object, if this parameter is true then the pivot will be located at the center of this object.</param>
    public static void SetPositionY(this RectTransform tr, float targetPositionY, CoordinateSystem coordinateSystem = CoordinateSystem.IgnoreAnchorsAndPivot, bool rtePivotCentered = false)
    {
        Vector2 currentPos = tr.GetPosition(coordinateSystem, rtePivotCentered);
        tr.SetPosition(new Vector2(currentPos.x, targetPositionY), coordinateSystem, rtePivotCentered);
    }

    /// <summary>
    /// With this method you can set the size of the UI object in different kind of coordinate systems. Returns the size as a Vector2.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="coordinateSystem">The returned size will be from the coordinate system you specify in this parameter.</param>
    public static Vector2 GetSize(this RectTransform tr, CoordinateSystem coordinateSystem = CoordinateSystem.IgnoreAnchorsAndPivot)
    {
        Vector2 result = Vector2.zero;

        switch (coordinateSystem)
        {
            case CoordinateSystem.IgnoreAnchorsAndPivot:
                result = RteRectTools.GetSizeIgnoringAnchors(tr);
                break;
            case CoordinateSystem.IgnoreAnchorsAndPivotNormalized:
                result = RteRectTools.GetSizeNormalizedIgnoringAnchors(tr);
                break;
            case CoordinateSystem.AsChildOfCanvas:
                result = RteAnchorTools.GetSizeInCanvasAnchorCoordinatesFromRectSize(tr);
                break;
            case CoordinateSystem.AsChildOfCanvasNormalized:
                result = RteAnchorTools.GetSizeNormalizedInCanvasAnchorCoordinatesFromRectSize(tr);
                break;
            case CoordinateSystem.ScreenSpacePixels:
                result = RteAnchorTools.GetScreenSpaceSizeFromRectSize(tr);
                break;
        }

        return result;
    }

    /// <summary>
    /// With this method you can set the size of the UI object in different kind of coordinate systems.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetSize"></param>
    /// <param name="coordinateSystem">The coordinate system of your target size.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of this object, if this parameter is true then the pivot will be located at the center of this object.</param>
    public static void SetSize(this RectTransform tr, Vector2 targetSize, CoordinateSystem coordinateSystem = CoordinateSystem.IgnoreAnchorsAndPivot, bool rtePivotCentered = false)
    {
        switch (coordinateSystem)
        {
            case CoordinateSystem.IgnoreAnchorsAndPivot:
                RteRectTools.SetSizeIgnoringAnchors(tr, targetSize, rtePivotCentered);
                break;

            case CoordinateSystem.IgnoreAnchorsAndPivotNormalized:
                RteRectTools.SetSizeNormalizedIgnoringAnchors(tr, targetSize, rtePivotCentered);
                break;

            case CoordinateSystem.AsChildOfCanvas:
                RteRectTools.SetSizeIgnoringAnchors(tr, RteAnchorTools.GetRectSizeFromCanvasAnchorCoordinatesSize(tr, targetSize), rtePivotCentered);
                break;

            case CoordinateSystem.AsChildOfCanvasNormalized:
                RteRectTools.SetSizeIgnoringAnchors(tr, RteAnchorTools.GetRectSizeFromCanvasAnchorCoordinatesSizeNormalized(tr, targetSize), rtePivotCentered);
                break;

            case CoordinateSystem.ScreenSpacePixels:
                RteRectTools.SetSizeIgnoringAnchors(tr, RteAnchorTools.GetRectSizeFromScreenSpaceSize(tr, targetSize), rtePivotCentered);
                break;
        }
    }

    /// <summary>
    /// With this method you can set the size of the UI object in different kind of coordinate systems.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetWidth"></param>
    /// <param name="coordinateSystem">The coordinate system of your target size.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of this object, if this parameter is true then the pivot will be located at the center of this object.</param>
    public static void SetWidth(this RectTransform tr, float targetWidth, CoordinateSystem coordinateSystem = CoordinateSystem.IgnoreAnchorsAndPivot, bool rtePivotCentered = false)
    {
        Vector2 currentSize = tr.GetSize(coordinateSystem);
        tr.SetSize(new Vector2(targetWidth, currentSize.y), coordinateSystem, rtePivotCentered);
    }

    /// <summary>
    /// With this method you can set the size of the UI object in different kind of coordinate systems.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetheight"></param>
    /// <param name="coordinateSystem">The coordinate system of your target size.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of this object, if this parameter is true then the pivot will be located at the center of this object.</param>
    public static void SetHeight(this RectTransform tr, float targetheight, CoordinateSystem coordinateSystem = CoordinateSystem.IgnoreAnchorsAndPivot, bool rtePivotCentered = false)
    {
        Vector2 currentSize = tr.GetSize(coordinateSystem);
        tr.SetSize(new Vector2(currentSize.x, targetheight), coordinateSystem, rtePivotCentered);
    }

    /// <summary>
    /// Get the anchors position. This is usefull to manipulate anchors position with only a single Vector2, specially usefull for tweens and clean code.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="coordinateSystem">The coordinate system you want the anchors position to be returned.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of the square formed by the anchors, if this parameter is true then the pivot will be located at the center of the square formed by the anchors.</param>
    public static Vector2 GetAnchorsPosition(this RectTransform tr, AnchorsCoordinateSystem coordinateSystem = AnchorsCoordinateSystem.Default, bool rtePivotCentered = false)
    {
        switch (coordinateSystem)
        {
            case AnchorsCoordinateSystem.Default:
                return RteAnchorTools.GetAnchorsPosition(tr, rtePivotCentered);
            case AnchorsCoordinateSystem.AsChildOfCanvas:
                return RteAnchorTools.GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, RteAnchorTools.GetAnchorsPosition(tr, rtePivotCentered));
            case AnchorsCoordinateSystem.ScreenSpacePixels:
                return  RteAnchorTools.GetScreenSpaceCoordinateFromAnchorCoordinate(tr, RteAnchorTools.GetAnchorsPosition(tr, rtePivotCentered));
            case AnchorsCoordinateSystem.AsRect:
                return RteAnchorTools.GetRectCoordinateFromAnchorCoordinate(tr, RteAnchorTools.GetAnchorsPosition(tr, rtePivotCentered));
            case AnchorsCoordinateSystem.InsideCanvas:
                return RteAnchorTools.GetInsideOfCanvasCoordinateFromAnchorCoordinate(tr, RteAnchorTools.GetAnchorsPosition(tr, rtePivotCentered));
            case AnchorsCoordinateSystem.OutsideCanvas:
                return RteAnchorTools.GetOutsideOfCanvasCoordinateFromAnchorCoordinate(tr, RteAnchorTools.GetAnchorsPosition(tr, rtePivotCentered));
            case AnchorsCoordinateSystem.OutsideContainer:
                return RteAnchorTools.GetOutsideOfContainerCoordinateFromAnchorCoordinate(tr, RteAnchorTools.GetAnchorsPosition(tr, rtePivotCentered));   
        }

        return RteAnchorTools.GetAnchorsPosition(tr, rtePivotCentered);
    }

    /// <summary>
    /// Move the anchors position. This is usefull to manipulate anchors position with only a single Vector2, specially usefull for tweens and clean code.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetPosition"></param>
    /// <param name="coordinateSystem">The coordinate system of the anchors position you are passing.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of the square formed by the anchors, if this parameter is true then the pivot will be located at the center of the square formed by the anchors.</param>
    /// <param name="alsoMoveTheRect">Select if you want to move the object with the anchors or only move the anchors. In most cases it's a good idea to move and resize images with thier anchors because everything becomes screen size independent.</param>
    public static void SetAnchorsPosition(this RectTransform tr, Vector2 targetPosition, AnchorsCoordinateSystem coordinateSystem = AnchorsCoordinateSystem.Default, bool rtePivotCentered = false, bool alsoMoveTheRect = true)
    {
        switch (coordinateSystem)
        {
            case AnchorsCoordinateSystem.Default:
                RteAnchorTools.SetAnchorsPosition(tr, targetPosition, rtePivotCentered, alsoMoveTheRect);
                break;
            case AnchorsCoordinateSystem.AsChildOfCanvas:
                RteAnchorTools.SetAnchorsPosition(tr, RteAnchorTools.GetAnchorCoordinateFromCanvasAnchorCoordinate(tr, targetPosition), rtePivotCentered, alsoMoveTheRect);
                break;
            case AnchorsCoordinateSystem.ScreenSpacePixels:
                RteAnchorTools.SetAnchorsPosition(tr, RteAnchorTools.GetAnchorCoordinateFromScreenSpaceCoordinate(tr, targetPosition), rtePivotCentered, alsoMoveTheRect);
                break;
            case AnchorsCoordinateSystem.AsRect:
                RteAnchorTools.SetAnchorsPosition(tr, RteAnchorTools.GetAnchorCoordianteFromRectCoordinate(tr, targetPosition), rtePivotCentered, alsoMoveTheRect);
                break;
            case AnchorsCoordinateSystem.InsideCanvas:
                RteAnchorTools.SetAnchorsPosition(tr, RteAnchorTools.GetAnchorCoordinateFromInsideOfCanvasCoordinate(tr, targetPosition), rtePivotCentered, alsoMoveTheRect);
                break;
            case AnchorsCoordinateSystem.OutsideCanvas:
                RteAnchorTools.SetAnchorsPosition(tr, RteAnchorTools.GetAnchorCoordinateFromOutsideOfCanvasCoordinate(tr, targetPosition), rtePivotCentered, alsoMoveTheRect);
                break;
            case AnchorsCoordinateSystem.OutsideContainer:
                RteAnchorTools.SetAnchorsPosition(tr, RteAnchorTools.GetAnchorCoordinateFromOutsideOfContainerCoordinate(tr, targetPosition), rtePivotCentered, alsoMoveTheRect);
                break;
        }
    }

    /// <summary>
    /// Move the anchors position in the X axis. This is usefull to manipulate anchors position with only a single float, specially usefull for tweens and clean code.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetPositionX"></param>
    /// <param name="coordinateSystem">The coordinate system of the anchors position you are passing.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of the square formed by the anchors, if this parameter is true then the pivot will be located at the center of the square formed by the anchors.</param>
    /// <param name="alsoMoveTheRect">Select if you want to move the object with the anchors or only move the anchors. In most cases it's a good idea to move and resize images with thier anchors because everything becomes screen size independent.</param>
    public static void SetAnchorsPositionX(this RectTransform tr, float targetPositionX, AnchorsCoordinateSystem coordinateSystem = AnchorsCoordinateSystem.Default, bool rtePivotCentered = false, bool alsoMoveTheRect = true)
    {
        Vector2 currentPos = tr.GetAnchorsPosition(coordinateSystem, rtePivotCentered);
        tr.SetAnchorsPosition(new Vector2(targetPositionX, currentPos.y), coordinateSystem, rtePivotCentered, alsoMoveTheRect);
    }

    /// <summary>
    /// Move the anchors position in the Y axis. This is usefull to manipulate anchors position with only a single float, specially usefull for tweens and clean code.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetPositionY"></param>
    /// <param name="coordinateSystem">The coordinate system of the anchors position you are passing.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of the square formed by the anchors, if this parameter is true then the pivot will be located at the center of the square formed by the anchors.</param>
    /// <param name="alsoMoveTheRect">Select if you want to move the object with the anchors or only move the anchors. In most cases it's a good idea to move and resize images with thier anchors because everything becomes screen size independent.</param>
    public static void SetAnchorsPositionY(this RectTransform tr, float targetPositionY, AnchorsCoordinateSystem coordinateSystem = AnchorsCoordinateSystem.Default, bool rtePivotCentered = false, bool alsoMoveTheRect = true)
    {
        Vector2 currentPos = tr.GetAnchorsPosition(coordinateSystem, rtePivotCentered);
        tr.SetAnchorsPosition(new Vector2(currentPos.x, targetPositionY), coordinateSystem, rtePivotCentered, alsoMoveTheRect);
    }

    /// <summary>
    /// A method get the anchors size. This is usefull to manipulate anchors size with only a single Vector2, specially usefull for tweens and clean code.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="coordinateSystem">The coordinate system you want the anchors size to be returned.</param>
    public static Vector2 GetAnchorsSize(this RectTransform tr, AnchorsCoordinateSystem coordinateSystem = AnchorsCoordinateSystem.Default)
    {
        switch (coordinateSystem)
        {
            case AnchorsCoordinateSystem.Default:
                return RteAnchorTools.GetAnchorsSize(tr);
            case AnchorsCoordinateSystem.AsChildOfCanvas:
                return RteAnchorTools.GetSizeInCanvasAnchorCoordinatesFromAnchorsSize(tr);
            case AnchorsCoordinateSystem.ScreenSpacePixels:
                return RteAnchorTools.GetScreenSpaceSizeFromAnchorsSize(tr);
            case AnchorsCoordinateSystem.AsRect:
                return RteAnchorTools.GetRectSizeFromAnchorSize(tr, RteAnchorTools.GetAnchorsSize(tr));
            case AnchorsCoordinateSystem.InsideCanvas:
                return RteAnchorTools.GetSizeInCanvasAnchorCoordinatesFromAnchorsSize(tr);
            case AnchorsCoordinateSystem.OutsideCanvas:
                return RteAnchorTools.GetSizeInCanvasAnchorCoordinatesFromAnchorsSize(tr);
            case AnchorsCoordinateSystem.OutsideContainer:
                return RteAnchorTools.GetAnchorsSize(tr);
        }

        return RteAnchorTools.GetAnchorsSize(tr);
    }

    /// <summary>
    /// A method to resize the anchors. This is usefull to manipulate anchors size with only a single Vector2, specially usefull for tweens and clean code.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetSize"></param>
    /// <param name="coordinateSystem">The coordinate system of the size you are passing.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of the square formed by the anchors, if this parameter is true then the pivot will be located at the center of the square formed by the anchors.</param>
    /// <param name="alsoResizeTheRect">Select if you want to resize the object with the anchors or only resize the anchors. In most cases it's a good idea to move and resize images with thier anchors because everything becomes screen size independent.</param>
    public static void SetAnchorsSize(this RectTransform tr, Vector2 targetSize, AnchorsCoordinateSystem coordinateSystem = AnchorsCoordinateSystem.Default, bool rtePivotCentered = false, bool alsoResizeTheRect = true)
    {
        switch (coordinateSystem)
        {
            case AnchorsCoordinateSystem.Default:
                RteAnchorTools.SetAnchorsSize(tr, targetSize, rtePivotCentered, alsoResizeTheRect);
            break;
            case AnchorsCoordinateSystem.AsChildOfCanvas:
                RteAnchorTools.SetAnchorsSize(tr, RteAnchorTools.GetAnchorsSizeFromCanvasAnchorCoordinatesSize(tr, targetSize), rtePivotCentered, alsoResizeTheRect);
                break;
            case AnchorsCoordinateSystem.ScreenSpacePixels:
                RteAnchorTools.SetAnchorsSize(tr, RteAnchorTools.GetAnchorsSizeFromScreenSpaceSize(tr, targetSize), rtePivotCentered, alsoResizeTheRect);
                break;
            case AnchorsCoordinateSystem.InsideCanvas:
                RteAnchorTools.SetAnchorsSize(tr, RteAnchorTools.GetAnchorsSizeFromCanvasAnchorCoordinatesSize(tr, targetSize), rtePivotCentered, alsoResizeTheRect);
                break;
            case AnchorsCoordinateSystem.AsRect:
                RteAnchorTools.SetAnchorsSize(tr, RteAnchorTools.GetAnchorSizeFromRectSize(tr, targetSize), rtePivotCentered, alsoResizeTheRect);
                break;
            case AnchorsCoordinateSystem.OutsideCanvas:
                RteAnchorTools.SetAnchorsSize(tr, RteAnchorTools.GetAnchorsSizeFromCanvasAnchorCoordinatesSize(tr, targetSize), rtePivotCentered, alsoResizeTheRect);
                break;
            case AnchorsCoordinateSystem.OutsideContainer:
                RteAnchorTools.SetAnchorsSize(tr, targetSize, rtePivotCentered, alsoResizeTheRect);
                break;
        }
    }

    /// <summary>
    /// A method to resize the width of the anchors. This is usefull to manipulate anchors size with only a single float, specially usefull for tweens and clean code.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetWidth"></param>
    /// <param name="coordinateSystem">The coordinate system of the size you are passing.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of the square formed by the anchors, if this parameter is true then the pivot will be located at the center of the square formed by the anchors.</param>
    /// <param name="alsoResizeTheRect">Select if you want to resize the object with the anchors or only resize the anchors. In most cases it's a good idea to move and resize images with thier anchors because everything becomes screen size independent.</param>
    public static void SetAnchorsWidth(this RectTransform tr, float targetWidth, AnchorsCoordinateSystem coordinateSystem = AnchorsCoordinateSystem.Default, bool rtePivotCentered = false, bool alsoResizeTheRect = true)
    {
        Vector2 currentSize = tr.GetAnchorsSize(coordinateSystem);
        tr.SetAnchorsSize(new Vector2(targetWidth, currentSize.y), coordinateSystem, rtePivotCentered, alsoResizeTheRect);
    }

    /// <summary>
    /// A method to resize the height of the anchors. This is usefull to manipulate anchors size with only a single float, specially usefull for tweens and clean code.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetHeight"></param>
    /// <param name="coordinateSystem">The coordinate system of the size you are passing.</param>
    /// <param name="rtePivotCentered">When using this tool the Unity pivot is ignored and a pivot from this tool is used, wich is located at the lower left corner of the square formed by the anchors, if this parameter is true then the pivot will be located at the center of the square formed by the anchors.</param>
    /// <param name="alsoResizeTheRect">Select if you want to resize the object with the anchors or only resize the anchors. In most cases it's a good idea to move and resize images with thier anchors because everything becomes screen size independent.</param>
    public static void SetAnchorsHeight(this RectTransform tr, float targetHeight, AnchorsCoordinateSystem coordinateSystem = AnchorsCoordinateSystem.Default, bool rtePivotCentered = false, bool alsoResizeTheRect = true)
    {
        Vector2 currentSize = tr.GetAnchorsSize(coordinateSystem);
        tr.SetAnchorsSize(new Vector2(currentSize.x, targetHeight), coordinateSystem, rtePivotCentered, alsoResizeTheRect);
    }
}

public enum CoordinateSystem
{
    /// <summary>
    /// Ignores anchors and instead of the Unity's pivot uses the Rect Transform Extended pivot wich is located at the lower 
    /// left corner by default. The result is the same with any anchor or Unity pivot position because they are ignored.
    /// This coordinate system allows you to control the UI object in the same way than other frameworks like CSS, Flash, etc.
    /// All the other coordinate systems of this method also ignores anchors and pivot.
    /// </summary>
    IgnoreAnchorsAndPivot,

    /// <summary>
    /// This is The same than IgnoreAnchorsAndPivot but using values from 0 to 1 (normalized). Ignores anchors and instead 
    /// of the Unity's pivot uses the Rect Transform Extended pivot wich is located at the lower left corner by default.
    /// The result is the same with any anchor or Unity pivot position because they are ignored.
    /// This coordinate system allows you to control the UI object in the same way than other frameworks like CSS, Flash, etc.
    /// Warning: Coordinate systems that goes from 0 to 1 includes a square aspect ratio information in the position values that 
    /// is probably different than the real aspect ratio, this means that rotation or distance between points calculations may 
    /// generate unexpected results.
    /// </summary>
    IgnoreAnchorsAndPivotNormalized,

    /// <summary>
    /// Move or resize the element as if it was a child of the canvas. This is useful for example to match coordinates 
    /// from objects located at different containers.
    /// </summary>
    AsChildOfCanvas,

    /// <summary>
    /// This is The same than AsChildOfCanvas but using values from 0 to 1 (normalized). Move or resize the element as if it was 
    /// a child of the canvas. This is useful for example to match coordinates from objects located at different containers.
    /// Warning: Coordinate systems that goes from 0 to 1 includes a square aspect ratio information in the position values that 
    /// is probably different than the real aspect ratio, this means that rotation or distance between points calculations may 
    /// generate unexpected results.
    /// </summary>
    AsChildOfCanvasNormalized,

    /// <summary>
    /// Screen pixels (screen space). Usefull to move or resize the UI object with mouse/touch coordinates wich came in the screen 
    /// space coordinate system. 
    /// </summary>
    ScreenSpacePixels
}

public enum AnchorsCoordinateSystem
{
    /// <summary>
    /// Coordinates from 0 to 1 used by anchors. The difference of using this instead of AnchorMin and AnchorMax is that with this tool you move the 
    /// anchors with a Vector2 and resize them with another Vector2 for the size. Otherwise if you use RectTransform's AnchorMin and AnchorMax vectors 
    /// you control the position of 2 corners of the "anchors square" so if you want to move the anchors without changing thier size you have to move 
    /// both Vectors istead of one. This is specially important when moving anchors with a tween animation because you use a single tween instead 
    /// of 2 tweens.
    /// </summary>
    Default,

    /// <summary>
    /// Move or resize the anchors as if the UI object was a child of the canvas.
    /// This is useful for example to match the position visually of 2 objects located at different containers.
    /// The position range goes from 0 to 1 like the usual for anchors. 
    /// </summary>
    AsChildOfCanvas,

    /// <summary>
    /// Pixels (screen space). Usefull to move or resize the anchors with mouse/touch coordinates wich came in the screen 
    /// space coordinate system. 
    /// </summary>
    ScreenSpacePixels,

    /// <summary>
    /// Move or resize the anchors with the same kind of coordinates used for the rect in the IgnoreAnchorsAndPivot coordinate system.
    /// </summary>
    AsRect,

    /// <summary>
    /// This is a 0 to 1 coordinate system. When you set 1 in the X axis the anchors are moved outside of the container to the right all the distance needed 
    /// to have the object outside of it's container, the same happens if you set 0 but for the other side. Useful to animate an object out of the container 
    /// without thinking about anchors, position, size or containers. There is no pivot in this coordinate system and only works for position, using this 
    /// with SetAnchorsSize is mapped to Default.
    /// </summary>
    OutsideContainer,

    /// <summary>
    /// This is a 0 to 1 coordinate system. When you set 1 in the X axis the anchors are moved outside of the canvas to the right all the distance needed
    /// to have the object outside of the screen, invisible. The same happens if you set 0 but for the other side. Useful to animate an object out of the 
    /// screen without thinking about anchors, position, size or containers. There is no pivot in this coordinate system and only works for position, using 
    /// this with SetAnchorsSize is mapped to AsChildOfCanvas.
    /// </summary>
    OutsideCanvas,

    /// <summary>
    /// This is a 0 to 1 coordinate system to move the element inside the canvas. For example when you set 1 in the X axis the element moves to the right 
    /// edge of the canvas without going outside, if you set 0 is the same but to the right. Useful to make a dragable object but prevent the object to go 
    /// outside of the screen. There is no pivot in this coordinate system and only works for position, using this with SetAnchorsSize is mapped to 
    /// AsChildOfCanvas.
    /// </summary>
    InsideCanvas
}

using System.Collections.Generic;
using UnityEngine;

public static class RteAnchorTools
{
    public static RectTransform GetCanvas(this RectTransform tr)
    {
        RectTransform current = tr;

        // Store all the containers of the object until the canvas is reached
        while (current != null && current.parent != current && current.parent is RectTransform)
        {
            current = (RectTransform)current.parent;
        }

        return current;
    }

    /// <summary>
    /// Get the anchors position. The intent of this function is manipulating anchors with only a single Vector2, specially 
    /// usefull for tweens and clean code.
    /// In most cases it's a good idea to move and resize images using anchors because everything becomes screen size independent.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="centeredPivot">If false the pivot is in the down left corner, if true the pivot is in the center</param>
    /// <returns></returns>
    public static Vector2 GetAnchorsPosition(RectTransform tr, bool centeredPivot = false)
    {
        if(!centeredPivot)
            return tr.anchorMin;

        Vector2 size = GetAnchorsSize(tr);
        return new Vector2(tr.anchorMin.x + size.x * 0.5f, tr.anchorMin.y + size.y * 0.5f);
    }

    /// <summary>
    /// Get the anchors size. This just returns tr.anchorMax - tr.anchorMin because size is determined by anchorMax.
    /// Note: If the anchors are placed together this returns (0,0).
    /// The aim of this function is manipulating anchors with only a single Vector2, specially usefull
    /// for tweens and clean code. In most cases it's a good idea to move and resize images using anchors because 
    /// everything becomes screen size independent.
    /// </summary>
    /// <param name="tr"></param>
    /// <returns></returns>
    public static Vector2 GetAnchorsSize(RectTransform tr)
    {
        return tr.anchorMax - tr.anchorMin;
    }

    /// <summary>
    /// Move the anchors, moving anchors also moves the image as you may know. This is the same than
    /// moving the anchors holding control + shift key in the Unity editor.
    /// To set the position of another object you must pass: SetPosition(tr.anchorMin.x, tr.anchorMin.y) because 
    /// these are the values that determines the anchors position of the object. For a more simple sintax use 
    /// SetPosition(targetTransform.GetPosition()) or SetPosition(targetTransform).
    /// The aim of this function is manipulating anchors with only a single Vector2, specially usefull
    /// for tweens and clean code. In most cases it's a good idea to move and resize images using anchors because 
    /// everything becomes screen size independent.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetPos"></param>
    /// <param name="centeredPivot">If false the pivot is in the down left corner, if true the pivot is in the center</param>
    public static void SetAnchorsPosition(RectTransform tr, Vector2 targetPos, bool centeredPivot = false, bool alsoChangeRect = true)
    {
        Vector2 rectPos  = Vector2.zero;
        if(!alsoChangeRect)
            rectPos  = RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr);

        Vector2 size = GetAnchorsSize(tr);
        
        if (centeredPivot)
            targetPos -= new Vector2(size.x * 0.5f, size.y * 0.5f);

        // anchorMin.x and anchorMix.y moves the object, the other 2 values just move the same amount (if the size is not changed).
        tr.anchorMin = targetPos;
        tr.anchorMax = targetPos + size;

        if(!alsoChangeRect)
            RteRectTools.SetPositionIgnoringAnchorsAndPivot(tr, rectPos);
    }

    /// <summary>
    /// Move the anchors, moving anchors also moves the image as you may know. This is the same than
    /// moving the anchors holding control + shift key in the Unity editor.
    /// To set the position of another object you must pass: SetPosition(tr.anchorMin.x, tr.anchorMin.y) because 
    /// these are the values that determines the anchors position. For a more simple sintax use 
    /// SetPosition(targetTransform.GetPosition()) or SetPosition(targetTransform).
    /// The aim of this function is manipulating anchors with only a single Vector2, specially usefull
    /// for tweens and clean code. In most cases it's a good idea to move and resize images using anchors because 
    /// everything becomes screen size independent.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetPosFromOtherObject">Copy the position from another RectTransform, can be an object inside another container or even another canvas, the position will be translated</param>
    public static void SetAnchorsPosition(RectTransform tr, RectTransform targetPosFromOtherObject)
    {
        SetAnchorsPosition
            (tr, GetAnchorCoordinateFromCanvasAnchorCoordinate
                (tr,
                    GetCanvasAnchorCoordinateFromAnchorCoordinate(targetPosFromOtherObject,
                        GetAnchorsPosition(targetPosFromOtherObject)
                    )
                )
           );
    }

    /// <summary>
    /// Resize the anchors, resizing anchors also resizes the image as you may know. This is the same than
    /// moving the anchors holding shift key in the Unity editor.
    /// To set the size of another object you must pass: SetSize(targetTransform.GetSize()) Or for a 
    /// more simple sintax use SetSize(targetRectTransform).
    /// The aim of this function is manipulating anchors with only a single Vector2, specially usefull
    /// for tweens and clean code. In most cases it's a good idea to move and resize images using anchors because 
    /// everything becomes screen size independent.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetSize"></param>
    public static void SetAnchorsSize(RectTransform tr, Vector2 targetSize, bool centeredPivot = false, bool alsoChangeRect = true)
    {
        Vector2 rectPos  = Vector2.zero;
        Vector2 rectSize = Vector2.zero;
        if(!alsoChangeRect)
        {
            rectPos  = RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr);
            rectSize = RteRectTools.GetSizeIgnoringAnchors(tr);
        }

        if(centeredPivot)
        {
            Vector2 sizeDif = targetSize - (tr.anchorMax - tr.anchorMin);
            Vector2 sizeDifHalf = new Vector2(sizeDif.x * 0.5f, sizeDif.y * 0.5f);
            tr.anchorMax -= sizeDifHalf;
            tr.anchorMin -= sizeDifHalf;
        }

        tr.anchorMax = tr.anchorMin + targetSize;

        if(!alsoChangeRect)
        {
            RteRectTools.SetSizeIgnoringAnchors(tr, rectSize, centeredPivot);
            RteRectTools.SetPositionIgnoringAnchorsAndPivot(tr, rectPos);
        }
    }

    /// <summary>
    /// Resize the anchors, resizing anchors also resizes the image as you may know. This is the same than
    /// moving the anchors holding shift key in the Unity editor.
    /// To set the size of another object you must pass: SetSize(targetTransform.GetSize()) Or for a 
    /// more simple sintax use SetSize(targetTransform).
    /// The aim of this function is manipulating anchors with only a single Vector2, specially usefull
    /// for tweens and clean code. In most cases it's a good idea to move and resize images using anchors because 
    /// everything becomes screen size independent.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="targetPosFromOtherObject">Copy the size from another RectTransform, can be an object inside another container or even another canvas, the position will be translated</param>
    public static void SetAnchorsSize(RectTransform tr, RectTransform targetPosFromOtherObject)
    {
        SetAnchorsSize
        (tr, GetAnchorCoordinateFromCanvasAnchorCoordinate(tr,
                GetCanvasAnchorCoordinateFromAnchorCoordinate(targetPosFromOtherObject,
                    GetAnchorsSize(targetPosFromOtherObject)
                )
            )
       );
    }

    /// <summary>
    /// Converts a screen space coordinate (pixels) to a range between 0 to 1 (used by the anchors), I call it "anchor space".
    /// An example: To make the anchors follow the mouse, place the anchors in the corners of the image and add this line of 
    /// code to the Update function of a component in the target UI object: transform.SetAnchorsPosition(transform.ScreenSpaceToAnchorSpace(Input.mousePosition));
    /// It's not a good idea to make a mouse pointer with this, but it's usefull in other cases like a dragable object.
    /// Note: This probably will not work as you expect when the canvas is configured as "Word Space" because everithing becomes 3D.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="screenSpacePoint">The screen space point (pixel) to convert to anchor space point at the level of this object</param>
    /// <returns></returns>
    public static Vector2 GetAnchorCoordinateFromScreenSpaceCoordinate(RectTransform tr, Vector2 screenSpacePoint)
    {
        // Convert screen space coordinates to 0..1 range, this alone is enought if the parent of the object is the canvas.
        var result = FixNaN(new Vector2(screenSpacePoint.x / Screen.width, screenSpacePoint.y / Screen.height));
        return GetAnchorCoordinateFromCanvasAnchorCoordinate(tr, result);
    }
    
    /// <summary>
    /// Converts a range between 0 to 1 (used by the anchors), I call it "anchor space" to a screen space coordinate (pixels).
    /// Note: This probably will not work as you expect when the canvas is configured as "Word Space" because everithing becomes 3D.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="anchorSpacePoint">The anchor space point at the level of this object to be converted to screen space coordinate point (pixel)</param>
    /// <returns></returns>
    public static Vector2 GetScreenSpaceCoordinateFromAnchorCoordinate(RectTransform tr, Vector2 anchorSpacePoint)
    {
        Vector2 result = GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, anchorSpacePoint);
        return new Vector2(Screen.width * result.x, Screen.height * result.y);      // convert 0..1 of the canvas to pixels.
    }

    public static Vector2 GetCanvasCoordinateFromScreenSpaceCoordinate(RectTransform tr, Vector2 screenSpaceCoordinate)
    {
        return FixNaN(new Vector2(screenSpaceCoordinate.x / Screen.width, screenSpaceCoordinate.y / Screen.height));      // convert 0..1 of the canvas to pixels.
    }

    public static Vector2 GetScreenSpaceCoordinateFromCanvasCoordinate(RectTransform tr, Vector2 canvasCoordinate)
    {
        return new Vector2(Screen.width * canvasCoordinate.x, Screen.height * canvasCoordinate.y);                // convert 0..1 of the canvas to pixels.
    }

    public static Vector2 GetAnchorsSizeFromScreenSpaceSize(RectTransform tr, Vector2 screenSpaceSize)
    {
        return GetAnchorsSizeFromCanvasAnchorCoordinatesSize(tr, GetCanvasCoordinateFromScreenSpaceCoordinate(tr, screenSpaceSize));
    }

    public static Vector2 GetScreenSpaceSizeFromAnchorsSize(RectTransform tr)
    {
        return GetScreenSpaceCoordinateFromCanvasCoordinate(tr, GetSizeInCanvasAnchorCoordinatesFromAnchorsSize(tr));
    }
    /// <summary>
    /// Converts a range between 0 to 1 (used by the anchors), I call it "anchor space" to a screen space coordinate (pixels).
    /// Note: This probably will not work as you expect when the canvas is configured as "Word Space" because everithing becomes 3D.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="anchorSpacePoint">The anchor space point at the level of this object to be converted to screen space coordinate point (pixel)</param>
    /// <returns></returns>
    public static Vector2 GetScreenSpaceCoordinateFromRectCoordinate(RectTransform tr, Vector2 rectCoordinate)
    {
        return GetScreenSpaceCoordinateFromAnchorCoordinate(tr, GetAnchorCoordianteFromRectCoordinate(tr, rectCoordinate));
    }

    /// <summary>
    /// Return the rect size as screen space pixels. It does not matter if the container is not the canvas or the anchors positios.
    /// </summary>
    /// <param name="tr"></param>
    public static Vector2 GetScreenSpaceSizeFromRectSize(RectTransform tr)
    {
        Vector2 position     = GetScreenSpaceCoordinateFromAnchorCoordinate(tr, GetAnchorCoordianteFromRectCoordinate(tr, RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr)));
        Vector2 topLeftPoint = GetScreenSpaceCoordinateFromAnchorCoordinate(tr, GetAnchorCoordianteFromRectCoordinate(tr, RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr) + RteRectTools.GetSizeIgnoringAnchors(tr)));
        return topLeftPoint - position;
    }


    /// <summary>
    /// Return the rect size from screen space pixels. It does not matter if the container is not the canvas or the anchors positios.
    /// </summary>
    /// <param name="tr"></param>
    public static Vector2 GetRectSizeFromScreenSpaceSize(RectTransform tr, Vector2 screenSpaceSize)
    {
        // Convert screen space coordinates to 0..1 range, this alone is enought if the parent of the object is the canvas.
        screenSpaceSize = FixNaN(new Vector2(screenSpaceSize.x / Screen.width, screenSpaceSize.y / Screen.height));
        screenSpaceSize = GetRectSizeFromCanvasAnchorCoordinatesSizeNormalized(tr, screenSpaceSize);
        return screenSpaceSize;
    }

    /// <summary>
    /// An anchor space point at the level of this object to be converted to an anchor space point at the level of the canvas.
    /// Example: If this object is inside a container which is a child of the canvas and is located at a position of (0.5, 0.5), this 
    /// method will return (0.5, 0.5) when you pass (0, 0).
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="anchorSpacePoint"></param>
    /// <returns></returns>
    public static Vector2 GetCanvasAnchorCoordinateFromAnchorCoordinate(RectTransform tr, Vector2 anchorSpacePoint)
    {
        List<RectTransform> containers = new List<RectTransform>();
        RectTransform current = tr;

        // Store all the containers of the object until the canvas is reached
        while (current != null && current.parent != current && current.parent is RectTransform)
        {
            if (current != tr)
                containers.Add(current);

            current = (RectTransform)current.parent;
        }

        Vector2 result = anchorSpacePoint;

        // The target object can be inside one or more containers so we translate through 0..1 of all the container/s
        foreach (RectTransform container in containers)
        {
            RectTransform parent = (RectTransform)container.parent;
            Vector2 offsetPos = FixNaN(new Vector2
                (
                    container.offsetMin.x / parent.rect.width,
                    container.offsetMin.y / parent.rect.height
                ));
            Vector2 offsetSize = FixNaN(new Vector2
                (
                    (container.offsetMax.x - container.offsetMin.x) / parent.rect.width,
                    (container.offsetMax.y - container.offsetMin.y) / parent.rect.height
                ));
            Vector2 anchorsPos = GetAnchorsPosition(container) + offsetPos;
            Vector2 anchorsSize = GetAnchorsSize(container) + offsetSize;

            // Translate the child to the parent
            result = new Vector2
            (
                result.x * anchorsSize.x + anchorsPos.x,
                result.y * anchorsSize.y + anchorsPos.y
            );
        }

        return result;
    }

    /// <summary>
    /// A canvas anchor space point to be converted to an anchor space point at the level of this object.
    /// Example: If this object is inside a container which is a child of the canvas and is located at a position of (0.5, 0.5), this 
    /// method will return (-0.5, -0.5) when you pass (0, 0).
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="anchorSpacePoint">Point to be converted to canvas anchor space point</param>
    /// <returns></returns>
    public static Vector2 GetAnchorCoordinateFromCanvasAnchorCoordinate(RectTransform tr, Vector2 anchorSpacePoint)
    {
        Vector2 result                  = anchorSpacePoint;
        List<RectTransform> containers  = new List<RectTransform>();
        RectTransform current           = tr;

        // Store all the containers of the object until the canvas is reached
        while (current != null && current.parent != current && current.parent is RectTransform)
        {
            if (current != tr)
                containers.Add(current);

            current = (RectTransform)current.parent;
        }

        // The target object can be inside one or more containers so we translate through 0..1 of all the container/s
        for (int i = containers.Count - 1; i >= 0; i--)
        {
            RectTransform container = containers[i];
            RectTransform parent = (RectTransform)container.parent;
            Vector2 offsetPos = FixNaN(new Vector2
                (
                    container.offsetMin.x / parent.rect.width,
                    container.offsetMin.y / parent.rect.height
                ));
            Vector2 offsetSize = FixNaN(new Vector2
                (
                    (container.offsetMax.x - container.offsetMin.x) / parent.rect.width,
                    (container.offsetMax.y - container.offsetMin.y) / parent.rect.height
                ));
            Vector2 anchorsPos = GetAnchorsPosition(container) + offsetPos;
            Vector2 anchorsSize = GetAnchorsSize(container) + offsetSize;

            // Translate the parent to the child
            result = FixNaN(new Vector2
            (
                (result.x - anchorsPos.x) / anchorsSize.x,
                (result.y - anchorsPos.y) / anchorsSize.y
            ));
        }

        return result;
    }

    /// <summary>
    /// Get the position of the anchors as if the object was a child of the canvas.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="centeredPivot"></param>
    /// <returns></returns>
    public static Vector2 GetAnchorPositionInCanvasAnchorCoordinates(RectTransform tr, bool centeredPivot = false)
    {
        return GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, GetAnchorsPosition(tr, centeredPivot));
    }

    /// <summary>
    /// Return the anchors size as a value relative to the canvas size. Examples: When the size is the same than the canvas this returns (1,1) and returns (0.5,0.5) when the size is the half of the canvas size in both axis. It does not matter if the container is not the canvas or the anchors positios.
    /// </summary>
    /// <param name="tr"></param>
    public static Vector2 GetSizeInCanvasAnchorCoordinatesFromAnchorsSize(RectTransform tr)
    {
        Vector2 localAnchorsPos = GetAnchorsPosition(tr);
        Vector2 position        = GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, localAnchorsPos);
        Vector2 topLeftPoint    = GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, localAnchorsPos + GetAnchorsSize(tr));
        return topLeftPoint - position;                                           
    }                                                                                 

    /// <summary>
    /// Returns a size in local anchor coordinates from a size in anchor canvas coordinate.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="canvasAnchorCoordinateSize"></param>
    /// <returns></returns>
    public static Vector2 GetAnchorsSizeFromCanvasAnchorCoordinatesSize(RectTransform tr, Vector2 canvasAnchorCoordinateSize)
    {
        Vector2 localAnchorsPos = GetAnchorsPosition(tr);
        Vector2 canvasPos       = GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, localAnchorsPos);
        return GetAnchorCoordinateFromCanvasAnchorCoordinate(tr, canvasPos + canvasAnchorCoordinateSize) - localAnchorsPos;
    }


    /// <summary>
    /// Converts a point in the rect coordinates to anchor coordinates. The returned value will be the anchor coordinates if the anchors were in that specific position position in rect coordinates.
    /// </summary>
    /// <param name="tr"></param>
    public static Vector2 GetAnchorCoordianteFromRectCoordinate(RectTransform tr, Vector2 rectCoordinate)
    {
        Vector2 parentSize = RteRectTools.GetParentSizeIgnoringAnchors(tr);
        return FixNaN(new Vector2(rectCoordinate.x / parentSize.x, rectCoordinate.y / parentSize.y));
    }

    /// <summary>
    /// Converts a point in the anchor coordinates to rect coordinates. For example place the rect in a specific position and the returned value will be the anchor coordinates if the anchors were in that specific position.
    /// </summary>
    /// <param name="tr"></param>
    public static Vector2 GetRectCoordinateFromAnchorCoordinate(RectTransform tr, Vector2 anchorCoordinate)
    {
        Vector2 parentSize = RteRectTools.GetParentSizeIgnoringAnchors(tr);
        return new Vector2(parentSize.x * anchorCoordinate.x, parentSize.y * anchorCoordinate.y);
    }

    public static Vector2 GetRectSizeFromAnchorSize(RectTransform tr, Vector2 anchorSize)
    {
        return GetRectCoordinateFromAnchorCoordinate(tr, anchorSize);
    }

    public static Vector2 GetAnchorSizeFromRectSize(RectTransform tr, Vector2 rectSize)
    {
        return GetAnchorCoordianteFromRectCoordinate(tr, rectSize);
    }

    /// <summary>
    /// Converts a point in rect coordinates to canvas anchor cordinates. So if you place the rect position at the lower left corner of the canvas it should give you (0,0). It does not matter if the container is not the canvas or the anchors positions.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="rectCoordinate"></param>
    public static Vector2 GetCanvasAnchorCoordinateNormalizedFromRectCoordinate(RectTransform tr, Vector2 rectCoordinate)
    {
        return GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, GetAnchorCoordianteFromRectCoordinate(tr, rectCoordinate));
    }

    /// <summary>
    /// Converts a point in rect coordinates to canvas anchor cordinates. So if you place the rect position at the lower left corner of the canvas it should give you (0,0). It does not matter if the container is not the canvas or the anchors positions.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="rectCoordinate"></param>
    public static Vector2 GetCanvasAnchorCoordinateFromRectCoordinate(RectTransform tr, Vector2 rectCoordinate)
    {
        Vector2 result      = GetCanvasAnchorCoordinateNormalizedFromRectCoordinate(tr, rectCoordinate);
        Vector2 canvasSize  = tr.GetCanvas().sizeDelta;

        // Convert normalized to not normalized
        result.x *= canvasSize.x;
        result.y *= canvasSize.y;

        return result;
    }

    /// <summary>
    /// Converts a point in canvas coordinates to rect cordinates. So if you place the rect position at the lower left corner of the canvas it should give you (0,0). It does not matter if the container is not the canvas or the anchors positions.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="rectCoordinates"></param>
    public static Vector2 GetRectCoordinateFromCanvasAnchorCoordinateNormalized(RectTransform tr, Vector2 canvasCoordinate)
    {
        return GetRectCoordinateFromAnchorCoordinate(tr, GetAnchorCoordinateFromCanvasAnchorCoordinate(tr, canvasCoordinate));
    }

    /// <summary>
    /// Converts a point in canvas coordinates to rect cordinates. So if you place the rect position at the lower left corner of the canvas it should give you (0,0). It does not matter if the container is not the canvas or the anchors positions.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="rectCoordinates"></param>
    public static Vector2 GetRectCoordinateFromCanvasAnchorCoordinate(RectTransform tr, Vector2 canvasCoordinate)
    {
        // Convert to normalized
        Vector2 canvasSize  = tr.GetCanvas().sizeDelta;
        canvasCoordinate.x /= canvasSize.x;
        canvasCoordinate.y /= canvasSize.y;
        canvasCoordinate    = FixNaN(canvasCoordinate);

        return GetRectCoordinateFromAnchorCoordinate(tr, GetAnchorCoordinateFromCanvasAnchorCoordinate(tr, canvasCoordinate));
    }

    /// <summary>
    /// Return the rect position as a value relative to the canvas size. When the rect is located at the Left Down corner of the canvas this returns (0,0) and returns (1,1) when is placed at the top right corner of the canvas. It does not matter if the container is not the canvas or the anchors positions.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="centerPivot"></param>
    public static Vector2 GetCanvasAnchorCoordinateNormalizedFromRectPosition(RectTransform tr, bool centerPivot = false)
    {
        return GetCanvasAnchorCoordinateNormalizedFromRectCoordinate(tr, RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr, centerPivot));
    }


    /// <summary>
    /// Return the rect size normalized as a value relative to the canvas size. Examples: When the size is the same than the canvas this returns (1,1) and returns (0.5,0.5) when the size is the half of the canvas size in both axis. It does not matter if the container is not the canvas or the anchors positios.
    /// </summary>
    /// <param name="tr"></param>
    public static Vector2 GetSizeNormalizedInCanvasAnchorCoordinatesFromRectSize(RectTransform tr)
    {
        Vector2 position     = GetCanvasAnchorCoordinateNormalizedFromRectPosition(tr);
        Vector2 topLeftPoint = GetCanvasAnchorCoordinateNormalizedFromRectCoordinate(tr, RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr) + RteRectTools.GetSizeIgnoringAnchors(tr));
        return topLeftPoint - position;
    }

    /// <summary>
    /// Return the rect size as a value relative to the canvas size. Examples: When the size is the same than the canvas this returns (1,1) and returns (0.5,0.5) when the size is the half of the canvas size in both axis. It does not matter if the container is not the canvas or the anchors positios.
    /// </summary>
    /// <param name="tr"></param>
    public static Vector2 GetSizeInCanvasAnchorCoordinatesFromRectSize(RectTransform tr)
    {
        RectTransform   canvas      = tr.GetCanvas();
        Vector2         normalized  = GetSizeNormalizedInCanvasAnchorCoordinatesFromRectSize(tr);
        return new Vector2(normalized.x * canvas.sizeDelta.x, normalized.y * canvas.sizeDelta.y);
    }

    /// <summary>
    /// Returns a size in rect coordinates from a size in anchor canvas coordinate.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="canvasCoordinateSize"></param>
    /// <returns></returns>
    public static Vector2 GetRectSizeFromCanvasAnchorCoordinatesSize(RectTransform tr, Vector2 canvasCoordinateSize)
    {
        RectTransform canvas = tr.GetCanvas();

        //From not normalized to normalized:
        canvasCoordinateSize.x = canvasCoordinateSize.x / canvas.sizeDelta.x;
        canvasCoordinateSize.y = canvasCoordinateSize.y / canvas.sizeDelta.y;
        canvasCoordinateSize   = FixNaN(canvasCoordinateSize);

        return GetRectSizeFromCanvasAnchorCoordinatesSizeNormalized(tr, canvasCoordinateSize);
    }

    /// <summary>
    /// Returns a size in rect coordinates from a size in anchor canvas coordinate (normalized).
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="canvasAnchorCoordinateSize"></param>
    /// <returns></returns>
    public static Vector2 GetRectSizeFromCanvasAnchorCoordinatesSizeNormalized(RectTransform tr, Vector2 canvasAnchorCoordinateSize)
    {
        Vector2 canvasPos = GetCanvasAnchorCoordinateNormalizedFromRectPosition(tr);
        Vector2 canvasAnchorCoordinateSizeToLocalAnchorSize = GetAnchorCoordinateFromCanvasAnchorCoordinate(tr, canvasPos + canvasAnchorCoordinateSize) - GetAnchorCoordianteFromRectCoordinate(tr, RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr));
        return GetRectCoordinateFromAnchorCoordinate(tr, canvasAnchorCoordinateSizeToLocalAnchorSize);
    }

    public static Vector2 GetRectCoordinateFromScreenSpaceCoordinate(RectTransform tr, Vector2 screenSpaceCoordinate)
    {
        return GetRectCoordinateFromAnchorCoordinate(tr, GetAnchorCoordinateFromScreenSpaceCoordinate(tr, screenSpaceCoordinate));
    }

    private static Rect GetOutsideOfCanvasRange(RectTransform tr)
    {
        Vector2 anchorPosAsCanvas   = GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, GetAnchorsPosition(tr));
        Vector2 anchorSizeAsCanvas  = GetSizeInCanvasAnchorCoordinatesFromAnchorsSize(tr);
        Vector2 rectPosAsCanvas     = GetCanvasAnchorCoordinateNormalizedFromRectPosition(tr);
        Vector2 rectSizeAsCanvas    = GetSizeNormalizedInCanvasAnchorCoordinatesFromRectSize(tr);
        Vector2 anchorsOffset       = anchorPosAsCanvas - rectPosAsCanvas;
        Rect anchorsAsRect          = new Rect(anchorPosAsCanvas, anchorSizeAsCanvas);
        Rect rectAsRect             = new Rect(rectPosAsCanvas, rectSizeAsCanvas);
        Rect bounds                 = anchorsAsRect.CombineWith(rectAsRect);

        if(anchorsOffset.x < 0)
            anchorsOffset.x = 0;
        if(anchorsOffset.y < 0)
            anchorsOffset.y = 0;

        Vector2 minPos = -bounds.size + anchorsOffset;
        Vector2 maxPos = Vector2.one  + anchorsOffset;

        return new Rect(minPos, maxPos);
    }

    /// <summary>
    /// Outside of canvas coordinate means: when the object is at 0 in the x axis, then is placed outside of canvas to the left. 1 is outside the canvas to the right. Obviously the same applies to the Y axis. Does not matter if the object is a child of the canvas or any anchors configuration. This is usefull to move things out of the screen.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="outsideOfCanvasCoordinate"></param>
    /// <returns>Anchor coordinates to move the anchors.</returns>
    public static Vector2 GetAnchorCoordinateFromOutsideOfCanvasCoordinate(RectTransform tr, Vector2 outsideOfCanvasCoordinate)
    {
        Rect range = GetOutsideOfCanvasRange(tr);   // A Rect is used just because is a group of Vector2;
        Vector2 resultAsCanvasAnchorCoord = FixNaN(ValueRangeTranslate.TranslateVector2(outsideOfCanvasCoordinate, Vector2.zero, Vector2.one, range.position, range.size, false));
        return GetAnchorCoordinateFromCanvasAnchorCoordinate(tr, resultAsCanvasAnchorCoord);
    }
    
    /// <summary>
    /// To know how far from being outside of the screen is an object. Outside of canvas coordinate means: when the object is at 0 in the x axis, then is placed outside of canvas to the left. 1 is outside the canvas to the right. Obviously the same applies to the Y axis. Does not matter if the object is a child of the canvas or any anchors configuration. This is usefull to move things out of the screen.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="anchorCoordinate"></param>
    /// <returns></returns>
    public static Vector2 GetOutsideOfCanvasCoordinateFromAnchorCoordinate(RectTransform tr, Vector2 anchorCoordinate)
    {
        Rect range = GetOutsideOfCanvasRange(tr);   // A Rect is used just because is a group of Vector2;
        Vector2 anchorCoordinateAsCanvas = GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, anchorCoordinate);
        return FixNaN(ValueRangeTranslate.TranslateVector2(anchorCoordinateAsCanvas, range.position, range.size, Vector2.zero, Vector2.one, false));
    }

    private static Rect GetOutsideOfContainerRange(RectTransform tr)
    {
        Vector2 anchorPos            = GetAnchorsPosition(tr);
        Vector2 anchorSize           = GetAnchorsSize(tr);
        Vector2 rectPosAsAnchors     = GetAnchorCoordianteFromRectCoordinate(tr, RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr));
        Vector2 rectSizeAsAnchor     = GetAnchorCoordianteFromRectCoordinate(tr, RteRectTools.GetSizeIgnoringAnchors(tr));
        Vector2 anchorsOffset        = anchorPos - rectPosAsAnchors;
        Rect anchorsAsRect           = new Rect(anchorPos, anchorSize);
        Rect rectAsRect              = new Rect(rectPosAsAnchors, rectSizeAsAnchor);
        Rect bounds                  = anchorsAsRect.CombineWith(rectAsRect);

        if(anchorsOffset.x < 0)
            anchorsOffset.x = 0;
        if(anchorsOffset.y < 0)
            anchorsOffset.y = 0;

        Vector2 minPos = -bounds.size + anchorsOffset;
        Vector2 maxPos = Vector2.one  + anchorsOffset;

        return new Rect(minPos, maxPos);
    }

    /// <summary>
    /// Outside of container coordinate means: when the object is at 0 in the x axis then is placed outside of container to the left. 1 is outside the container to the right. The same applies to the Y axis. Does not matter the anchors configuration in the object or in the container.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="outsideOfContainerCoordinate"></param>
    /// <returns></returns>
    public static Vector2 GetAnchorCoordinateFromOutsideOfContainerCoordinate(RectTransform tr, Vector2 outsideOfContainerCoordinate)
    {
        Rect range = GetOutsideOfContainerRange(tr);  // A Rect is used just because is a group of Vector2;
        return FixNaN(ValueRangeTranslate.TranslateVector2(outsideOfContainerCoordinate, Vector2.zero, Vector2.one, range.position, range.size, false));
    }

    /// <summary>
    /// To know how far the object is from the are of it's container. Outside of container coordinate means: when the object is at 0 in the x axis then is placed outside of container to the left. 1 is outside the container to the right. The same applies to the Y axis. Does not matter the anchors configuration in the object or in the container.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="anchorCoordinate"></param>
    /// <returns></returns>
    public static Vector2 GetOutsideOfContainerCoordinateFromAnchorCoordinate(RectTransform tr, Vector2 anchorCoordinate)
    {
        Rect range = GetOutsideOfContainerRange(tr);  // A Rect is used just because is a group of Vector2;
        return FixNaN(ValueRangeTranslate.TranslateVector2(anchorCoordinate, range.position, range.size, Vector2.zero, Vector2.one, false));
    }

    private static Rect GetInsideOfCanvasRange(RectTransform tr)
    {
        Vector2 anchorPosAsCanvas   = GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, GetAnchorsPosition(tr));
        Vector2 rectPosAsCanvas     = GetCanvasAnchorCoordinateNormalizedFromRectPosition(tr);
        Vector2 rectSizeAsCanvas    = GetSizeNormalizedInCanvasAnchorCoordinatesFromRectSize(tr);
        Vector2 anchorsOffset       = anchorPosAsCanvas - rectPosAsCanvas;
        Rect rectAsRect             = new Rect(rectPosAsCanvas, rectSizeAsCanvas);

        Vector2 minPos = Vector2.zero + anchorsOffset;
        Vector2 maxPos = Vector2.one - rectAsRect.size + anchorsOffset;

        return new Rect(minPos, maxPos);
    }

    /// <summary>
    /// Outside of canvas coordinate means: when the object is at 0 in the x axis, then is placed outside of canvas to the left. 1 is outside the canvas to the right. Obviously the same applies to the Y axis. Does not matter if the object is a child of the canvas or any anchors configuration. This is usefull to move things out of the screen.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="outsideOfCanvasCoordinate"></param>
    /// <returns>Anchor coordinates to move the anchors.</returns>
    public static Vector2 GetAnchorCoordinateFromInsideOfCanvasCoordinate(RectTransform tr, Vector2 outsideOfCanvasCoordinate)
    {
        Rect range = GetInsideOfCanvasRange(tr);    // A Rect is used just because is a group of Vector2;
        Vector2 resultAsCanvasAnchorCoord = FixNaN(ValueRangeTranslate.TranslateVector2(outsideOfCanvasCoordinate, Vector2.zero, Vector2.one, range.position, range.size, false));
        return GetAnchorCoordinateFromCanvasAnchorCoordinate(tr, resultAsCanvasAnchorCoord);
    }
    
    /// <summary>
    /// To know how far from being outside of the screen is an object. Outside of canvas coordinate means: when the object is at 0 in the x axis, then is placed outside of canvas to the left. 1 is outside the canvas to the right. Obviously the same applies to the Y axis. Does not matter if the object is a child of the canvas or any anchors configuration. This is usefull to move things out of the screen.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="anchorCoordinate"></param>
    /// <returns></returns>
    public static Vector2 GetInsideOfCanvasCoordinateFromAnchorCoordinate(RectTransform tr, Vector2 anchorCoordinate)
    {
        Rect    range = GetInsideOfCanvasRange(tr);    // A Rect is used just because is a group of Vector2;
        Vector2 anchorCoordinateAsCanvas = GetCanvasAnchorCoordinateFromAnchorCoordinate(tr, anchorCoordinate);
        return FixNaN(ValueRangeTranslate.TranslateVector2(anchorCoordinateAsCanvas, range.position, range.size, Vector2.zero, Vector2.one, false));
    }

    private static Vector2 FixNaN(Vector2 source)
    {
        if(float.IsNaN(source.x) || float.IsInfinity(source.x))
            source.x = 0;
        if(float.IsNaN(source.y) || float.IsInfinity(source.y))
            source.y = 0;
        return source;
    }
}

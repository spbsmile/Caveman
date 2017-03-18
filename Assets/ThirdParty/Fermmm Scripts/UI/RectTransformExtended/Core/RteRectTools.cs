using UnityEngine;

public static class RteRectTools
{
    // Todo: The following 3 methods can be used as a replace for the old ones and get a performance upgrade.

    public static void GetRectCornersInScreenSpacePixels(RectTransform transform, Canvas canvas, Vector3[] cornersArray)
    {
        if(canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            GetCornersInCanvasSpace(transform, cornersArray);
            cornersArray[0] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, cornersArray[0]);
            cornersArray[1] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, cornersArray[1]);
            cornersArray[2] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, cornersArray[2]);
            cornersArray[3] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, cornersArray[3]);
        }
        else
        {
            transform.GetWorldCorners(cornersArray);
        }
    }

    /// <summary>
    /// Gets the corners of a RectTransform in global canvas space.
    /// </summary>
    public static void GetCornersInCanvasSpace(RectTransform transform, Vector3[] cornersArray)
    {
        //Bounds
        Vector2 min = transform.rect.min;
        Vector2 max = transform.rect.max;

        //The corners
        var a = transform.TransformPoint(min);
        var b = transform.TransformPoint(new Vector3(min.x, max.y));
        var c = transform.TransformPoint(max);
        var d = transform.TransformPoint(new Vector3(max.x, min.y));

        cornersArray[0] = a;
        cornersArray[1] = b;
        cornersArray[2] = c;
        cornersArray[3] = d;
    }

    public static Rect ConvertCornersToRect(Vector3[] corners)
    {
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }
    
    /// <summary>
    /// The With and Height parameters on the RectTransform of the parent ignoring anchors. With this you can get the real Size of the RectTransform. Like when the anchors are joined.
    /// </summary>
    /// <param name="transf"></param>
    public static Vector2 GetParentSizeIgnoringAnchors(RectTransform transf)
    {
        if(transf.parent == null)
            return transf.sizeDelta;

        if(!(transf.parent is RectTransform))
            return transf.sizeDelta;

        return GetSizeIgnoringAnchors((RectTransform)transf.parent);
    }

    /// <summary>
    /// Returns the position of the pivot point relative to the parent rect coordinates, instead of what you get with AnchoredPosition property which is the position of the pivot relative to the anchors center point.
    /// </summary>
    /// <param name="transf"></param>
    /// <returns></returns>
    public static Vector2 GetPositionIgnoringAnchors(RectTransform transf)
    {
        Vector2 anchorsSize         = RteAnchorTools.GetAnchorsSize(transf);
        Vector2 anchorsPivotedPos   = new Vector2(transf.anchorMin.x + anchorsSize.x * transf.pivot.x, transf.anchorMin.y + anchorsSize.y * transf.pivot.y);
        Vector2 parentSize          = GetParentSizeIgnoringAnchors(transf);

        return new Vector2
            (
                transf.anchoredPosition.x + parentSize.x  * anchorsPivotedPos.x,
                transf.anchoredPosition.y + parentSize.y  * anchorsPivotedPos.y
            );
    }

    /// <summary>
    /// Returns the position of the object with the left down point as the position point instead of the pivot point as the position point, also the position is not relative to the anchors but relative to the parent rect coordinates.
    /// </summary>
    /// <param name="transf"></param>
    /// <returns></returns>
    public static Vector2 GetPositionIgnoringAnchorsAndPivot(RectTransform transf, bool centerPivot = false)
    {
        Vector2 nonAnchoredPos = GetPositionIgnoringAnchors(transf);

        Vector2 size = Vector2.zero;
        if(centerPivot)
            size = GetSizeIgnoringAnchors(transf);

        return new Vector2
        (
            nonAnchoredPos.x - (transf.rect.width * transf.pivot.x) + size.x * 0.5f,    
            nonAnchoredPos.y - (transf.rect.height * transf.pivot.y) + size.y * 0.5f
        );
    }

    /// <summary>
    /// Sets the position of the object with a coordinate relative to the parent rect coordinates, instead of what you set with AnchoredPosition property which is the position relative to the anchors center point.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="newPosition"></param>
    public static void SetPositionIgnoringAnchors(RectTransform transf, Vector2 newPosition)
    {
        Vector2 anchorsSize = RteAnchorTools.GetAnchorsSize(transf);
        Vector2 anchorsPivotedPos = new Vector2(transf.anchorMin.x + anchorsSize.x * transf.pivot.x, transf.anchorMin.y + anchorsSize.y * transf.pivot.y);
        Vector2 parentSize = GetParentSizeIgnoringAnchors(transf);

        transf.anchoredPosition = newPosition - new Vector2
            (
                parentSize.x * anchorsPivotedPos.x,
                parentSize.y * anchorsPivotedPos.y
            );
    }

    /// <summary>
    /// Sets the position of the object with the left down point as the position point instead of the pivot point as the position point, also the position is not set relative to the anchors but relative to the parent rect coordinates.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="newPosition"></param>
    public static void SetPositionIgnoringAnchorsAndPivot(RectTransform transf, Vector2 newPosition, bool centerPivot = false)
    {
        Vector2 rectSize            = GetSizeIgnoringAnchors(transf);
        Vector2 anchorsSize         = RteAnchorTools.GetAnchorsSize(transf);
        Vector2 anchorsPivotedPos   = new Vector2(transf.anchorMin.x + anchorsSize.x * transf.pivot.x, transf.anchorMin.y + anchorsSize.y * transf.pivot.y);
        Vector2 parentSize          = GetParentSizeIgnoringAnchors(transf);

        transf.anchoredPosition = newPosition - new Vector2
            (
                parentSize.x * anchorsPivotedPos.x - rectSize.x * transf.pivot.x,
                parentSize.y * anchorsPivotedPos.y - rectSize.y * transf.pivot.y
            );

        if(centerPivot)
            transf.anchoredPosition -= new Vector2(rectSize.x * 0.5f, rectSize.y * 0.5f);
    }

    /// <summary>
    /// Sets the X position of the object with the left down point as the position point instead of the pivot point as the position point, also the position is not set relative to the anchors but relative to the parent rect coordinates.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="newPositionX"></param>
    public static void SetPositionXIgnoringAnchorsAndPivot(RectTransform transf, float newPositionX, bool centerPivot = false)
    {
        Vector2 size = Vector2.zero;
        if(centerPivot)
            size = GetSizeIgnoringAnchors(transf);
        Vector2 newPos = transf.anchoredPosition;
        Vector2 parentSize = GetParentSizeIgnoringAnchors(transf);

        newPos.x = newPositionX - (parentSize.x * RteAnchorTools.GetAnchorsPosition(transf,true).x) + (transf.rect.width * transf.pivot.x) - size.x * 0.5f;
        transf.anchoredPosition = newPos;
    }

    /// <summary>
    /// Sets the Y position of the object with the left down point as the position point instead of the pivot point as the position point, also the position is not set relative to the anchors but relative to the parent rect coordinates.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="newPositionY"></param>
    public static void SetPositionYIgnoringAnchorsAndPivot(RectTransform transf, float newPositionY, bool centerPivot = false)
    {
        Vector2 size = Vector2.zero;
        if(centerPivot)
            size = GetSizeIgnoringAnchors(transf);
        Vector2 newPos = transf.anchoredPosition;
        Vector2 parentSize = GetParentSizeIgnoringAnchors(transf);

        newPos.y = newPositionY - (parentSize.y * RteAnchorTools.GetAnchorsPosition(transf,true).y) + (transf.rect.height * transf.pivot.y) - size.y * 0.5f;
        transf.anchoredPosition = newPos;
    }

    /// <summary>
    /// The With and Height parameters of RectTransform gives relative to anchors values when anchors are not joined, wich is not so usefull. With this you can get the real Size of the RectTransform. Like when the anchors are joined.
    /// </summary>
    /// <param name="transf"></param>
    public static Vector2 GetSizeIgnoringAnchors(RectTransform transf)
    {
        Vector2 parentSize = GetParentSizeIgnoringAnchors(transf);
        Vector2 anchorsSize = RteAnchorTools.GetAnchorsSize(transf);

        return new Vector2
            (
                parentSize.x * anchorsSize.x + transf.sizeDelta.x,
                parentSize.y * anchorsSize.y + transf.sizeDelta.y
            );            
    }

    /// <summary>
    /// Get the size not ignoring anchors from a size value that ignores anchors.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="sizeIgnoringAnchors"></param>
    public static Vector2 GetSizeAnchored(RectTransform transf, Vector2 sizeIgnoringAnchors)
    {
        Vector2 anchorsSize = RteAnchorTools.GetAnchorsSize(transf);
        Vector2 parentSize  = GetParentSizeIgnoringAnchors(transf);

        Vector2 size = new Vector2
            (
                parentSize.x * anchorsSize.x - sizeIgnoringAnchors.x,
                parentSize.y * anchorsSize.y - sizeIgnoringAnchors.y
            );

        return size;
    }

    /// <summary>
    /// Get the width not ignoring anchors from a width value that ignores anchors.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="widthIgnoringAnchors"></param>
    /// <returns></returns>
    public static float GetWidthAnchored(RectTransform transf, float widthIgnoringAnchors)
    {
        Vector2 anchorsSize = RteAnchorTools.GetAnchorsSize(transf);
        Vector2 parentSize  = GetParentSizeIgnoringAnchors(transf);

        float width = parentSize.x * anchorsSize.x - widthIgnoringAnchors;
        
        return width;
    }

    /// <summary>
    /// Get the height not ignoring anchors from a height value that ignores anchors.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="heightIgnoringAnchors"></param>
    /// <returns></returns>
    public static float GetHeightAnchored(RectTransform transf, float heightIgnoringAnchors)
    {
        Vector2 anchorsSize = RteAnchorTools.GetAnchorsSize(transf);
        Vector2 parentSize  = GetParentSizeIgnoringAnchors(transf);

        float height = parentSize.y * anchorsSize.y - heightIgnoringAnchors;

        return height;
    }

    /// <summary>
    /// The With and Height parameters of RectTransform gives relative to anchors values when anchors are not joined, wich is not so usefull. With this you can set the real Size of the RectTransform. Like when the anchors are joined.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="newSize"></param>
    public static void SetSizeIgnoringAnchors(RectTransform transf, Vector2 newSize, bool centerPivot = false)
    {
        SetWidthIgnoringAnchors(transf, newSize.x, centerPivot);
        SetHeightIgnoringAnchors(transf, newSize.y, centerPivot);
    }

    /// <summary>
    /// The With and Height parameters of RectTransform gives relative to anchors values when anchors are not joined, wich is not so usefull. With this you can set the real Width of the RectTransform. Like when the anchors are joined.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="newWidth"></param>
    public static void SetWidthIgnoringAnchors(RectTransform transf, float newWidth, bool centerPivot = false)
    {
        // Todo: Implement centerPivot
        transf.offsetMax = new Vector2(transf.offsetMin.x - GetWidthAnchored(transf, newWidth), transf.offsetMax.y);
    }

    /// <summary>
    /// The With and Height parameters of RectTransform gives relative to anchors values when anchors are not joined, wich is not so usefull. With this you can set the real Height of the RectTransform. Like when the anchors are joined.
    /// </summary>
    /// <param name="transf"></param>
    /// <param name="newHeight"></param>
    public static void SetHeightIgnoringAnchors(RectTransform transf, float newHeight, bool centerPivot = false)
    {
        // Todo: Implement centerPivot
        transf.offsetMax = new Vector2(transf.offsetMax.x, transf.offsetMin.y - GetHeightAnchored(transf, newHeight));
    }

    /// <summary>
    /// Get the rect position in values from 0 to 1 relative to the parent. This ignores anchors.
    /// </summary>
    public static Vector2 GetPositionNormalizedIgnoringAnchorsAndPivot(RectTransform transf, bool centerPivot = false)
    {
        Vector2 rectPos    = GetPositionIgnoringAnchorsAndPivot(transf, centerPivot);
        Vector2 parentSize = GetParentSizeIgnoringAnchors(transf);

        return FixNaN(new Vector2(rectPos.x / parentSize.x, rectPos.y / parentSize.y));
    }

    /// <summary>
    /// Set the rect position in values from 0 to 1 relative to the parent. This ignores anchors.
    /// </summary>
    public static void SetPositionNormalizedIgnoringAnchorsAndPivot(RectTransform transf, Vector2 newRelativePos, bool centerPivot = false)
    {
        Vector2 parentSize = GetParentSizeIgnoringAnchors(transf);
        SetPositionIgnoringAnchorsAndPivot(transf, new Vector2(parentSize.x * newRelativePos.x, parentSize.y * newRelativePos.y), centerPivot);
    }

    /// <summary>
    /// Get the rect size in values from 0 to 1 relative to the parent. This ignores anchors.
    /// </summary>
    public static Vector2 GetSizeNormalizedIgnoringAnchors(RectTransform transf)
    {
        Vector2 rectSize    = GetSizeIgnoringAnchors(transf);
        Vector2 parentSize = GetParentSizeIgnoringAnchors(transf);

        return FixNaN(new Vector2(rectSize.x / parentSize.x, rectSize.y / parentSize.y));
    }

    /// <summary>
    /// Set the rect size in values from 0 to 1 relative to the parent. This ignores anchors.
    /// </summary>
    public static void SetSizeNormalizedIgnoringAnchors(RectTransform transf, Vector2 newRelativeSize, bool centerPivot = false)
    {
        Vector2 parentSize = GetParentSizeIgnoringAnchors(transf);
        SetSizeIgnoringAnchors(transf, new Vector2(parentSize.x * newRelativeSize.x, parentSize.y * newRelativeSize.y), centerPivot);
    }

    public static Rect GetRectIngoringAnchorsAndPivot(RectTransform transf, bool centeredPivot = false)
    {
        return new Rect(GetPositionIgnoringAnchorsAndPivot(transf, centeredPivot), GetSizeIgnoringAnchors(transf));
    }

    public static Rect GetRectBoundsIncludingRotation(RectTransform transf)
    {
        Rect    rect     = GetRectIngoringAnchorsAndPivot(transf);
        Vector2 pivot    = new Vector2((rect.width * transf.pivot.x) + rect.position.x, (rect.height * transf.pivot.y) + rect.position.y);
        float   rotation = transf.localRotation.eulerAngles.z * Mathf.Deg2Rad;

        Vector2 corner1 = TranslatePointToRotation(new Vector2(rect.xMin, rect.yMin), rotation, pivot);
        Vector2 corner2 = TranslatePointToRotation(new Vector2(rect.xMax, rect.yMax), rotation, pivot);
        Vector2 corner3 = TranslatePointToRotation(new Vector2(rect.xMin, rect.yMax), rotation, pivot);
        Vector2 corner4 = TranslatePointToRotation(new Vector2(rect.xMax, rect.yMin), rotation, pivot);

        float minX = Mathf.Min(corner1.x, corner2.x, corner3.x, corner4.x);
        float maxX = Mathf.Max(corner1.x, corner2.x, corner3.x, corner4.x);
        float minY = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);
        float maxY = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);

        return new Rect(new Vector2(minX, minY), new Vector2(maxX - minX, maxY - minY));
    }

    private static Vector2 TranslatePointToRotation(Vector2 point, float rotation, Vector2 pivotPos)
    {
        return new Vector2(
                pivotPos.x + (point.x - pivotPos.x) * Mathf.Cos(rotation) + (point.y - pivotPos.y) * Mathf.Sin(rotation),
                //pivotPos.x + (point.x - pivotPos.x) * Mathf.Cos(rotation) - (point.y - pivotPos.y) * Mathf.Sin(rotation),   // Alternative
                pivotPos.y - (point.x - pivotPos.x) * Mathf.Sin(rotation) + (point.y - pivotPos.y) * Mathf.Cos(rotation)
                //pivotPos.y - (point.x - pivotPos.x) * Mathf.Sin(rotation) - (point.y - pivotPos.y) * Mathf.Cos(rotation)    // Alternative
            );
    }

    private static Vector2 FixNaN(Vector2 source)
    {
        if(float.IsNaN(source.x))
            source.x = 0;
        if(float.IsNaN(source.y))
            source.y = 0;
        return source;
    }
}

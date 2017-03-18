using UnityEngine;

public class RtePivotTools
{
    public static Vector2 GetCanvasPositionFromLocalPosition(RectTransform tr, Vector2 pivotPosition)
    {
        Vector2 size     = RteRectTools.GetSizeIgnoringAnchors(tr);
        Vector2 position = RteRectTools.GetPositionIgnoringAnchorsAndPivot(tr);
        size.x  *= pivotPosition.x;
        size.y  *= pivotPosition.y;
        size    += position;
        return RteAnchorTools.GetCanvasAnchorCoordinateNormalizedFromRectCoordinate(tr, size);
    }

    public static Vector2 GetLocalPositionFromCanvasPosition(RectTransform transform, Vector2 canvasPosition)
    {
        Vector2 rectCoordinate = RteAnchorTools.GetRectCoordinateFromCanvasAnchorCoordinateNormalized(transform, canvasPosition);
        Vector2 position       = RteRectTools.GetPositionIgnoringAnchorsAndPivot(transform);
        Vector2 size           = RteRectTools.GetSizeIgnoringAnchors(transform);
        rectCoordinate -= position;
        rectCoordinate.x  /= size.x;
        rectCoordinate.y  /= size.y;
        rectCoordinate.FixNaN();
        return rectCoordinate;
    }

    public static void SetPivotPosition(RectTransform transform, Vector2 newPivotPos, bool alsoMoveRect)
    {
        if(alsoMoveRect)
        {
            transform.pivot = newPivotPos;
            return;
        }

        Vector2 size             = transform.rect.size;
        Vector2 deltaPivot       = transform.pivot - newPivotPos;
        Vector3 deltaPosition    = new Vector3(deltaPivot.x * size.x * transform.localScale.x, deltaPivot.y * size.y * transform.localScale.y);
        transform.pivot          = newPivotPos;
        transform.localPosition -= deltaPosition;
    }
}

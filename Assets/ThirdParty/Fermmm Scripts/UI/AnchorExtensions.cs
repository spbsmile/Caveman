using System;
using System.Collections.Generic;
using UnityEngine;

public static class AnchorExtensions
{
    public static bool ThrowExeptions = true;

    /// <summary>
    /// Get the anchors position. The intent of this function is manipulating anchors with only a single Vector2, specially 
    /// usefull for tweens and clean code.
    /// In most cases it's a good idea to move and resize images using anchors because everything becomes screen size independent.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="centeredPivot">If false the pivot is in the down left corner, if true the pivot is in the center</param>
    /// <returns></returns>
    public static Vector2 GetAnchorsPosition(this RectTransform tr, bool centeredPivot = false)
    {
        if(!centeredPivot)
            return tr.anchorMin;

        Vector2 size = tr.GetAnchorsSize();
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
    public static Vector2 GetAnchorsSize(this RectTransform tr)
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
    public static void SetAnchorsPosition(this RectTransform tr, Vector2 targetPos, bool centeredPivot = false)
    {
        Vector2 size = tr.GetAnchorsSize();
        
        if (centeredPivot)
            targetPos -= new Vector2(size.x * 0.5f, size.y * 0.5f);

        // anchorMin.x and anchorMix.y moves the object, the other 2 values just move the same amount (if the size is not changed).
        tr.anchorMin = targetPos;
        tr.anchorMax = targetPos + size;
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
    public static void SetAnchorsPosition(this RectTransform tr, RectTransform targetPosFromOtherObject)
    {
        tr.SetAnchorsPosition
            (
                tr.CanvasAnchorCoordinatesToThisAnchorCoordinates
                (
                    targetPosFromOtherObject.ThisAnchorCoordinatesToCanvasAnchorCoordinates
                    (
                        targetPosFromOtherObject.GetAnchorsPosition()
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
    public static void SetAnchorsSize(this RectTransform tr, Vector2 targetSize)
    {
        //Size is determined by AnchorMax
        tr.anchorMax = tr.anchorMin + targetSize;
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
    public static void SetAnchorsSize(this RectTransform tr, RectTransform targetPosFromOtherObject)
    {
        tr.SetAnchorsSize
        (
            tr.CanvasAnchorCoordinatesToThisAnchorCoordinates
            (
                targetPosFromOtherObject.ThisAnchorCoordinatesToCanvasAnchorCoordinates
                (
                    targetPosFromOtherObject.GetAnchorsSize()
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
    public static Vector2 ScreenSpaceToAnchorCoordinates(this RectTransform tr, Vector2 screenSpacePoint)
    {
        // Convert screen space coordinates to 0..1 range, this alone is enought if the parent of the object is the canvas.
        var result = new Vector2(screenSpacePoint.x / Screen.width, screenSpacePoint.y / Screen.height);
        return tr.CanvasAnchorCoordinatesToThisAnchorCoordinates(result);
    }
    
    /// <summary>
    /// Converts a range between 0 to 1 (used by the anchors), I call it "anchor space" to a screen space coordinate (pixels).
    /// Note: This probably will not work as you expect when the canvas is configured as "Word Space" because everithing becomes 3D.
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="anchorSpacePoint">The anchor space point at the level of this object to be converted to screen space coordinate point (pixel)</param>
    /// <returns></returns>
    public static Vector2 AnchorCoordinatesToScreenSpace(this RectTransform tr, Vector2 anchorSpacePoint)
    {
        Vector2 result = tr.ThisAnchorCoordinatesToCanvasAnchorCoordinates(anchorSpacePoint);
        return new Vector2(Screen.width * result.x, Screen.height * result.y);      // convert 0..1 of the canvas to pixels.
    }

    /// <summary>
    /// An anchor space point at the level of this object to be converted to an anchor space point at the level of the canvas.
    /// Example: If this object is inside a container which is a child of the canvas and is located at a position of (0.5, 0.5), this 
    /// method will return (0.5, 0.5) when you pass (0, 0).
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="anchorSpacePoint"></param>
    /// <returns></returns>
    public static Vector2 ThisAnchorCoordinatesToCanvasAnchorCoordinates(this RectTransform tr, Vector2 anchorSpacePoint)
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
            Vector2 offsetPos = new Vector2
                (
                    container.offsetMin.x / parent.rect.width,
                    container.offsetMin.y / parent.rect.height
                );
            Vector2 offsetSize = new Vector2
                (
                    (container.offsetMax.x - container.offsetMin.x) / parent.rect.width,
                    (container.offsetMax.y - container.offsetMin.y) / parent.rect.height
                );
            Vector2 anchorsPos = container.GetAnchorsPosition() + offsetPos;
            Vector2 anchorsSize = container.GetAnchorsSize() + offsetSize;

            // Translate the child to the parent
            result = new Vector2
            (
                result.x * anchorsSize.x + anchorsPos.x,
                result.y * anchorsSize.y + anchorsPos.y
            );

            if (ThrowExeptions & Mathf.Approximately(anchorsSize.x, 0f) || Mathf.Approximately(anchorsSize.y, 0f))
                throw new Exception("To use this tool anchors must not be joined in any parent of this object.");
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
    public static Vector2 CanvasAnchorCoordinatesToThisAnchorCoordinates(this RectTransform tr, Vector2 anchorSpacePoint)
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
            Vector2 offsetPos = new Vector2
                (
                    container.offsetMin.x / parent.rect.width,
                    container.offsetMin.y / parent.rect.height
                );
            Vector2 offsetSize = new Vector2
                (
                    (container.offsetMax.x - container.offsetMin.x) / parent.rect.width,
                    (container.offsetMax.y - container.offsetMin.y) / parent.rect.height
                );
            Vector2 anchorsPos = container.GetAnchorsPosition() + offsetPos;
            Vector2 anchorsSize = container.GetAnchorsSize() + offsetSize;

            // Translate the parent to the child
            result = new Vector2
            (
                (result.x - anchorsPos.x) / anchorsSize.x,
                (result.y - anchorsPos.y) / anchorsSize.y
            );

            if (ThrowExeptions && Mathf.Approximately(anchorsSize.x, 0f) || Mathf.Approximately(anchorsSize.y, 0f))
                throw new Exception("To use this tool anchors must not be joined in any parent of this object.");
        }

        return result;
    }

}

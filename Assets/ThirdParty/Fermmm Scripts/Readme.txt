By importing the asset the following methods will be added to any RectTransform:

- GetAnchorsPosition
- SetAnchorsPosition
- GetAnchorsSize
- SetAnchorsSize
- ScreenSpaceToAnchorCoordinates
- AnchorCoordinatesToScreenSpace
- ThisAnchorCoordinatesToCanvasAnchorCoordinates
- CanvasAnchorCoordinatesToThisAnchorCoordinates

Example:

To move a uGUI object (with its anchors) to the mouse position:
void Update () 
{
    var tr = transform as RectTransform;
    tr.SetAnchorsPosition(tr.ScreenSpaceToAnchorCoordinates(Input.mousePosition));
}

For any question or issue:
fermmm@gmail.com
Current version:
3.1

Instructions:

Add the Rect Transform Extended component to any UI GameObject to practice and understand what this tool can do.

When you import this asset, the following methods are added to RectTransform:

RectTransform.GetPosition()
RectTransform.SetPosition()

RectTransform.GetAnchorsPosition()
RectTransform.SetAnchorsPosition()

RectTransform.SetPositionX()
RectTransform.SetPositionY()

RectTransform.SetWidth()
RectTransform.SetHeight()

RectTransform.SetAnchorsPositionX()
RectTransform.SetAnchorsPositionY()

RectTransform.SetAnchorsWidth()
RectTransform.SetAnchorsHeight()

Note:

To use this tool from code use the methods described above, don't get a reference to the Rect Transform Extended component, 
that is slow, difficult and leads to problems because the Rect Transform Extended component was not made to be used from code.

Also 4 options are added to the Tools menu:

	- UI Anchors To Corners
	- UI Corners To Anchors
	- UI Center Rect
	- UI Center Anchors

Usange example:
	
	RectTransfrom myRectTransform = GetComponent<RectTransform>();
	Vector2 size = myRectTransform.GetSize(CoordinateSystem.IgnoreAnchorsAndPivot);

If you need to convert coordinates and not a getter setter of position and size use the methods of the static classes: RteRectTools and RteAnchorTools
Also these static classes contains some extra coordinate conversion methods.

For questions write on this forum thread:
https://community.unity.com/t5/Asset-Store/RELEASED-EASIER-UI/td-p/2577628#post-2630254

For any other kind of feedback:
fermmm@gmail.com
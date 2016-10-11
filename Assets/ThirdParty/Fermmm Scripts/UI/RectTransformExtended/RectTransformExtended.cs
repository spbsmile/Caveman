using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("UI/Rect Transform Extended")]
public class RectTransformExtended : MonoBehaviour
{
    // Roadmap:
    // Todo: Make the trePivotCentered parameter work with rect size setter.
    // Todo: Support UI elements that are rotated and scaled.


    // Bool:
    public  bool       CenterPivot                          = true;
    public  bool       ChangeRectWithAnchors                = true;
    public  bool       AllowDistortion                      = true;
                       
    // Position:       
    public  Vector2    PositionIgnoringAnchors;
    public  Vector2    PositionIgnoringAnchorsRelative;
    public  Vector2    CanvasPos;
    public  Vector2    CanvasPosNormalized;
    public  Vector2    PixelPos;
                       
    public  Vector2    AnchorsPosition;
    public  Vector2    AnchorsPixelPosition;
    public  Vector2    AnchorsCanvasPos;
    public  Vector2    AnchorsRectPos;
    public  Vector2    AnchorsOutOfCanvasPos;
    public  Vector2    AnchorsOutOfContainerPos;
    public  Vector2    AnchorsInsideOfCanvasPos; 
                       
    //Size:            
    public  Vector2    AnchorsSize;
    public  Vector2    AnchorsCanvasSize;
    public  Vector2    AnchorsPixelsSize;
    public  Vector2    AnchorsRectSize;
                       
    public  Vector2    SizeIgnoringAnchors;
    public  Vector2    SizeIgnoringAnchorsRelative;
    public  Vector2    SizeInPixels;           
    public  Vector2    SizeAsCanvas;           
    public  Vector2    SizeAsCanvasNormalized;  
                       
    //Private:         
    private  Vector2   AnchorsPositionPrev;
    private  Vector2   AnchorsPixelPositionPrev;
    private  Vector2   PixelPositionPrev;
    private  Vector2   AnchorsSizePrev;
    private  Vector2   PositionIgnoringAnchorsPrev;
    private  Vector2   SizeIgnoringAnchorsPrev;
    private  Vector2   AnchorsCanvasPosPrev;
    private  Vector2   AnchorsOutOfCanvasPosPrev;
    private  Vector2   AnchorsOutOfContainerPosPrev;
    private  Vector2   PositionIgnoringAnchorsRelativePrev;
    private  Vector2   SizeIgnoringAnchorsRelativePrev;
    private  Vector2   AnchorsInsideOfCanvasPosPrev;
    private  Vector2   CanvasPosPrev;
    private  Vector2   CanvasPosNormalizedPrev;
    private  Vector2   SizeInPixelsPrev;
    private  Vector2   SizeAsCanvasPrev;
    private  Vector2   SizeAsCanvasNormalizedPrev;
    private  Vector2   AnchorsCanvasSizePrev;
    private  Vector2   AnchorsPixelsSizePrev;
    private  Vector2   AnchorsRectPosPrev;
    private  Vector2   AnchorsRectSizePrev;
                       
    //Editor           
    public int         SelectedCoordinates;
    public int         SelectedAnchorCoordinates;

    private RectTransform   Rt;

	// Use this for initialization
	void Start ()
    {
        GetReferences();
        UpdateChanges();
        SaveFrameData();
	}

    /// <summary>
    /// For internal usage.
    /// </summary>
    public void GetReferences()
    {
        if(Rt == null)
	        Rt = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	public void Update ()
    {
        GetReferences();
        UpdateAndApply();
	}

    public bool UpdateAndApply()
    {
        // Apply changes to RectTransform only if there is any
        bool somethingChanged = ApplyChanges();
        // Pick all stuff from RectTransform, including the changes from the previous line, and apply them back to this component.
        UpdateChanges();
        // Save frame data.
        SaveFrameData();

        return somethingChanged;
    }

    public bool ApplyChanges()
    {
        bool changed = false;
        
        if(!PositionIgnoringAnchors.IsAproximately(PositionIgnoringAnchorsPrev))
        {
            Rt.SetPosition(PositionIgnoringAnchors, CoordinateSystem.IgnoreAnchorsAndPivot, CenterPivot);
            changed = true;
        }
        
        if (!SizeIgnoringAnchors.IsAproximately(SizeIgnoringAnchorsPrev))
        {
            Rt.SetSize(SizeIgnoringAnchors, CoordinateSystem.IgnoreAnchorsAndPivot);
            changed = true;
        }

        if(!AnchorsPosition.IsAproximately(AnchorsPositionPrev))
        {
            Rt.SetAnchorsPosition(AnchorsPosition, AnchorsCoordinateSystem.Default, CenterPivot, ChangeRectWithAnchors);
            changed = true;
        }

        if (!AnchorsPixelPosition.IsAproximately(AnchorsPixelPositionPrev))
        {
            Rt.SetAnchorsPosition(AnchorsPixelPosition, AnchorsCoordinateSystem.ScreenSpacePixels, CenterPivot, ChangeRectWithAnchors);
            changed = true;
        }

        if(!AnchorsSize.IsAproximately(AnchorsSizePrev))
        {
            Rt.SetAnchorsSize(AnchorsSize, AnchorsCoordinateSystem.Default, CenterPivot, ChangeRectWithAnchors);
            changed = true;
        }

        if (!AnchorsCanvasPos.IsAproximately(AnchorsCanvasPosPrev))
        {
            Rt.SetAnchorsPosition(AnchorsCanvasPos, AnchorsCoordinateSystem.AsChildOfCanvas, CenterPivot, ChangeRectWithAnchors);
            changed = true;
        }
        
        if (!AnchorsOutOfCanvasPos.IsAproximately(AnchorsOutOfCanvasPosPrev))
        {
            Rt.SetAnchorsPosition(AnchorsOutOfCanvasPos, AnchorsCoordinateSystem.OutsideCanvas);
            changed = true;
        }

        if (!AnchorsOutOfContainerPos.IsAproximately(AnchorsOutOfContainerPosPrev))
        {
            Rt.SetAnchorsPosition(AnchorsOutOfContainerPos, AnchorsCoordinateSystem.OutsideContainer);
            changed = true;
        }
        
        if (!PositionIgnoringAnchorsRelative.IsAproximately(PositionIgnoringAnchorsRelativePrev))
        {
            Rt.SetPosition(PositionIgnoringAnchorsRelative, CoordinateSystem.IgnoreAnchorsAndPivotNormalized, CenterPivot);
            changed = true;
        }

        if (!SizeIgnoringAnchorsRelative.IsAproximately(SizeIgnoringAnchorsRelativePrev))
        {
            Rt.SetSize(SizeIgnoringAnchorsRelative, CoordinateSystem.IgnoreAnchorsAndPivotNormalized);
            changed = true;
        }

        if (!AnchorsInsideOfCanvasPos.IsAproximately(AnchorsInsideOfCanvasPosPrev))
        {
            Rt.SetAnchorsPosition(AnchorsInsideOfCanvasPos, AnchorsCoordinateSystem.InsideCanvas);
            changed = true;
        }

        if(!CanvasPos.IsAproximately(CanvasPosPrev))
        {
            Rt.SetPosition(CanvasPos, CoordinateSystem.AsChildOfCanvas, CenterPivot);
            changed = true;
        }

        if(!PixelPos.IsAproximately(PixelPositionPrev))
        {
            Rt.SetPosition(PixelPos, CoordinateSystem.ScreenSpacePixels, CenterPivot);
            changed = true;
        }

        if(!CanvasPosNormalized.IsAproximately(CanvasPosNormalizedPrev))
        {
            Rt.SetPosition(CanvasPosNormalized, CoordinateSystem.AsChildOfCanvasNormalized, CenterPivot);
            changed = true;
        }

        if(!SizeInPixels.IsAproximately(SizeInPixelsPrev))
        {
            Rt.SetSize(SizeInPixels, CoordinateSystem.ScreenSpacePixels, CenterPivot);
            changed = true;
        }

        if(!SizeAsCanvas.IsAproximately(SizeAsCanvasPrev))
        {
            Rt.SetSize(SizeAsCanvas, CoordinateSystem.AsChildOfCanvas, CenterPivot);
            changed = true;
        }

        if (!SizeAsCanvasNormalized.IsAproximately(SizeAsCanvasNormalizedPrev))
        {
            Rt.SetSize(SizeAsCanvasNormalized, CoordinateSystem.AsChildOfCanvasNormalized, CenterPivot);
            changed = true;
        }

        if (!AnchorsPixelsSize.IsAproximately(AnchorsPixelsSizePrev))
        {
            Rt.SetAnchorsSize(AnchorsPixelsSize, AnchorsCoordinateSystem.ScreenSpacePixels, CenterPivot, ChangeRectWithAnchors);
            changed = true;
        }

        if (!AnchorsCanvasSize.IsAproximately(AnchorsCanvasSizePrev))
        {
            Rt.SetAnchorsSize(AnchorsCanvasSize, AnchorsCoordinateSystem.AsChildOfCanvas, CenterPivot, ChangeRectWithAnchors);
            changed = true;
        }

        if(!AnchorsRectPos.IsAproximately(AnchorsRectPosPrev))
        {
            Rt.SetAnchorsPosition(AnchorsRectPos, AnchorsCoordinateSystem.AsRect, CenterPivot, ChangeRectWithAnchors);
            changed = true;
        }

        if(!AnchorsRectSize.IsAproximately(AnchorsRectSizePrev))
        {
            Rt.SetAnchorsSize(AnchorsRectSize, AnchorsCoordinateSystem.AsRect, CenterPivot, ChangeRectWithAnchors);
            changed = true;
        }

        return changed;
    }

    public void UpdateChanges()
    {
        PositionIgnoringAnchors         = Rt.GetPosition(CoordinateSystem.IgnoreAnchorsAndPivot, CenterPivot);
        SizeIgnoringAnchors             = Rt.GetSize(CoordinateSystem.IgnoreAnchorsAndPivot);
        AnchorsPosition                 = Rt.GetAnchorsPosition(AnchorsCoordinateSystem.Default, CenterPivot);
        AnchorsPixelPosition            = Rt.GetAnchorsPosition(AnchorsCoordinateSystem.ScreenSpacePixels, CenterPivot);
        AnchorsSize                     = Rt.GetAnchorsSize(AnchorsCoordinateSystem.Default);
        AnchorsCanvasPos                = Rt.GetAnchorsPosition(AnchorsCoordinateSystem.AsChildOfCanvas, CenterPivot);
        AnchorsOutOfCanvasPos           = Rt.GetAnchorsPosition(AnchorsCoordinateSystem.OutsideCanvas);
        AnchorsOutOfContainerPos        = Rt.GetAnchorsPosition(AnchorsCoordinateSystem.OutsideContainer);
        PositionIgnoringAnchorsRelative = Rt.GetPosition(CoordinateSystem.IgnoreAnchorsAndPivotNormalized, CenterPivot);
        SizeIgnoringAnchorsRelative     = Rt.GetSize(CoordinateSystem.IgnoreAnchorsAndPivotNormalized);
        AnchorsInsideOfCanvasPos        = Rt.GetAnchorsPosition(AnchorsCoordinateSystem.InsideCanvas);
        CanvasPos                       = Rt.GetPosition(CoordinateSystem.AsChildOfCanvas, CenterPivot);
        PixelPos                        = Rt.GetPosition(CoordinateSystem.ScreenSpacePixels, CenterPivot);
        CanvasPosNormalized             = Rt.GetPosition(CoordinateSystem.AsChildOfCanvasNormalized, CenterPivot);
        SizeInPixels                    = Rt.GetSize(CoordinateSystem.ScreenSpacePixels);
        SizeAsCanvas                    = Rt.GetSize(CoordinateSystem.AsChildOfCanvas);
        SizeAsCanvasNormalized          = Rt.GetSize(CoordinateSystem.AsChildOfCanvasNormalized);
        AnchorsCanvasSize               = Rt.GetAnchorsSize(AnchorsCoordinateSystem.AsChildOfCanvas);
        AnchorsPixelsSize               = Rt.GetAnchorsSize(AnchorsCoordinateSystem.ScreenSpacePixels);
        AnchorsRectPos                  = Rt.GetAnchorsPosition(AnchorsCoordinateSystem.AsRect, CenterPivot);
        AnchorsRectSize                 = Rt.GetAnchorsSize(AnchorsCoordinateSystem.AsRect);
    }

    public void SaveFrameData()
    {
        PositionIgnoringAnchorsPrev = PositionIgnoringAnchors;
        SizeIgnoringAnchorsPrev     = SizeIgnoringAnchors;
        AnchorsPositionPrev         = AnchorsPosition;
        AnchorsPixelPositionPrev    = AnchorsPixelPosition;
        AnchorsSizePrev             = AnchorsSize;
        AnchorsCanvasPosPrev        = AnchorsCanvasPos;
        AnchorsOutOfCanvasPosPrev   = AnchorsOutOfCanvasPos;
        AnchorsOutOfContainerPosPrev = AnchorsOutOfContainerPos;
        PositionIgnoringAnchorsRelativePrev = PositionIgnoringAnchorsRelative;
        SizeIgnoringAnchorsRelativePrev = SizeIgnoringAnchorsRelative;
        AnchorsInsideOfCanvasPosPrev = AnchorsInsideOfCanvasPos;
        CanvasPosPrev                = CanvasPos;
        PixelPositionPrev            = PixelPos;
        CanvasPosNormalizedPrev      = CanvasPosNormalized;
        SizeInPixelsPrev             = SizeInPixels;
        SizeAsCanvasPrev             = SizeAsCanvas;
        SizeAsCanvasNormalizedPrev   = SizeAsCanvasNormalized;
        AnchorsCanvasSizePrev        = AnchorsCanvasSize;
        AnchorsPixelsSizePrev        = AnchorsPixelsSize;
        AnchorsRectPosPrev           = AnchorsRectPos;
        AnchorsRectSizePrev          = AnchorsRectSize;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RectTransformExtended))]
public class RectTransformExtendedInspector : Editor
{
    [SerializeField]
    private bool                    HelpFolded;
    private RectTransformExtended   Script;

    public override void OnInspectorGUI()
    {
        Script = (RectTransformExtended)target;

        Script.GetReferences();

        Undo.RecordObject((RectTransform)Script.transform, "Changed RectTransformExtended");

        ShowScaleWarning();

        EditorGUILayout.Space();
        EditorGUIUtility.labelWidth = 140;
        Script.CenterPivot                      = EditorGUILayout.Toggle(new GUIContent("Center Pivot", "Pixel 0 of the object can be in the center or in the lower left corner.\n\nThis component ignores the RectTransform pivot and has his own pivot that you can reposition with this toggle." ), Script.CenterPivot);
        Script.AllowDistortion                  = EditorGUILayout.Toggle(new GUIContent("Allow Distortion", "Resize proportionaly. This can't be used from code." ), Script.AllowDistortion);
        EditorGUILayout.Space();

        
        EditorGUILayout.HelpBox("Rect control", MessageType.None);
        Script.SelectedCoordinates              = EditorGUILayout.Popup("Coordinates", Script.SelectedCoordinates, new string[]{"Ignore Anchors And Pivot", "Ignore Anchors And Pivot Normalized", "As Child Of Canvas", "As Child Of Canvas Normalized", "Screen Space Pixels"});

        if(Script.SelectedCoordinates == (int)CoordinateSystem.IgnoreAnchorsAndPivot)
        {
            Script.PositionIgnoringAnchors          = PositionControl(new GUIContent("Position", "To control the position using a value relative to the parent, ignoring anchors and pivot position. Without this the only option is to control position with a value relative to the anchors using RectTransform.AnchoredPosition. \n \nUsefull to ignore anchors in one or both axis."), Script.PositionIgnoringAnchors);
            Script.SizeIgnoringAnchors              = SizeControl(new GUIContent("Size", "To control the size using a value relative to the parent, ignoring anchors. Without this the only option is to control size with a value relative to the anchors using RectTransform.SizeDelta. \n \nUsefull to ignore anchors in one or both axis."), Script.SizeIgnoringAnchors, Script.AllowDistortion);
        }

        if(Script.SelectedCoordinates == (int)CoordinateSystem.IgnoreAnchorsAndPivotNormalized)
        {
            Script.PositionIgnoringAnchorsRelative  = PositionControl(new GUIContent("Position", "To control the position using a value relative to the parent with a range from 0 to 1, ignoring anchors. \n\nUsefull to divide the space of the parent between the children."), Script.PositionIgnoringAnchorsRelative);
            Script.SizeIgnoringAnchorsRelative      = SizeControl(new GUIContent("Size", "To control the size using a value relative to the parent with a range from 0 to 1, ignoring anchors. \n\nUsefull to divide the space of the parent between the children."), Script.SizeIgnoringAnchorsRelative, Script.AllowDistortion);
        }

        if(Script.SelectedCoordinates == (int)CoordinateSystem.AsChildOfCanvas)
        {
            Script.CanvasPos                        = PositionControl(new GUIContent("Position", "Move the element as if it was a child of the canvas when is not. \n\nUseful for example to match coordinates from objects located at different containers."), Script.CanvasPos);
            Script.SizeAsCanvas                     = SizeControl(new GUIContent("Size", "Resize the element as if it was a child of the canvas when is not. \n\nUseful for example to match coordinates from objects located at different containers."), Script.SizeAsCanvas, Script.AllowDistortion);
        }

        if(Script.SelectedCoordinates == (int)CoordinateSystem.AsChildOfCanvasNormalized)
        {
            Script.CanvasPosNormalized              = PositionControl(new GUIContent("Position", "This is The same than Position As Canvas Child but using values from 0 to 1 (normalized). Move the element as if it was a child of the canvas when is not. \n\nUseful for example to match coordinates from objects located at different containers."), Script.CanvasPosNormalized);
            Script.SizeAsCanvasNormalized           = SizeControl(new GUIContent("Size", "This is The same than Size As Canvas but using values from 0 to 1 (normalized). Resize the element as if it was a child of the canvas when is not. \n\nUseful for example to match coordinates from objects located at different containers."), Script.SizeAsCanvasNormalized, Script.AllowDistortion);
        }

        if(Script.SelectedCoordinates == (int)CoordinateSystem.ScreenSpacePixels)
        {
            Script.PixelPos                         = PositionControl(new GUIContent("Position", "To control the position in screen space pixels instead of units. \n \nUsefull when you want to control the position of an object using the mouse/touch position."), Script.PixelPos);
            Script.SizeInPixels                     = SizeControl(new GUIContent("Size", "To control the size in screen space pixels instead of units. \n\nUsefull when you want to control the size of an object using the mouse/touch position."), Script.SizeInPixels, Script.AllowDistortion);
        }
        EditorGUIUtility.labelWidth = 140;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Anchors control", MessageType.None);
        Script.SelectedAnchorCoordinates        = EditorGUILayout.Popup("Anchor Coordinates", Script.SelectedAnchorCoordinates, new string[]{"Default", "As Child Of Canvas", "Screen Space Pixels", "As Rect", "Outside Container", "Outside Canvas", "Inside Canvas"});

        if(Script.SelectedAnchorCoordinates == (int)AnchorsCoordinateSystem.Default)
        {
            Script.AnchorsPosition                  = PositionControl(new GUIContent("Anchors Position", "To control anchors position with a single Vector2 instead of AnchorsMin and AnchorsMax. \n \nUsefull for tweens and simplicity."), Script.AnchorsPosition);
            Script.AnchorsSize                      = SizeControl(new GUIContent("Anchors Size", "To control anchors size with a single Vector2 instead of AnchorsMin and AnchorsMax. \n \nUsefull for tweens and simplicity."), Script.AnchorsSize, Script.AllowDistortion);
        }

        if (Script.SelectedAnchorCoordinates == (int)AnchorsCoordinateSystem.AsChildOfCanvas)
        {
            Script.AnchorsCanvasPos                 = PositionControl(new GUIContent("Anchors Position", "To control the anchors position with values as if the object was chid of the canvas. \n \nUsefull when you want to set or get the position relative to the canvas but the object is not a child of the canvas."), Script.AnchorsCanvasPos);
            Script.AnchorsCanvasSize                = SizeControl(new GUIContent("Anchors Size", "To control the anchors size with values as if the object was chid of the canvas. \n \nUsefull when you want to set or get the size relative to the canvas but the object is not a child of the canvas."), Script.AnchorsCanvasSize, Script.AllowDistortion);
        }

        if (Script.SelectedAnchorCoordinates == (int)AnchorsCoordinateSystem.ScreenSpacePixels)
        {
            Script.AnchorsPixelPosition             = PositionControl(new GUIContent("Anchors Position", "To control anchors position in screen space pixels instead of anchor values from 0 to 1. \n \nUsefull when you want to control the position of an object using the mouse/touch position and you want to keep the anchors attached."), Script.AnchorsPixelPosition);
            Script.AnchorsPixelsSize                = SizeControl(new GUIContent("Anchors Size", "To control anchors size in screen space pixels instead of anchor values from 0 to 1. \n \nUsefull when you want to control the size of the anchors using the mouse/touch values."), Script.AnchorsPixelsSize, Script.AllowDistortion);
        }

        if (Script.SelectedAnchorCoordinates == (int)AnchorsCoordinateSystem.AsRect)
        {
            Script.AnchorsRectPos                   = PositionControl(new GUIContent("Anchors Position", "Move the anchors with the same kind of coordinates used for the rect in the IgnoreAnchorsAndPivot coordinate system. \n \nUsefull to fit the anchors with the rect in different ways."), Script.AnchorsRectPos);
            Script.AnchorsRectSize                  = SizeControl(new GUIContent("Anchors Size", "Resize the anchors with the same kind of coordinates used for the rect in the IgnoreAnchorsAndPivot coordinate system. \n \nUsefull to fit the anchors with the rect in different ways."), Script.AnchorsRectSize, Script.AllowDistortion);
        }

        if (Script.SelectedAnchorCoordinates == (int)AnchorsCoordinateSystem.OutsideCanvas)
        {
            Script.AnchorsOutOfCanvasPos            = PositionControl(new GUIContent("Anchors Position", "To control how much outside of the canvas is the object. 0 means outside of the canvas. Does not matter if the container is not the canvas, or any anchor configuration. \n \nUsefull to move and animate elements from or to the outside of the screen in a very easy way."), Script.AnchorsOutOfCanvasPos);
        }

        if (Script.SelectedAnchorCoordinates == (int)AnchorsCoordinateSystem.OutsideContainer)
        {
            Script.AnchorsOutOfContainerPos         = PositionControl(new GUIContent("Anchors Position", "To control how much outside of the container is the object. 0 means outside of the container. Does not matter the anchor configuration in the object or in the container. \n \nUsefull to move and animate elements from or to the outside of the container in a very easy way."), Script.AnchorsOutOfContainerPos);
        }

        if (Script.SelectedAnchorCoordinates == (int)AnchorsCoordinateSystem.InsideCanvas)
        {
            Script.AnchorsInsideOfCanvasPos         = PositionControl(new GUIContent("Anchors Position", "To control the object preventing to go outside of the screen. Does not matter the anchor configuration in the object or in the container. \n \nUsefull to make dragable UI elements that can't be dragged outside of the screen."), Script.AnchorsInsideOfCanvasPos);
        }

        EditorGUIUtility.labelWidth = 140;

        if(Script.SelectedAnchorCoordinates != (int)AnchorsCoordinateSystem.OutsideCanvas && Script.SelectedAnchorCoordinates != (int)AnchorsCoordinateSystem.InsideCanvas && Script.SelectedAnchorCoordinates != (int)AnchorsCoordinateSystem.OutsideContainer)
            Script.ChangeRectWithAnchors            = EditorGUILayout.Toggle(new GUIContent("Change Also Rect", "Check to move and resize the anchors and the rect at the same time."), Script.ChangeRectWithAnchors);

        if(Script.UpdateAndApply() && !Application.isPlaying)
            EditorSceneManager.MarkAllScenesDirty();

        EditorGUILayout.Space();
        HelpFolded = EditorGUILayout.Foldout(HelpFolded, "How to use this from code");
        if(HelpFolded)
            EditorGUILayout.HelpBox("To use these values from code don't use this component instead you must use the new methods added to RectTransform:\n \n" +
                                "RectTransform.GetPosition()\n" +
                                "RectTransform.SetPosition()\n\n" +
                                "RectTransform.GetAnchorsPosition()\n" +
                                "RectTransform.SetAnchorsPosition()\n\n" +
                                "RectTransform.SetPositionX()\n" +
                                "RectTransform.SetPositionY()\n\n" +
                                "RectTransform.SetWidth()\n" +
                                "RectTransform.SetHeight()\n\n" +
                                "RectTransform.SetAnchorsPositionX()\n" +
                                "RectTransform.SetAnchorsPositionY()\n\n" +
                                "RectTransform.SetAnchorsWidth()\n" +
                                "RectTransform.SetAnchorsHeight()\n\n" +
                                "Example:\n"+
                                "var rectTransf = GetComponent<RectTransform>();\n"+
                                "Vector2 size = rectTransf.GetSize(CoordinateSystem.IgnoreAnchorsAndPivot);\n\n"+
                                "If you need more specific coordinate conversion use the methods of the static classes: RteRectTools and RteAnchorTools"
                                , MessageType.None);
    }

    private Vector2 SizeControl(GUIContent content, Vector2 previousSize, bool allowDistortion = true)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(content, GUILayout.MinWidth(70), GUILayout.MaxWidth(228));
        EditorGUIUtility.labelWidth = 37;
        float width = EditorGUILayout.FloatField(new GUIContent("Width", content.tooltip), previousSize.x, GUILayout.Width(92), GUILayout.ExpandWidth(false));
        EditorGUIUtility.labelWidth = 42;
        float height = EditorGUILayout.FloatField(new GUIContent("Height", content.tooltip), previousSize.y, GUILayout.Width(95), GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();

        if(allowDistortion)
             return new Vector2(width, height);

        // Allow distortion support
        bool changedWidth  = false;
        bool changedHeight = false;

        if(!previousSize.x.IsApproximately(width))
            changedWidth = true;

        if(!previousSize.y.IsApproximately(height))
            changedHeight = true;

        float finalWidth  = width;
        float finalHeight = height;
        if(changedWidth)
            finalHeight = (previousSize.y / previousSize.x).FixNaN() * width;
        else if(changedHeight)
            finalWidth = (previousSize.x / previousSize.y).FixNaN() * height;

        return new Vector2(finalWidth, finalHeight);
    }

    private Vector2 PositionControl(GUIContent content, Vector2 value)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(content, GUILayout.MinWidth(70), GUILayout.MaxWidth(250));
        EditorGUIUtility.labelWidth = 15;
        float x = EditorGUILayout.FloatField(new GUIContent("X", content.tooltip), value.x, GUILayout.Width(70), GUILayout.ExpandWidth(false));
        EditorGUIUtility.labelWidth = 15;
        float y = EditorGUILayout.FloatField(new GUIContent("Y", content.tooltip), value.y, GUILayout.MinWidth(95), GUILayout.MaxWidth(95), GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();
        return new Vector2(x, y);
    }

    private void ShowScaleWarning()
    {
        RectTransform rt = Script.GetComponent<RectTransform>();
        if(!rt.localScale.IsAproximately(Vector3.one))
            EditorGUILayout.HelpBox("The scale of the object is not 1. This causes unexpected results since Width and Height does not represent the real visual size anymore.", MessageType.Warning);
    }
}
#endif

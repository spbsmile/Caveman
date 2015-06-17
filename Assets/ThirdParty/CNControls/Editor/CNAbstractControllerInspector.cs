using UnityEditor;

/// <summary>
/// Base inspector view for the CNAbstractController
/// If you implement a new CNControl, you can derive from it (take a look at joystick inspector)
/// </summary>
[CustomEditor(typeof(CNAbstractController))]
public class CNAbstractControllerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var cnAbstractController = (CNAbstractController)target;

        DisplayScriptField();
        DisplayAxisNames(cnAbstractController);
        DisplayAnchorTouchZoneSizeMargins(cnAbstractController);

        // Sometimes simple checks for changed properties simply don't work
        // We will just set our thing dirty (oh yeah)
        EditorUtility.SetDirty(cnAbstractController);
        SceneView.RepaintAll();
    }

    protected void DisplayAxisNames(CNAbstractController cnAbstractController)
    {
        cnAbstractController.AxisNameX = EditorGUILayout.TextField("X Axis:", cnAbstractController.AxisNameX);
        cnAbstractController.AxisNameY = EditorGUILayout.TextField("Y Axis:", cnAbstractController.AxisNameY);
    }

    protected void DisplayAnchorTouchZoneSizeMargins(CNAbstractController cnAbstractController)
    {
        cnAbstractController.Anchor = (CNAbstractController.Anchors)
            EditorGUILayout.EnumPopup("Anchor:", cnAbstractController.Anchor);
        
        cnAbstractController.TouchZoneSize = EditorGUILayout.Vector2Field("Touch Zone Size:",
            cnAbstractController.TouchZoneSize);
        cnAbstractController.Margins = EditorGUILayout.Vector2Field("Margins:", cnAbstractController.Margins);
    }

    protected void DisplayScriptField()
    {
        base.OnInspectorGUI();
    }
}



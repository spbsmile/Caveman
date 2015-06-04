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
        base.OnInspectorGUI();
        var cnAbstractController = (CNAbstractController)target;

        EditorGUI.BeginChangeCheck();
        
        cnAbstractController.Anchor = (CNAbstractController.Anchors) 
            EditorGUILayout.EnumPopup("Anchor:", cnAbstractController.Anchor);
        cnAbstractController.AxisNameX = EditorGUILayout.TextField("X Axis:", cnAbstractController.AxisNameX);
        cnAbstractController.AxisNameY = EditorGUILayout.TextField("Y Axis:", cnAbstractController.AxisNameY);
        cnAbstractController.TouchZoneSize = EditorGUILayout.Vector2Field("Touch Zone Size:",
            cnAbstractController.TouchZoneSize);
        cnAbstractController.Margins = EditorGUILayout.Vector2Field("Margins:", cnAbstractController.Margins);

        // We need to repaint because it recalculates touchzone size and position based on margins and anchor
        if (!EditorGUI.EndChangeCheck()) return;

        EditorUtility.SetDirty(cnAbstractController);
        SceneView.RepaintAll();
    }
}



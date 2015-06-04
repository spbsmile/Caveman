using UnityEditor;

[CustomEditor(typeof(CNJoystick))]
public class CNJoystickInspector : CNAbstractControllerInspector
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var cnJoystick = (CNJoystick)target;

        EditorGUI.BeginChangeCheck();

        cnJoystick.DragRadius = EditorGUILayout.FloatField("Drag radius:", cnJoystick.DragRadius);
        cnJoystick.SnapsToFinger = EditorGUILayout.Toggle("Snaps to finger:", cnJoystick.SnapsToFinger);
        cnJoystick.IsHiddenIfNotTweaking = EditorGUILayout.Toggle("Hide on release:", cnJoystick.IsHiddenIfNotTweaking);

        if (!EditorGUI.EndChangeCheck()) return;

        EditorUtility.SetDirty(cnJoystick);
        cnJoystick.OnEnable();
    }
}

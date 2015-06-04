using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CNThrowableTouchpad))]
public class CNThrowableTouchpadInspector : CNTouchpadInspector
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        var cnThrowTouchpad = (CNThrowableTouchpad)target;
        cnThrowTouchpad.SpeedDecay = EditorGUILayout.Slider("Speed decay:", cnThrowTouchpad.SpeedDecay, 0f, 1f);

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(cnThrowTouchpad);
    }
}

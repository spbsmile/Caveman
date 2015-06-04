using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CNTouchpad))]
public class CNTouchpadInspector : CNAbstractControllerInspector
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        var cnTouchpad = (CNTouchpad)target;
        cnTouchpad.IsAlwaysNormalized = EditorGUILayout.Toggle("Normalize:", cnTouchpad.IsAlwaysNormalized);

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(cnTouchpad);
    }
}

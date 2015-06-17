using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CNTouchpad))]
public class CNTouchpadInspector : CNAbstractControllerInspector
{
    public override void OnInspectorGUI()
    {
        var cnTouchpad = (CNTouchpad)target;

        EditorGUI.BeginChangeCheck();

        DisplayScriptField();

        cnTouchpad.IsStretched = EditorGUILayout.Toggle("Stretch:", cnTouchpad.IsStretched);

        if (!cnTouchpad.IsStretched)
        {
            DisplayAnchorTouchZoneSizeMargins(cnTouchpad);
        }
        else
        {
            DisplayAxisNames(cnTouchpad);
        }

        cnTouchpad.IsAlwaysNormalized = EditorGUILayout.Toggle("Normalize:", cnTouchpad.IsAlwaysNormalized);

        if (EditorGUI.EndChangeCheck())
        {
            cnTouchpad.OnEnable();
            EditorUtility.SetDirty(cnTouchpad);
        }
    }
}

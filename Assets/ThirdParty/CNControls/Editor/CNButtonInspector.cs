using UnityEditor;

[CustomEditor(typeof(CNButton))]
public class CNButtonInspector : CNAbstractControllerInspector
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        var cnButton = (CNButton)target;

        EditorGUI.BeginChangeCheck();

        // We don't show axis names
        // You can enable it, just uncomment the line in the middle
        DisplayScriptField();
        // DisplayAxisNames(cnButton);
        DisplayAnchorTouchZoneSizeMargins(cnButton);

        if (!EditorGUI.EndChangeCheck()) return;

        EditorUtility.SetDirty(cnButton);
        cnButton.OnEnable();
    }
}
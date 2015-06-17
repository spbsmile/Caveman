using UnityEngine;
using UnityEditor;

public class CNButtonCreatorMenu : EditorWindow
{
    [MenuItem("GameObject/Create Other/CNControls/Button")]
    static void CreateCNButton()
    {
        CNInputEditorTools.CreateControlFromPrefabsByName("CNButton");
    }
}
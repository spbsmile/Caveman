using UnityEditor;
using UnityEngine;

public class CNTouchpadCreatorMenu : EditorWindow
{
    [MenuItem("GameObject/Create Other/CNControls/Touchpad")]
    private static void CreateCNTouchpad()
    {
        CNInputEditorTools.CreateControlFromPrefabsByName("CNTouchpad");
    }
}

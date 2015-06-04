using UnityEditor;

public class CNThrowableTouchpadCreatorMenu : EditorWindow
{
    [MenuItem("GameObject/Create Other/CNControls/Throwable Touchpad")]
    private static void CreateCNTouchpad()
    {
        CNInputEditorTools.CreateControlFromPrefabsByName("CNThrowableTouchpad");
    }
}

using UnityEditor;

public class CNJoystickCreatorMenu : EditorWindow
{
    [MenuItem("GameObject/Create Other/CNControls/Joystick")]
    private static void CreateCNJoystick()
    {
        CNInputEditorTools.CreateControlFromPrefabsByName("CNJoystick");
    }
}

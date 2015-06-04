using UnityEditor;
using UnityEngine;

/// <summary>
/// Some tools for CNControls
/// </summary>
public class CNInputEditorTools : EditorWindow
{
    /// <summary>
    /// Gets or creates a CNControlCamera
    /// If it's already on the scene, it will just pass the reference
    /// If it's not, it will create one from the Prefab
    /// </summary>
    /// <returns>CNControlCamera object</returns>
    public static GameObject GetControlCamera()
    {
        GameObject cameraGo = GameObject.Find("CNControlCamera");

        if (cameraGo == null)
        {
            cameraGo = AssetDatabase.LoadAssetAtPath("Assets/CNControls/Prefabs/CNControlCamera.prefab", typeof(GameObject)) as GameObject;
            if (cameraGo == null)
            {
                throw new UnityException("Can't find CNControlCamera prefab. " +
                               "Asset Database may be corrupted, or you could've renamed or moved the folder and/or the prefab. " +
                               "Please reimport the package or change everything back");
            }

            cameraGo = GameObject.Instantiate(cameraGo,
                new Vector3(-50f, 0f, 0f),
                // Don't change the rotation, it's assumed that it's stays right up for calculation simplicity
                Quaternion.identity) as GameObject;
            cameraGo.name = "CNControlCamera";
        }
        return cameraGo;
    }

    /// <summary>
    /// Creates a CNControl by name
    /// All controls are stored as prefabs in the CNControls/Prefabs folder
    /// </summary>
    /// <param name="controlName">A name of the control to create. Should be one of the ones that are in the CNControls/Prefabs folder</param>
    public static void CreateControlFromPrefabsByName(string controlName)
    {
        GameObject cameraGo = CNInputEditorTools.GetControlCamera();

        var controlObject = AssetDatabase.LoadAssetAtPath(
            "Assets/CNControls/Prefabs/" + controlName + ".prefab", 
            typeof(GameObject)) as GameObject;

        if (controlObject == null)
        {
            throw new UnityException("Can't find " + controlName + " prefab. " +
                           "Asset Database may be corrupted, or you could've renamed or moved the folder and/or the prefab. " +
                           "Please reimport the package or change everything back");
        }

        // TODO Check for any CNControls on the scene and change the Anchor property of the new one accordingly

        GameObject instantiatedControl = GameObject.Instantiate(controlObject, Vector3.zero, Quaternion.identity) as GameObject;
        instantiatedControl.transform.parent = cameraGo.GetComponent<Transform>();
        instantiatedControl.name = controlName;
        instantiatedControl.GetComponent<CNAbstractController>().OnEnable();
    }
}

using Caveman.Weapons.Melee;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SwordModel))]
public class SwordModelEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        var model = (SwordModel)target;

        if (GUILayout.Button("Activate Weapon"))
        {
            model.Activate("", Vector2.zero, Vector2.zero);
        }

    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Caveman.CustomTools
{
    public class SceneSwitcher : EditorWindow
    {
        public List<string> scenes = new List<string>();
        public Vector2 scroll;

        [MenuItem("Tools/Scene Switcher")]
        public static void Open()
        {
            GetWindow<SceneSwitcher>("Scene Switch").minSize = new Vector2(100, 43);
        }

        public void OnGUI()
        {
            if (scenes.FirstOrDefault() != EditorApplication.currentScene)
            {
                scenes.RemoveAll(x => x == EditorApplication.currentScene);
                scenes.Insert(0, EditorApplication.currentScene);
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var scene in scenes)
            {
                GUI.enabled = scene != EditorApplication.currentScene;
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(scene) && EditorApplication.SaveCurrentSceneIfUserWantsTo())
                {
                    EditorApplication.OpenScene(scene);
                }
                if (GUILayout.Button("Play", GUILayout.Width(50)) && EditorApplication.SaveCurrentSceneIfUserWantsTo())
                {
                    EditorApplication.OpenScene(scene);
                    EditorApplication.isPlaying = true;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
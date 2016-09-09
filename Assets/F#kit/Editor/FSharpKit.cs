// hack the planet!
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;

public class FSharpKit : EditorWindow {
    public static bool showDebug = false;

    [MenuItem ("Window/F# kit")]
    public static void ShowWindow () {
        EditorWindow.GetWindow(typeof(FSharpKit), false, "F# Kit");
    }

    void OnGUI () {
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("F# kit by noobtuts.com", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(@"Usage Guide:
1. Right click in the Project Area, select Create->F# Script.
2. Open and modify it.
3. Wait for F# kit to rebuild.
4. Select your GameObject, click Add Component->Scripts to add it.)",
            MessageType.Info);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
        showDebug = EditorGUILayout.Toggle("Show Debug Log", showDebug);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Status", EditorStyles.boldLabel);

#if UNITY_EDITOR_OSX
        OnGuiOSX();
#elif UNITY_EDITOR_WIN
        OnGuiWin();
#else
        OnGuiLinux();
#endif

        //if (GUILayout.Button("Build manually")) Build();
        GUILayout.EndVertical();
    }

    void OnGuiOSX() {
        // mono
        GUILayout.BeginHorizontal();
        GUILayout.Label("Mono:");
        GUILayout.FlexibleSpace();
        if (Directory.Exists(FSharpBuildOSX.monoPath)) {
            GUILayout.Label("found");
        } else {            
            if (GUILayout.Button("Please download & install Mono")) {
                System.Diagnostics.Process.Start(FSharpBuildOSX.monoUrl);
            }
        }
        GUILayout.EndHorizontal();

        // xamarin
        GUILayout.BeginHorizontal();
        GUILayout.Label("Xamarin Studio:");
        GUILayout.FlexibleSpace();
        if (File.Exists(FSharpBuildOSX.xamarinPath)) {
            GUILayout.Label("found");
        } else {
            if (GUILayout.Button("Please download & install Xamarin Studio")) {
                System.Diagnostics.Process.Start(FSharpBuildOSX.xamarinUrl);
            }
        }
        GUILayout.EndHorizontal();

        // UnityEngine.dll
        GUILayout.BeginHorizontal();
        GUILayout.Label("UnityEngine.dll:");
        GUILayout.FlexibleSpace();
        GUILayout.Label((File.Exists(FSharpBuildOSX.unityenginedllPath) ? "found" : "missing"));
        GUILayout.EndHorizontal();

    }

    void OnGuiWin() {
        // .Net Framework 4.5
        GUILayout.BeginHorizontal();
        GUILayout.Label(".NET Framework 4.5:");
        GUILayout.FlexibleSpace();
        if (Directory.Exists(FSharpBuildWindows.dotnetPath)) {
            GUILayout.Label("found");
        } else {
            if (GUILayout.Button("Please download & install .NET Framework 4.5")) {
                System.Diagnostics.Process.Start(FSharpBuildWindows.dotnetUrl);
            }
        }
        GUILayout.EndHorizontal();

        // GTK# for .NET
        GUILayout.BeginHorizontal();
        GUILayout.Label("GTK# for .NET:");
        GUILayout.FlexibleSpace();
        if (Directory.Exists(FSharpBuildWindows.gtksharpPath)) {
            GUILayout.Label("found");
        } else {
            if (GUILayout.Button("Please download & install GTK# for .NET")) {
                System.Diagnostics.Process.Start(FSharpBuildWindows.gtksharpUrl);
            }
        }
        GUILayout.EndHorizontal();

        // F# Bundle
        GUILayout.BeginHorizontal();
        GUILayout.Label("F# Bundle 3.1:");
        GUILayout.FlexibleSpace();
        if (Directory.Exists(FSharpBuildWindows.fsharpbundlePath)) {
            GUILayout.Label("found");
        } else {
            if (GUILayout.Button("Please download & install F# Bundle 3.1")) {
                System.Diagnostics.Process.Start(FSharpBuildWindows.fsharpbundleUrl);
            }
        }
        GUILayout.EndHorizontal();

        // xamarin
        GUILayout.BeginHorizontal();
        GUILayout.Label("Xamarin Studio:");
        GUILayout.FlexibleSpace();
        if (File.Exists(FSharpBuildWindows.xamarinPath)) {
            GUILayout.Label("found");
        } else {
            if (GUILayout.Button("Please download & install Xamarin Studio")) {
                System.Diagnostics.Process.Start(FSharpBuildWindows.xamarinUrl);
            }
        }
        GUILayout.EndHorizontal();

        // UnityEngine.dll
        GUILayout.BeginHorizontal();
        GUILayout.Label("UnityEngine.dll:");
        GUILayout.FlexibleSpace();
        GUILayout.Label((File.Exists(FSharpBuildWindows.unityenginedllPath) ? "found" : "missing"));
        GUILayout.EndHorizontal();
    }

    void OnGuiLinux() {
        GUILayout.Label("Operating System not supported yet");
    }

    // LogEntries.Clear() is hidden, so we have to use reflection
    static void ClearConsole() {
         var entries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
         var fn = entries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
         fn.Invoke(null, null);
     }

    static string SelectionPath() {
        // find all selected assets and return the path of one of them
        var sel = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        if (sel.Length > 0) {
            var fpath = AssetDatabase.GetAssetPath(sel[0]);            
            if (!string.IsNullOrEmpty(fpath) && File.Exists(fpath))
                return Path.GetDirectoryName(fpath);
        }

        // use Assets folder if nothing is selected
        return "Assets";
    }
    
    [MenuItem ("Assets/Create/F# Script")]
    public static void Create () {
        //File.Create("Assets/Script.fs").Dispose();
        var content = @"namespace FSharp
open UnityEngine
 
type NewBehaviourScript() = 
    inherit MonoBehaviour()
    member this.Start() = Debug.Log(""Hello World"")";
        File.WriteAllText(SelectionPath() + "/NewBehaviourScript.fs", content);
        AssetDatabase.Refresh();

        // TODO create it in the selected folder with name editing:
        //var DoCreateScriptAsset = Type.GetType("UnityEditor.ProjectWindowCallback.DoCreateScriptAsset, UnityEditor");    
        //ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
        //    0,
        //    ScriptableObject.CreateInstance(DoCreateScriptAsset) as UnityEditor.ProjectWindowCallback.EndNameEditAction,
        //    "Script.fs",
        //    null,
        //    null);
    }

    public static void Build() {
        // clear previous errors
        ClearConsole();

        // show progress bar so that the user knows what's happening
        EditorUtility.DisplayProgressBar("F# kit", "Building F# Scripts...", 1f);

        // wrap the build in a try/finally, otherwise errors will cause the
        // progress bar to never disappear, hence Unity becomes unusable
        try {
#if UNITY_EDITOR_OSX
            // xamarin missing?
            if (!File.Exists(FSharpBuildOSX.xamarinPath))
                Debug.LogError("Xamarin Studio is missing. Please download & install: " + FSharpBuildOSX.xamarinUrl);
            // mono missing?
            else if (!Directory.Exists(FSharpBuildOSX.monoPath))
                Debug.LogError("Mono is missing. Please download & install: " + FSharpBuildOSX.monoUrl);
            // unityengine.dll missing?
            else if (!File.Exists(FSharpBuildOSX.unityenginedllPath))
                Debug.LogError("UnityEngine.dll is missing. Please install it in the default directory.");
            // otherwise build
            else
                FSharpBuildOSX.Build();
#elif UNITY_EDITOR_WIN
            // .NET missing?
            if (!Directory.Exists(FSharpBuildWindows.dotnetPath))
                Debug.LogError(".NET Framework 4.5 is missing. Please download & install: " + FSharpBuildWindows.dotnetUrl);
            // GTK# missing?
            else if (!Directory.Exists(FSharpBuildWindows.gtksharpPath))
                Debug.LogError("GTK# for .NET is missing. Please download & install: " + FSharpBuildWindows.gtksharpUrl);
            // f# bundle missing?
            else if (!Directory.Exists(FSharpBuildWindows.fsharpbundlePath))
                Debug.LogError("F# bundle 3.1 is missing. Please download & install: " + FSharpBuildWindows.fsharpbundleUrl);
            // xamarin missing?
            else if (!File.Exists(FSharpBuildWindows.xamarinPath))
                Debug.LogError("Xamarin Studio is missing. Please download & install: " + FSharpBuildWindows.xamarinUrl);
            // unityengine.dll missing?
            else if (!File.Exists(FSharpBuildWindows.unityenginedllPath))
                Debug.LogError("UnityEngine.dll is missing. Please install it in the default directory.");
            // otherwise build
            else
                FSharpBuildWindows.Build();
#else
            Debug.Log("Operating System not supported yet.");
#endif
        } finally {
            EditorUtility.ClearProgressBar();
        }
    }
}

// detect asset changes
class MyAllPostprocessor : AssetPostprocessor {
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
        // any .fs script that was reimported or deleted?
        if (importedAssets.Any(s => s.EndsWith(".fs")) || deletedAssets.Any(s => s.EndsWith(".fs")))
            FSharpKit.Build();
    }
}
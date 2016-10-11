using UnityEngine;
using System;
using System.IO;

public class FSharpBuildLinux {
    
    public static void Build() {
        /*try {
            // get all F# source files
            string[] fsharps = Directory.GetFiles(".", "*.fs", SearchOption.AllDirectories);
            foreach (string file in fsharps) {
                Debug.Log(file);
                string script = Path.GetFileNameWithoutExtension(file);
                string outputFile = Path.ChangeExtension(Path.Combine (FSCompilerOptions.outputDir, script), "dll");
                // only compile if source file is newer than it's dll
                if (File.GetLastWriteTime(file) > File.GetLastWriteTime(outputFile)) {
                    Debug.Log("Compiling " + script);
                    
                }
                else if (FSCompilerOptions.outputUpToDate) {
                    Debug.Log(script + " is up to date");
                }
            }
        }
        catch (DirectoryNotFoundException ex) {
            Debug.Log("Error on finding F#s: " + ex.Message);
        }*/
    }
}

namespace FSharp
open UnityEngine
 
type HelloWorld() = 
    inherit MonoBehaviour()
    member this.Start() = Debug.Log("Hello World")


    
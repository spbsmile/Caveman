namespace FSharp
open UnityEngine
 
type Block() = 
    inherit MonoBehaviour()

    // destroy self on collision
    member this.OnCollisionEnter2D(collisionInfo : Collision2D) =
        GameObject.Destroy this.gameObject
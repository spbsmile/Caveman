namespace FSharp
open UnityEngine
 
type Racket() = 
    inherit MonoBehaviour()
    
    [<SerializeField>]
    let mutable speed = 150.0f

    member this.FixedUpdate() =
        let h = Input.GetAxisRaw("Horizontal")
        this.GetComponent<Rigidbody2D>().velocity <- Vector2.right * h * speed
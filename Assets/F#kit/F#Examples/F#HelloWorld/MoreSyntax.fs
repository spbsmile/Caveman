namespace FSharp
open UnityEngine
 
type MoreSyntax() = 
    inherit MonoBehaviour()
    
    // just a variable
    let a = 1

    // a variable with a specified type
    let f : float = 0.0

    // a mutable variable that is shown in the Inspector
    let mutable b = 2

    // a mutable variable that is shown in the Inspector
    [<SerializeField>]
    let mutable c = 3

    // creating a simple function
    let square n = n * n

    // using the function
    let nine = square 3

    // creating a list (can't be shown in the Inspector yet)
    let lis = [ 1; 2; 3 ]

    // creating an array that is shown in the Inspector
    [<SerializeField>]
    let mutable arr = [| 1; 2; 3 |]

    // a tuple
    let tu = (1, "a")

    // implementing the Start function
    member this.Start() =
        // logging
        Debug.Log("Hello World")

        // modify a mutable variable
        c <- 0

        // accessing the .NET library
        let r = System.DateTime.Now

        // accessing Unity components (always via 'this.')
        let pos = this.transform.position

        // using the generic GetComponent function
        let tr = this.GetComponent<Transform>()
        let tr2 = this.GetComponent(typeof<Transform>)

        // end the Start function without a return value
        ()
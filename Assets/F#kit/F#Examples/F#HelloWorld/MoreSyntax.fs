namespace FSharp
open UnityEngine
 
type MoreSyntax() = 
    inherit MonoBehaviour()

    // basics //////////////////////////////////////////////////////////////////
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


    // functional programming //////////////////////////////////////////////////
    let r = List.reduce (fun acc elem -> acc + elem) [1; 2; 3] // => 6
    let f = List.fold (fun acc elem -> acc + elem) 0 [1; 2; 3] // => 6
    let m = List.map (fun x -> x * 2) [1; 2; 3] // => [2; 4; 6]

    // sum via fold
    // sum [1; 2; 3] => 6
    let sum list = List.fold (fun acc elem -> acc + elem) 0 list

    // sum via recursion
    // sumr [1; 2; 3] => 6
    let rec sumr list =
        match list with
        | [] -> 0
        | x :: xs -> x + sumr xs

    // sum via endrecursion
    // sumer [1; 2; 3] 0 => 6
    let rec sumer list acc =
        match list with
        | [] -> acc
        | x :: xs -> sumer xs (x + acc)

    // partial
    // inc 2 => 3
    let inc = (+) 1
    

    // member functions ////////////////////////////////////////////////////////
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
namespace FSharp
open UnityEngine

type Ball() = 
    inherit MonoBehaviour()
    
    [<SerializeField>]
    let mutable speed = 100.0f

    let hitFactor(ballPos : Vector3, racketPos : Vector3, racketWidth : float32) =
        // ascii art:
        //
        // 1  -0.5  0  0.5   1  <- x value
        // ===================  <- racket
        //
        (ballPos.x - racketPos.x) / racketWidth

    member this.Start() =
        this.GetComponent<Rigidbody2D>().velocity <- Vector2.up * speed


    member this.OnCollisionEnter2D(col : Collision2D) =
        // hit the racket?
        if col.gameObject.name = "racket" then
            // calculate hit hitFactor
            let x = hitFactor(this.transform.position, col.transform.position, col.collider.bounds.size.x)

            // calculate direction, set length to 1
            let dir = (new Vector2(x, 1.0f)).normalized

            // set velocity with dir * speed
            this.GetComponent<Rigidbody2D>().velocity <- dir * speed
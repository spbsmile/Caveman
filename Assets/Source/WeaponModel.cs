using Caveman;
using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    private float speed = 1.5f;
    private Vector2 target;
    private Vector2 delta;

	public void Update () 
    {
	    if (delta.magnitude > 0.1f)
	    {
            transform.position = new Vector3(transform.position.x + delta.x * Time.deltaTime,
                transform.position.y + delta.y * Time.deltaTime); 
	    }
	}

    public void OnCollisionEnter2D(Collision2D coll)
    {

    }

    public void Move(Vector3 position, Vector2 positionTarget)
    {
        target = positionTarget;
        delta = UnityExtensions.CalculateDelta(position, positionTarget, speed);
    }
}

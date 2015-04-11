using Caveman;
using Caveman.Players;
using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    public Player owner;
    
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

    public void Move(Player player, Vector3 positionStart, Vector2 positionTarget)
    {
        owner = player;
        transform.position = positionStart;
        target = positionTarget;
        delta = UnityExtensions.CalculateDelta(positionStart, positionTarget, speed);
    }
}

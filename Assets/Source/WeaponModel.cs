using Caveman;
using Caveman.Players;
using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    private const float Speed = 2.8f;

    public Player owner;
   
    private Vector2 target;
    private Vector2 delta;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

	public void Update () 
    {
	    if (delta.magnitude > UnityExtensions.ThresholdPosition)
	    {
	        if (Vector2.Distance(target, transform.position) > UnityExtensions.ThresholdPosition)
	        {
	             transform.position = new Vector3(transform.position.x + delta.x * Time.deltaTime,
                transform.position.y + delta.y * Time.deltaTime); 
                transform.Rotate(Vector3.forward, 10);
	        }
	        else
	        {
	            Destroy();
	        }
	    }
	}
   

    public void Move(Player player, Vector3 positionStart, Vector2 positionTarget)
    {
        owner = player;
        transform.position = positionStart;
        target = positionTarget;
        delta = UnityExtensions.CalculateDelta(positionStart, positionTarget, Speed);
    }

    private void Destroy()
    {
        //stoneCrash
        Destroy(gameObject);
    }
}

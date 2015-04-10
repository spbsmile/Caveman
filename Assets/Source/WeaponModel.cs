using UnityEngine;

public class WeaponModel : MonoBehaviour 
{

	public void Start () 
    {
	    
	}
	
	public void Update () 
    {
	
	}

    public void OnCollisionEnter2D(Collision2D coll)
    {
        print("OnCollisionEnter2D");
        if (coll.gameObject.tag == "Enemy")
            coll.gameObject.SendMessage("ApplyDamage", 10);

    }
}

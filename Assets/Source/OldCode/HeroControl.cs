using UnityEngine;
using System.Collections;

public class HeroControl : MonoBehaviour
{
	public GameObject obj;

	int speed = 100;

	float vx = 0;
	float vy = 0;
	float targetX = 0;
	float targetY = 0;


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		var pos = obj.transform.position;
		pos.x = pos.x + vx * Time.deltaTime;
		pos.y = pos.y + vy * Time.deltaTime;
		obj.transform.position = pos;
	}

	public void Move ()
	{
		var pos = Input.mousePosition;
		Debug.Log (pos);
		targetX = pos.x;
		targetY = pos.y;

		var heroPos = obj.transform.position;

		float dx = targetX - heroPos.x;
		float dy = targetY - heroPos.y;
		float d = Mathf.Sqrt (dx * dx + dy * dy);

		if (d != 0) {
			vx = (dx / d) * speed;
			vy = (dy / d) * speed;
		}
	}
}	

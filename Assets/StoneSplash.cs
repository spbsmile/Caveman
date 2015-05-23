using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

public class StoneSplash : MonoBehaviour
{
    private Vector2 delta;
    private Vector2 target;

    private bool afterInit; 
    
    public void Update () 
    {
        if (delta.magnitude > UnityExtensions.ThresholdPosition)
        {
            if (Vector2.Distance(target, transform.position) > UnityExtensions.ThresholdPosition)
            {
                transform.position = new Vector2(transform.position.x + delta.x * Time.deltaTime,
                    transform.position.y + delta.y * Time.deltaTime);
                transform.Rotate(Vector3.forward, Settings.RotateStoneParameter);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (afterInit)
            {
                Destroy(gameObject);    
            }
        }
	}

    public void Init(int i, Vector2 position)
    {
        transform.position = position;
        if (i == 0)
        {
            target = position + (0.5f)*Vector2.right;
        }
        if (i == 1)
        {
            target = position - (0.5f) * Vector2.right;
        }
        if (i == 2)
        {
            target = position + (0.5f) * Vector2.up;
        }
        if (i == 3)
        {
            target = position - (0.5f) * Vector2.up;
        }
        afterInit = true;
        delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedWeapon);
    }
}

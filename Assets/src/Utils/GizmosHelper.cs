using UnityEngine;

public class GizmosHelper : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right);


        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up);
    }
}
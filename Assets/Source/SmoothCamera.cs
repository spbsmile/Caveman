using UnityEngine;

namespace Caveman
{
    public class SmoothCamera : MonoBehaviour
    {
        public float dampTime = 0.15f;
        public Transform target;

        private Vector3 velocity = Vector3.zero;

        public void Update()
        {
            if (target)
            {
                var point = Camera.main.WorldToViewportPoint(target.position);
                var delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                    //(new Vector3(0.5, 0.5, point.z));
                var destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
        }
    }
}



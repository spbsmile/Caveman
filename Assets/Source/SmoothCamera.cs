using System;
using UnityEngine;
using Caveman.Setting;

namespace Caveman
{
    public class SmoothCamera : MonoBehaviour
    {
        private const float DampTime = 0.15f;
        
        public Transform target;

        private Vector3 velocity = Vector3.zero;
        private float criticalX;
        private float criticalY;

        public void Start()
        {
            var section = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
            criticalX = Settings.BoundaryEndMap - section.x;
            criticalY = Settings.BoundaryEndMap - section.y;
        }

        public void Update()
        {
            if (target)
            {
                var point = Camera.main.WorldToViewportPoint(target.position);
                var delta = Vector3.zero;
                var indent = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));

                var currX = target.position.x;
                var currY = target.position.y;

                if (Math.Abs(currX) > criticalX && Math.Abs(currY) > criticalY)
                {
                    delta = new Vector3(Math.Sign(currX)*criticalX, Math.Sign(currY)*criticalY)
                            - indent;
                }
                else
                {
                    if (Math.Abs(currX) > criticalX)
                    {
                        delta = new Vector3(Math.Sign(currX) * criticalX, currY)
                           - indent;
                    }
                    else
                    {
                        if (Math.Abs(currY) > criticalY)
                        {
                            delta = new Vector3(currX, Math.Sign(currY)*criticalY)
                           - indent;
                        }
                        else
                        {
                            delta = target.position - indent;
                        }
                    }
                }
                var destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
        }
    }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawSphere(new Vector3(Settings.BoundaryEndMap, 0, 0), 0.5f);

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(new Vector3(criticalX, 0, 0), 0.5f);

        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawSphere(new Vector3(0, Settings.BoundaryEndMap, 0), 0.5f);

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(new Vector3(0, criticalY, 0), 0.5f);

        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawSphere(new Vector3(-Settings.BoundaryEndMap, 0, 0), 0.5f);

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(new Vector3(-criticalX, 0, 0), 0.5f);
        //}
    }
}



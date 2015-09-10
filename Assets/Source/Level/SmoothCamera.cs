using Caveman.Players;
using UnityEngine;
using Caveman.Setting;

namespace Caveman.Level
{
    public class SmoothCamera : MonoBehaviour
    {
        public Transform target;

        private const float DampTime = 0.15f;
        
        private Vector3 velocity = Vector3.zero;
        private float criticalX;
        private float criticalY;
        private Vector2 section;
        private PlayerModelBase player;

        public void Start()
        {
            section = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
            criticalX = (Settings.WidthMap - section.x);
            criticalY = (Settings.HeightMap - section.y);
            if (target != null) player = target.GetComponent<PlayerModelBase>();
        }

        public void LateUpdate()
        {
            if (target && player.enabled)
            {
                var point = Camera.main.WorldToViewportPoint(target.position);
                var delta = Vector3.zero;
                var indent = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));

                var currX = target.position.x;
                var currY = target.position.y;

                if (currX > criticalX && currY > criticalY)
                {
                    delta = new Vector3(criticalX, criticalY)
                            - indent;
                }
                else
                {
                    if (currX < section.x && currY < section.y)
                    {
                        delta = new Vector3(section.x, section.y)
                                - indent;
                    }
                    else
                    {
                        if (currX < section.x && currY > criticalY)
                        {
                            delta = new Vector3(section.x, criticalY)
                                    - indent;
                        }
                        else
                        {
                            if (currX > criticalX && currY < section.y)
                            {
                                delta = new Vector3(criticalX, section.y)
                                        - indent;
                            }
                            else
                            {
                                if (currX < section.x)
                                {
                                    delta = new Vector3(section.x, currY)
                                            - indent;
                                }
                                else
                                {
                                    if (currX > criticalX)
                                    {
                                        delta = new Vector3(criticalX, currY)
                                                - indent;
                                    }
                                    else
                                    {
                                        if (currY < section.y)
                                        {
                                            delta = new Vector3(currX, section.y)
                                                    - indent;
                                        }
                                        else
                                        {
                                            if (currY > criticalY)
                                            {
                                                delta = new Vector3(currX, criticalY)
                                                        - indent;
                                            }
                                            else
                                            {
                                                delta = target.position - indent;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                var destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
            }
        }

        public void SetPlayer(PlayerModelBase player)
        {
            this.player = player;
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawSphere(new Vector3(Settings.HeightMap, 0, 0), 0.5f);

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(new Vector3(criticalX, 0, 0), 0.5f);

        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawSphere(new Vector3(0, Settings.HeightMap, 0), 0.5f);

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(new Vector3(0, criticalY, 0), 0.5f);

        //    //Gizmos.color = Color.yellow;
        //    //Gizmos.DrawSphere(new Vector3(-Settings.BoundaryEndMap, 0, 0), 0.5f);

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(new Vector3(-criticalX, 0, 0), 0.5f);
        //}
    }
}



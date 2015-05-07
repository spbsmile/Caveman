using Caveman.Players;
using UnityEngine;

namespace Caveman
{
    public class WeaponModel : MonoBehaviour
    {
        public Player owner;

        private Vector2 target;
        private Vector2 delta;

        public void Update()
        {
            if (delta.magnitude > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.Distance(target, transform.position) > UnityExtensions.ThresholdPosition)
                {
                    transform.position = new Vector3(transform.position.x + delta.x*Time.deltaTime,
                        transform.position.y + delta.y*Time.deltaTime);
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
            delta = UnityExtensions.CalculateDelta(positionStart, positionTarget, Settings.SpeedWeapon);
        }

        private void Destroy()
        {
            //stoneCrash
            Destroy(gameObject);
        }
    }
}

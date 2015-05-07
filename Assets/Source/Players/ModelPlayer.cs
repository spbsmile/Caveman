using UnityEngine;

namespace Caveman.Players
{
    public class ModelPlayer : ModelBasePlayer
    {
        public void Update()
        {
            ThrowStoneOnTimer();

            if (Input.GetMouseButton(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
                animator.SetFloat("Speed", Settings.SpeedPlayer);
            }
            if (MoveStop())
            {
                animator.SetFloat("Speed", 0);
            }
            else
            {
                 Move();   
            }
        }
    }
}

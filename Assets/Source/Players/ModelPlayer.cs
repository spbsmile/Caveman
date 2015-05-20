using Assets.Source.Settings;
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
                animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, 0);
                delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
                animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
            }
            if (MoveStop())
            {
                //переход в стоячее положение внутри animatora
                //animator.SetFloat("Speed", 0);
            }
            else
            {
                Move();
            }
        }
    }
}

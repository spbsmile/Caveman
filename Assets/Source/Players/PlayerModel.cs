using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModel : BasePlayerModel
    {
        public void Update()
        {
            ThrowStoneOnTimer();

            if (Input.GetMouseButton(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, 0);
                delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
                InMotion = true;
                animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
            }
            if (InMotion)
            {
                Move();
            }
        }
    }
}

using Caveman.Setting;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModel : BasePlayerModel
    {
        public void Update()
        {
            ThrowStoneOnTimer();

            //if (Input.GetMouseButton(0))
            //{
            //    target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, 0);
            //    delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
            //    InMotion = true;
            //    animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
            //}
            //if (InMotion)
            //{
            //    Move();
            //}
        }

        public void SetJoystick(CNAbstractController movementJoystick)
        {
            movementJoystick.ControllerMovedEvent += MovePlayer;
        }

        private void MovePlayer(Vector3 movement, CNAbstractController arg2)
        {
            var position = new Vector3(transform.position.x + movement.x * Time.deltaTime * Settings.SpeedPlayer,
               transform.position.y + movement.y * Time.deltaTime * Settings.SpeedPlayer);
            transform.position = position;
            animator.SetFloat(movement.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
        }
    }
}

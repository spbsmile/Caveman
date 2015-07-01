using Caveman.Setting;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModel : PlayerModelBase
    {
        public void SetJoystick(CNAbstractController movementJoystick)
        {
            movementJoystick.ControllerMovedEvent += MovePlayer;
        }

        private void MovePlayer(Vector3 movement, CNAbstractController arg2)
        {
            var position = new Vector3(transform.position.x + movement.x * Time.deltaTime * Speed,
               transform.position.y + movement.y * Time.deltaTime * Speed);
            transform.position = position;
            animator.SetFloat(movement.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Speed);
        }
    }
}

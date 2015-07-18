using Caveman.Setting;
using Caveman.UI;
using UnityEngine;

namespace Caveman.Players
{
    public class HumanPlayerModel : PlayerModelClient
    {
        public void Awake()
        {
            BattleGui.instance.movementJoystick.ControllerMovedEvent += MovePlayer;
        }

        private void MovePlayer(Vector3 movement, CNAbstractController arg2)
        {
           // if (multiplayer) serverConnection.SendMove(movement);
            var position = new Vector3(transform.position.x + movement.x * Time.deltaTime * Speed,
               transform.position.y + movement.y * Time.deltaTime * Speed);
            transform.position = position;

            animator.SetFloat(movement.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Speed);
        }
    }
}

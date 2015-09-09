using System.Collections;
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

        protected override void Start()
        {
            base.Start();
            if (multiplayer) StartCoroutine(SendMove());
        }

        private IEnumerator SendMove()
        {
            yield return new WaitForSeconds(5f);
            serverConnection.SendMove(transform.position);
            StartCoroutine(SendMove());
        }

        private void MovePlayer(Vector3 movement, CNAbstractController arg2)
        {
            var position = new Vector3(transform.position.x + movement.x * Time.deltaTime * Speed,
               transform.position.y + movement.y * Time.deltaTime * Speed);

            if (position.x < 0.01f || position.y < 0.01) return;
            if (position.x > Settings.WidthMap - 0.01f || position.y > Settings.HeightMap) return;

            transform.position = position;

            animator.SetFloat(movement.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Speed);
        }
    }
}

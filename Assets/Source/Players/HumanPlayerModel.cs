using System.Collections;
using Caveman.Setting;
using Caveman.UI;
using UnityEngine;

namespace Caveman.Players
{
    public class HumanPlayerModel : PlayerModelClient
    {
        protected override void Awake()
        {
            base.Awake();
            BattleGui.instance.movementJoystick.ControllerMovedEvent += MovePlayer;
            BattleGui.instance.movementJoystick.FingerLiftedEvent += controller => StopMove();
        }

        protected override void Start()
        {
            base.Start();
            if (multiplayer) StartCoroutine(SendMove());
        }

        private IEnumerator SendMove()
        {
            yield return new WaitForSeconds(0.3f);
            serverConnection.SendMove(transform.position);
            StartCoroutine(SendMove());
        }

        private void MovePlayer(Vector3 direction, CNAbstractController arg2)
        {
            delta = direction*specification.Speed;
            Move();
        }

        protected override void Move()
        {
            var position = new Vector3(transform.position.x + delta.x * Time.deltaTime,
                transform.position.y + delta.y * Time.deltaTime);
            if (position.x < 0.01f || position.y < 0.01) return;
            if (position.x > Settings.WidthMap - 0.01f || position.y > Settings.HeightMap) return;
            transform.position = position;

            playerAnimation.SetMoving(delta.y < 0, delta.x > 0);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (multiplayer) StartCoroutine(SendMove());
        }
    }
}

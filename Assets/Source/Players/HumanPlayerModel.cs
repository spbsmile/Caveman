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
            delta = direction*Speed;
            Move();
        }

        protected override void Move()
        {
            var x = transform.position.x + delta.x * Time.deltaTime;
            var y = transform.position.y + delta.y * Time.deltaTime;


            // check movement out field
            var halfX = spriteRenderer.bounds.size.x / 2;
            if (x < halfX)
                x = halfX;
            else if (x > Settings.WidthMap - halfX)
                x = Settings.WidthMap - halfX;

            var halfY = spriteRenderer.bounds.size.y / 2;
            if (y < halfY)
                y = halfY;
            else if (y > Settings.HeightMap)
                y = Settings.HeightMap;


            transform.position = new Vector3(x, y);

            playerAnimation.SetMoving(delta.y < 0, delta.x > 0);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (multiplayer) StartCoroutine(SendMove());
        }
    }
}

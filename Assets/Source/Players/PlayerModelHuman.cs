using System.Collections;
using Caveman.Setting;
using Caveman.UI;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModelHuman : PlayerModelClient
    {
        protected override void Awake()
        {
            base.Awake();
            BattleGui.instance.movementJoystick.ControllerMovedEvent += MovePlayer;
            BattleGui.instance.movementJoystick.FingerLiftedEvent += controller => StopMove();
        }

        protected void Start()
        {
            if (multiplayer) StartCoroutine(SendMove());
        }
        /*
        public override void Play()
        {
            base.Play();
            if (multiplayer) StartCoroutine(SendMove());
        }
        */
        private IEnumerator SendMove()
        {
            yield return new WaitForSeconds(0.3f);
            serverNotify.Move(transform.position);
            StartCoroutine(SendMove());
        }

        private void MovePlayer(Vector3 direction, CNAbstractController arg2)
        {
            moveUnit = direction*Speed;
            Move();
        }

        protected override void Move()
        {
            var x = transform.position.x + moveUnit.x * Time.deltaTime;
            var y = transform.position.y + moveUnit.y * Time.deltaTime;


            // check movement out field
            var halfX = spriteRenderer.bounds.size.x / 2;
            if (x < halfX)
                x = halfX;
            else if (x > Settings.WidthMap - halfX)
                x = Settings.WidthMap - halfX;

            var halfY = spriteRenderer.bounds.size.y / 2;
            if (y < halfY)
                y = halfY;
            else if (y > Settings.HeightMap - halfY)
                y = Settings.HeightMap - halfY;


            transform.position = new Vector3(x, y);

            playerAnimation.SetMoving(moveUnit.y < 0, moveUnit.x > 0);
        }
    }
}

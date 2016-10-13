using System.Collections;
using Caveman.Setting;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModelHuman : PlayerModelClient
    {
        private bool isMoving;

        protected void Start()
        {
            if (multiplayer && !Settings.DisableSendMove) StartCoroutine(SendMove());
        }

        public void HandlerOnStopMove()
        {
            StopMove();
            isMoving = false;
        }

        private IEnumerator SendMove()
        {
            yield return new WaitForSeconds(0.3f);
            if (isMoving) serverNotify.MoveSend(transform.position);            
            StartCoroutine(SendMove());
        }

        public void MovePlayer(Vector3 direction, CNAbstractController arg2)
        {
            if (!isMoving) isMoving = true;
            moveUnit = direction*PlayerCore.Speed;
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

        // todo OnEnable must see last commits
        //public override void OnEnable()
        //{
        //    base.OnEnable();
        //    if (multiplayer && !Settings.DisableSendMove) StartCoroutine(SendMove());
        //}
    }
}

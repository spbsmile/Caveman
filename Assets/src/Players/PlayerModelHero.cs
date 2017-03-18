using System.Collections;
using Caveman.Level;
using Caveman.DevSetting;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModelHero : PlayerModelClient
    {
        private bool isMoving;
        private int mapWidth;
        private int mapHeight;  

        private BlockerMove blocker;

        protected void Start()
        {
            if (multiplayer && !DevSettings.DisableSendMove) StartCoroutine(SendMove());
        }

        public void HandlerOnStopMove()
        {
            StopMove();
            isMoving = false;
        }

        public void InitializationByMap(int mapWidth, int mapHeight)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
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

        public override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            var blockerMove = other.GetComponent<BlockerMove>();
            if(blockerMove)
            {
                blocker = other.GetComponent<BlockerMove>();

            }
        }

        protected override void Move()
        {
            var x = transform.position.x + moveUnit.x * Time.deltaTime;
            var y = transform.position.y + moveUnit.y * Time.deltaTime;

            // check movement out field
            var halfHeroX = spriteRenderer.bounds.size.x / 2;
            if (x < halfHeroX)
                x = halfHeroX;
            else if (x > mapHeight - halfHeroX)
                x = mapWidth - halfHeroX;

            var halfHeroY = spriteRenderer.bounds.size.y / 2;
            if (y < halfHeroY)
                y = halfHeroY;
            else if (y > mapHeight - halfHeroY)
                y = mapHeight - halfHeroY;

            if(blocker)
            {
                transform.position = blocker.CorrectionMove(x, y, halfHeroX);
                playerAnimation.SetMoving(moveUnit.y < 0, moveUnit.x > 0);
                return;
            }

            transform.position = new Vector3(x, y);
            playerAnimation.SetMoving(moveUnit.y < 0, moveUnit.x > 0);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (multiplayer && !DevSettings.DisableSendMove) StartCoroutine(SendMove());
        }
    }
}

using System.Collections;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModel : BasePlayerModel
    {
        const float RotateSpeed = 15f;
       // private CNAbstractController movementJoystick;
        private Transform _transformCache;

        protected override void Start()
        {
            base.Start();
            //todo realy &
            _transformCache = GetComponent<Transform>();
            
        }

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
          //  this.movementJoystick = movementJoystick;
            movementJoystick.ControllerMovedEvent += MovePlayer;
        }

        private void MovePlayer(Vector3 movement, CNAbstractController arg2)
        {
            StopCoroutine("RotateCoroutine");
            StartCoroutine("RotateCoroutine", movement);

            _transformCache.position += _transformCache.up * movement.magnitude * Settings.SpeedPlayer * Time.deltaTime;
            //var position = new Vector3(transform.position.x + movement.x * Time.deltaTime,
            //   transform.position.y + movement.y * Time.deltaTime);
            //transform.position = position;
        }

        IEnumerator RotateCoroutine(Vector3 direction)
        {
            do
            {
                _transformCache.up = Vector3.Lerp(_transformCache.up, direction, RotateSpeed * Time.deltaTime);
                yield return null;
            }
            while ((direction - _transformCache.up).sqrMagnitude > 0.2f);
        }
    }
}

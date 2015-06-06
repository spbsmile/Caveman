using System;
using Caveman.Players;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Weapons
{
    public class StoneModel : BaseWeaponModel
    {
        public Action<Vector2> Splash;

        private Vector2 target;
        private Vector2 delta;

        public void Update()
        {
            if (delta.magnitude > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.Distance(target, transform.position) > UnityExtensions.ThresholdPosition)
                {
                    transform.position = new Vector2(transform.position.x + delta.x*Time.deltaTime,
                        transform.position.y + delta.y*Time.deltaTime);
                    transform.Rotate(Vector3.forward, Settings.RotateStoneParameter);
                }
                else
                {
                    Destroy();
                }
            }
        }

        public void Move(Player player, Vector3 positionStart, Vector2 positionTarget)
        {
            owner = player;
            transform.position = positionStart;
            target = positionTarget;
            delta = UnityExtensions.CalculateDelta(positionStart, positionTarget, Settings.SpeedStone);
        }

        public override void Destroy()
        {
            Splash(transform.position);
            base.Destroy();
        }

        private const float BuizerInterval = 1;

        private Vector2 CurveBuizer(Vector2 p0, Vector2 p2)
        {
            var p1 = new Vector2((p0.x + p2.x) * 0.5f, (p0.y + p2.y) * 0.5f);


            return Vector2.up;
        }
    }
}

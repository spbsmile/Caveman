using Caveman.Animation;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Weapons
{
    public class StoneModel : WeaponModelBase
    {
        private static float STONE_SPEED = 5;

        private ObjectPool poolStonesSplash;

        public override WeaponType Type
        {
            get { return WeaponType.Stone; }
        }

        protected override float Speed
        {
            get { return Settings.SpeedStone; }
        }

        float bezierTime = 0;

        public void Update()
        {
            MotionUpdate();
        }


        public override void Destroy()
        {
            CreateStoneFragments(transform.position);
            base.Destroy();
        }

        private void CreateStoneFragments(Vector2 position)
        {
            for (var i = 0; i < 4; i++)
            {
                var flagment = poolStonesSplash.New();
                flagment.GetComponent<StoneSplash>().Init(i, position, poolStonesSplash);
            }
        }

        public void SetPoolSplash(ObjectPool objectPool)
        {
            poolStonesSplash = objectPool;
        }

        override protected void MotionUpdate()
        {
            if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.SqrMagnitude(target - (Vector2)transform.position) > UnityExtensions.ThresholdPosition)
                {
                    bezierTime += Time.deltaTime / Vector2.Distance(startPosition, target) * STONE_SPEED;
                    if (bezierTime > 1) bezierTime = 0;

                    Vector2 nextPosition = BezierUtils.Bezier2(startPosition, BezierUtils.ControlPoint(startPosition, target), target, bezierTime);

                    transform.position = nextPosition;

                    //linear moving. can be used for testing
                    //                    transform.position = new Vector2(transform.position.x + delta.x * Time.deltaTime,
                    //                        transform.position.y + delta.y * Time.deltaTime);
                    transform.Rotate(Vector3.forward, Settings.RotateStoneParameter);
                }
                else
                {
                    // todo при любом кидании уничтожается - плохо 
                    Destroy();
                }
            }
        }
    }
}

using Caveman.CustomAnimation;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Weapons
{
    public class StoneModel : WeaponModelBase
    {
        private ObjectPool<EffectBase> poolStonesSplash;
        private float bezierTime;

        public void Update()
        {
            MotionUpdate();
        }

        public override void Destroy()
        {
            bezierTime = 0;
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

        public void SetPoolSplash(ObjectPool<EffectBase> objectPool)
        {
            poolStonesSplash = objectPool;
        }

        override protected void MotionUpdate()
        {
            if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.SqrMagnitude(target - (Vector2)transform.position) > UnityExtensions.ThresholdPosition)
                {
                    bezierTime += Time.deltaTime / Vector2.Distance(startPosition, target) * speed;
                    if (bezierTime > 1) bezierTime = 0;
                    transform.position = BezierUtils.Bezier2(startPosition, BezierUtils.ControlPoint(startPosition, target), target, bezierTime);

                    //linear moving. can be used for testing
                    //                    transform.position = new Vector2(transform.position.x + delta.x * Time.deltaTime,
                    //                        transform.position.y + delta.y * Time.deltaTime);
                    transform.Rotate(Vector3.forward, Settings.StoneRotateParameter);
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

using Caveman.CustomAnimation;
using Caveman.Pools;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Weapons
{
    public class StoneModel : WeaponModelBase 
    {
        private ObjectPool<ImageBase> poolStonesSplash;
        private float bezierTime;

        public void Awake()
        {
            Config = EnterPoint.Configs.Weapon["stone"];
        }

        public void Update()
        {
            MoveUpdate();
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
                flagment.GetComponent<StoneSplash>()
                    .Initialization(i, position, stoneSplash => poolStonesSplash.Store(stoneSplash));
            }
        }

        public void InitializationPoolSplashesStone(ObjectPool<ImageBase> objectPool)
        {
            poolStonesSplash = objectPool;
        }

        protected override void MoveUpdate()
        {
            if (Vector2.SqrMagnitude(moveUnit) > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.SqrMagnitude(targetPosition - (Vector2)transform.position) > UnityExtensions.ThresholdPosition)
                {
                    bezierTime += Time.deltaTime / Vector2.Distance(startPosition, targetPosition) * Config.Speed;
                    if (bezierTime > 1) bezierTime = 0;
                    transform.position = BezierUtils.Bezier2(startPosition, BezierUtils.ControlPoint(startPosition, targetPosition), targetPosition, bezierTime);

                    //linear moving. can be used for testing
                    //                    transform.position = new Vector2(transform.position.x + moveUnit.x * Time.deltaTime,
                    //                        transform.position.y + moveUnit.y * Time.deltaTime);
                    transform.Rotate(Vector3.forward, Config.RotateParameter * Time.deltaTime * 100);
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

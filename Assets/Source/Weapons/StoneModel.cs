using Caveman.Animation;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Weapons
{
    public class StoneModel : WeaponModelBase
    {
        private ObjectPool poolStonesSplash;

        public override WeaponType Type
        {
            get { return WeaponType.Stone; }
        }

        protected override float Speed
        {
            get { return Settings.SpeedStone; }
        }

        public void Update()
        {
            if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.SqrMagnitude(target - (Vector2)transform.position) > UnityExtensions.ThresholdPosition)
                {
                    transform.position = new Vector2(transform.position.x + delta.x*Time.deltaTime,
                        transform.position.y + delta.y*Time.deltaTime);
                    transform.Rotate(Vector3.forward, Settings.RotateStoneParameter);
                }
                else
                {
                    // todo при любом кидании уничтожается - плохо 
                    Destroy();
                }
            }
        }

        public override void Destroy()
        {
            CreateStoneFlagment(transform.position);
            base.Destroy();
        }

        private void CreateStoneFlagment(Vector2 position)
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
    }
}

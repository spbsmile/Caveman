using Caveman.Players;
using Caveman.Utils;
using UnityEngine;
using Caveman.Setting;

namespace Caveman.Weapons
{
    public enum WeaponType
    {
        Stone,
        Skull
    }

    public  class WeaponModelBase : ASupportPool<WeaponModelBase>
    {
        public WeaponType type;
        public float speed;

        protected Vector2 startPosition;
        protected Vector2 target;
        protected Vector2 delta;

        public Player owner;

        private ObjectPool<WeaponModelBase> pool;

        public virtual void Destroy()
        {
            owner = null;
            delta = Vector2.zero;
            pool.Store(this);
        }

        public void Awake()
        {
            speed = Settings.SpeedStone;
        }

        public void Take()
        {
            pool.Store(this);    
        }

        public void UnTake(Vector2 position)
        {
            owner = null;
            transform.position = position;
        }

        public void SetMotion(Player player, Vector3 start, Vector2 aim)
        {
            owner = player;
            startPosition = start;
            transform.position = start;
            target = aim;
            delta = UnityExtensions.CalculateDelta(start, aim, speed);
        }

        public override void SetPool(ObjectPool<WeaponModelBase> weaponPool)
        {
            pool = weaponPool;
        }

        virtual protected void MotionUpdate()
        {
            if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.SqrMagnitude(target - (Vector2)transform.position) > UnityExtensions.ThresholdPosition)
                {
                    transform.position = new Vector2(transform.position.x + delta.x * Time.deltaTime,
                        transform.position.y + delta.y * Time.deltaTime);
                    transform.Rotate(Vector3.forward, Settings.RotateStoneParameter);
                }
                else
                {
                    Destroy();
                }
            }

        }
    }
}

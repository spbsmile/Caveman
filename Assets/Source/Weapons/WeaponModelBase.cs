using Caveman.Players;
using Caveman.Pools;
using Caveman.Utils;
using UnityEngine;
using Caveman.Configs;

namespace Caveman.Weapons
{
    public  class WeaponModelBase : ASupportPool<WeaponModelBase>
    {
        public WeaponConfig Config { protected set; get; }
        public Player Owner { private set; get; }
        
        protected Vector2 startPosition;
        protected Vector2 target;
        /// <summary>
        /// Linear parameter, define direction and value and on each update
        /// </summary>
        protected Vector2 delta;

        private ObjectPool<WeaponModelBase> pool;

        public virtual void Destroy()
        {
            Owner = null;
            delta = Vector2.zero;
            pool.Store(this);
        }

        public void Take()
        {
            pool.Store(this);    
        }

        /// <summary>
        /// player can simple put weapon on lang
        /// </summary>
        /// <param name="position"></param>
        public void UnTake(Vector2 position)
        {
            Owner = null;
            transform.position = position;
        }

        public void SetMotion(Player player, Vector3 start, Vector2 aim)
        {
            Owner = player;
            startPosition = start;
            transform.position = start;
            target = aim;
            // todo if weapon move no linear, delta needless, example: stone model, bezier curve
            delta = UnityExtensions.CalculateDelta(start, aim, Config.Speed);
        }

        public override void SetPool(ObjectPool<WeaponModelBase> weaponPool)
        {
            pool = weaponPool;
        }

        protected virtual void MotionUpdate()
        {
            if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.SqrMagnitude(target - (Vector2)transform.position) > UnityExtensions.ThresholdPosition)
                {
                    transform.position = new Vector2(transform.position.x + delta.x * Time.deltaTime,
                        transform.position.y + delta.y * Time.deltaTime);
                    transform.Rotate(Vector3.forward, Config.RotateParameter * Time.deltaTime * 100);
                }
                else
                {
                    Destroy();
                }
            }
        }
    }
}

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
        public PlayerCore Owner { private set; get; }
        
        protected Vector2 startPosition;
        protected Vector2 target;
        /// <summary>
        /// Linear parameter, define direction and value and on each update
        /// </summary>
        protected Vector2 moveUnit;

        private ObjectPool<WeaponModelBase> pool;

        public virtual void Destroy()
        {
            Owner = null;
            moveUnit = Vector2.zero;
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

        public void InitializationMove(PlayerCore playerCore, Vector3 start, Vector2 aim)
        {
            Owner = playerCore;
            startPosition = start;
            transform.position = start;
            target = aim;
            // todo if weapon move no linear, moveUnit needless, example: stone model, bezier curve
            moveUnit = UnityExtensions.CalculateDelta(start, aim, Config.Speed);
        }

        public override void InitializationPool(ObjectPool<WeaponModelBase> weaponPool)
        {
            pool = weaponPool;
        }

        protected virtual void MoveUpdate()
        {
            if (Vector2.SqrMagnitude(moveUnit) > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.SqrMagnitude(target - (Vector2)transform.position) > UnityExtensions.ThresholdPosition)
                {
                    transform.position = new Vector2(transform.position.x + moveUnit.x * Time.deltaTime,
                        transform.position.y + moveUnit.y * Time.deltaTime);
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

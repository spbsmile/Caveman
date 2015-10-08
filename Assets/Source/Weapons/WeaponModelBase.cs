using Caveman.Players;
using Caveman.Utils;
using UnityEngine;
using Caveman.Specification;

namespace Caveman.Weapons
{
    public  class WeaponModelBase : ASupportPool<WeaponModelBase>
    {
        public WeaponSpecification Specification { protected set; get; }
        public Player Owner { private set; get; }
        
        protected Vector2 startPosition;
        protected Vector2 target;
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
            delta = UnityExtensions.CalculateDelta(start, aim, Specification.Speed);
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
                    transform.Rotate(Vector3.forward, Specification.RotateParameter);
                }
                else
                {
                    Destroy();
                }
            }
        }
    }
}

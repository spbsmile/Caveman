using Caveman.Pools;
using Caveman.Utils;
using UnityEngine;
using Caveman.Configs;

namespace Caveman.Weapons
{ 
    public  class WeaponModelBase : ASupportPool<WeaponModelBase>, IWeapon
    {
        public string Id => Id;
        public WeaponConfig Config { protected set; get; }
        public string OwnerId { private set; get; }

        protected Vector2 startPosition;
        protected Vector2 targetPosition;
        /// <summary>
        /// Linear parameter, define direction and value and on each update
        /// </summary>
        protected Vector2 moveUnit;

        private ObjectPool<WeaponModelBase> Pool { set; get; }

        public virtual void Destroy()
        {
            OwnerId = "";
            moveUnit = Vector2.zero;
            Pool.Store(this);
        }

        public void Activate(string Id, Vector2 @from, Vector2 to)
        {
            Pool.New().InitializationMove(Id, @from, to);
        }

        public void Take()
        {
            Pool.Store(this);
        }

        private void InitializationMove(string ownerId, Vector3 start, Vector2 aim)
        {
            OwnerId = ownerId;
            startPosition = start;
            transform.position = start;
            targetPosition = aim;
            // todo if weapon move no linear, moveUnit needless, example: stone model, bezier curve
            moveUnit = UnityExtensions.CalculateDelta(start, aim, Config.Speed);
        }

        public override void InitializationPool(ObjectPool<WeaponModelBase> weaponPool)
        {
            Pool = weaponPool;
        }

        protected virtual void MoveUpdate()
        {
            if (Vector2.SqrMagnitude(moveUnit) > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.SqrMagnitude(targetPosition - (Vector2)transform.position) > UnityExtensions.ThresholdPosition)
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

using Caveman.Pools;
using Caveman.Utils;
using UnityEngine;
using Caveman.Configs;

namespace Caveman.Weapons
{ 
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public  class WeaponModelBase : ASupportPool<WeaponModelBase>
    {
        public string Id => Id;
        public WeaponConfig Config { protected set; get; }
        public Transform Transform => transform;
        public string OwnerId { protected internal set; get; }

        protected internal Vector2 startPosition;
        protected internal Vector2 targetPosition;
        /// <summary>
        /// Linear parameter, define direction and value and on each update
        /// </summary>
        protected internal Vector2 moveUnit;

        protected ObjectPool<WeaponModelBase> Pool { private set; get; }

        public virtual void Destroy()
        {
            OwnerId = "";
            moveUnit = Vector2.zero;
            Pool.Store(this);
        }

        public virtual void Take(string playerId)
        {
            Pool.Store(this);
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

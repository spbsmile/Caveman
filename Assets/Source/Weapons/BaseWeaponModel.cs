using Caveman.Players;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Weapons
{
    public  class BaseWeaponModel : MonoBehaviour
    {
        public virtual WeaponType Type{ get { return WeaponType.Stone; }}
        protected virtual float Speed{ get { return 1f; }}

        protected Vector2 target;
        protected Vector2 delta;

        public Player owner;

        private ObjectPool pool;

        public virtual void Destroy()
        {
            owner = null;
            pool.Store(transform);
        }

        public void SetMotion(Player player, Vector3 start, Vector2 target)
        {
            owner = player;
            transform.position = start;
            this.target = target;
            delta = UnityExtensions.CalculateDelta(start, target, Speed);
        }

        public void SetPool(ObjectPool pool)
        {
            this.pool = pool;
        }

        public bool PoolIsEmty
        {
            get
            {
                return pool == null;
            }
        }
    }

    public enum WeaponType
    {
        Stone,
        Skull
    }
}

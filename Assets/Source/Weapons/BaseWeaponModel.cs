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

        public void Take()
        {
            pool.Store(transform);    
        }

        public void UnTake(Vector2 position)
        {
            owner = null;
            transform.position = position;
        }

        public void SetMotion(Player player, Vector3 start, Vector2 aim)
        {
            owner = player;
            transform.position = start;
            target = aim;
            delta = UnityExtensions.CalculateDelta(start, aim, Speed);
        }

        public void SetPool(ObjectPool weaponPool)
        {
            pool = weaponPool;
        }
    }

    public enum WeaponType
    {
        Stone,
        Skull
    }
}

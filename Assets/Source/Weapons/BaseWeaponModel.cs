using Caveman.Players;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Weapons
{
    public class BaseWeaponModel : MonoBehaviour
    {
        public Player owner;

        private ObjectPool pool;

        public void SetPool(ObjectPool pool)
        {
            this.pool = pool;
        }

        public virtual void Destroy()
        {
            pool.Store(transform);
        }

        public bool PoolIsEmty
        {
            get
            {
                return pool == null;
            }
        }
    }
}

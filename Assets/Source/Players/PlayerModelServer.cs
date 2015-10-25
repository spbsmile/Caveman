using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Players
{
    /*
     * All the other players in multiplayer mode
     */
    public class PlayerModelServer : PlayerModelBase
    {
        private int weapons;

        protected void Start()
        {
            ChangedWeapons += () => weapons = 0;
            print("hello subscribe ChangedWeapons" + name);
        }

        public override void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (weapons > weaponModel.Specification.MaxOnPLayer) return;
            base.PickupWeapon(weaponModel);
            weapons++;
        }

        public override void Die()
        {
            weapons = 0;
            base.Die();
        }

        public override void Throw(Vector2 aim)
        {
            base.Throw(aim);
            weapons--;
        }

        public void Update()
        {
            Move();
        }
    }
}

using Caveman.Setting;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModelServer : PlayerModelBase
    {
        private int weapons;

        protected override void Start()
        {
            base.Start();
            ChangedWeapons += () => weapons = 0;
            print("hello subscribe ChangedWeapons" + name);
        }

        public override void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (weapons > Settings.MaxCountWeapons) return;
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

        //todo move to target .target from server
        public void Update()
        {
            Move();
        }
    }
}

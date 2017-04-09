
using System;
using UnityEngine;

namespace Caveman.Weapons.Melee
{
    public class SwordModel : WeaponModelBase, IWeapon
    {
        public void Activate(string ownerId, Vector2 from, Vector2 to)
        {
            throw new NotImplementedException();
        }

        public void Awake()
        {
            Config = EnterPoint.Configs.Weapon["sword"];
        }

        public override void Destroy()
        {
            base.Destroy();
        }

    }
}
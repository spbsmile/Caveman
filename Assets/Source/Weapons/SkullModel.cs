using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Weapons
{
    public class SkullModel : WeaponModelBase
    {
        public override WeaponType Type
        {
            get { return WeaponType.Skull; }
        }

        protected override float Speed
        {
            get { return Settings.SpeedSkull; }
        }

        // TODO разные кривые траекторий
        public void Update()
        {
            MotionUpdate();
        }
    }
}

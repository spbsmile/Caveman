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
            if (delta.magnitude > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.Distance(target, transform.position) > UnityExtensions.ThresholdPosition)
                {
                    transform.position = new Vector2(transform.position.x + delta.x * Time.deltaTime,
                        transform.position.y + delta.y * Time.deltaTime);
                    transform.Rotate(Vector3.forward, Settings.RotateStoneParameter);
                }
                else
                {
                    Destroy();
                }
            }
        }
    }
}

using System.Collections;
using UnityEngine;

namespace Caveman.Weapons.Melee
{
    public class SwordModel : WeaponModelBase, IWeapon
    {
        public void Activate(string ownerId, Vector2 from, Vector2 to)
        {
            StartCoroutine(ContinuousRotation());
        }

        public void Awake()
        {
            Config = EnterPoint.Configs.Weapon["sword"];
        }

        public override void Take()
        {
            print("hello my sword");
        }

        IEnumerator ContinuousRotation()
        {
            while (true)
            {
                transform.Rotate(Vector3.forward, 10);
                yield return new WaitForSeconds(0.01f);
            }
        }

    }
}
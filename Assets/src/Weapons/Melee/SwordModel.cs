using System.Collections;
using UnityEngine;

namespace Caveman.Weapons.Melee
{
    public class SwordModel : WeaponModelBase, IWeapon
    {
        public void Activate(string ownerId, Vector2 from, Vector2 to)
        {
            print("hello activate from editor");
            //throw new NotImplementedException();
        }

        public void Awake()
        {
            Config = EnterPoint.Configs.Weapon["sword"];
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        IEnumerator ContinuousRotation ()
         {
             while(true){
                 transform.Rotate(Vector3.forward,10);
                 yield return new WaitForSeconds (0.01f);
             }
         }

    }
}
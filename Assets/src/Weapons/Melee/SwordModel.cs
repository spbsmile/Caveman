using System.Collections;
using UnityEngine;

namespace Caveman.Weapons.Melee
{
    public class SwordModel : WeaponModelBase, IWeapon
    {
        private const float IntervalRotate = 20f;
        private const float StepRotate = 5f;
        private const int LayerWhenActive = 15;
        private const int LayerWhenInActive = 0;
        private SpriteRenderer render;

        public void Awake()
        {
            render = GetComponent<SpriteRenderer>(); 
            Config = EnterPoint.Configs.Weapon["sword"];
        }

        public void Activate(string ownerId, Vector2 from, Vector2 to)
        {
            render.sortingOrder = LayerWhenActive;
            StartCoroutine(ContinuousRotation());
        }

        public override void Take(string playerId)
        {
            OwnerId = playerId;
            print("hello my sword");
        }

        private IEnumerator ContinuousRotation()
        {
            yield return new WaitForSeconds(0.05f);
            var angle = 0f;
            while (angle < IntervalRotate)
            {
                angle += StepRotate;
                transform.Rotate(Vector3.forward, angle);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1.5f);
            Sheathe();
        }

        private void Sheathe()
        {
            render.sortingOrder = LayerWhenInActive;
            transform.rotation = Quaternion.identity;
        }

    }
}
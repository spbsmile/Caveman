using System;
using Caveman.Players;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman
{
    public class WeaponModel : MonoBehaviour
    {
        public Player owner;
        public Action<Vector2> Splash;

        private Vector2 target;
        private Vector2 delta;
        //todo возможно, разные скрипты под разные пулы
        private ObjectPool<WeaponModel> weaponPool;
        private ObjectPool<WeaponModel> weaponLandPool; 

        public void Update()
        {
            if (delta.magnitude > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.Distance(target, transform.position) > UnityExtensions.ThresholdPosition)
                {
                    transform.position = new Vector2(transform.position.x + delta.x*Time.deltaTime,
                        transform.position.y + delta.y*Time.deltaTime);
                    transform.Rotate(Vector3.forward, Settings.RotateStoneParameter);
                }
                else
                {
                    // todo use Object pool pattern
                    Destroy();
                }
            }
        }

        public void SetWeaponPool(ObjectPool<WeaponModel> objectPool)
        {
            weaponPool = objectPool;
        }

        public void Move(Player player, Vector3 positionStart, Vector2 positionTarget)
        {
            owner = player;
            transform.position = positionStart;
            target = positionTarget;
            delta = UnityExtensions.CalculateDelta(positionStart, positionTarget, Settings.SpeedWeapon);
        }

        private void Destroy()
        {
            Splash(transform.position);
            //todo reference on poolweapons
            gameObject.SetActive(false);
            weaponPool.Store(this);
        }
    }
}

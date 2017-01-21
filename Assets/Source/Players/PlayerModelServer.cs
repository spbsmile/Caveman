using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Players
{
    /*
     * All the other players in multiplayer mode
     */
    public class PlayerModelServer : PlayerModelBase
    {
        private Vector3 targetPosition;

        public override void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (PlayerCore.WeaponCount > weaponModel.Config.Weight) return;
            base.PickupWeapon(weaponModel);
            PlayerCore.WeaponCount++;
        }

        public override void Die()
        {
            PlayerCore.WeaponCount = 0;
            base.Die();
        }

        public override void ActivateWeapon(Vector2 aim)
        {
            base.ActivateWeapon(aim);
            PlayerCore.WeaponCount--;
        }

        public override void CalculateMoveUnit(Vector2 targetPosition)
        {
            base.CalculateMoveUnit(targetPosition);
            this.targetPosition = targetPosition;
        }

        public void Update()
        {
            // todo add check time interval also
            if (Vector2.SqrMagnitude(transform.position - targetPosition) <
                UnityExtensions.ThresholdPosition)
            {
                StopMove();
            }
            else
            {
                Move();
            }
        }
    }
}

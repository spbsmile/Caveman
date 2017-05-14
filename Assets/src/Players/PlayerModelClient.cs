using System.Collections;
using Caveman.Bonuses;
using Caveman.Level;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModelClient : PlayerModelBase
    {
        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            var weapon = other.GetComponent<WeaponModelBase>();
            if (weapon)
            {
                if (string.IsNullOrEmpty(weapon.OwnerId))
                    PickupWeapon((IWeapon)weapon);
                else if (weapon.OwnerId != Core.Id)
                {
                    GetPlayerById(weapon.OwnerId).KillCount++;
                    Core.DeathCount++;
                    if (multiplayer)
                    {
                        serverNotify.PlayerDeadSend();
                        serverNotify.AddedKillStatSend(weapon.OwnerId);
                    }
                    StopAllCoroutines();
                    StartCoroutine(playerAnimation.Death(transform.position));
                    Die();
                    StartCoroutine(Respawn(GetRandomPosition()));
                }
            }
        }

        protected override IEnumerator Respawn(Vector2 position)
        {
            yield return StartCoroutine(base.Respawn(position));
        }

        public override void RespawnInstantly(Vector2 position)
        {
            base.RespawnInstantly(position);
            if (multiplayer) serverNotify.RespawnSend(position);
        }

        public override void PickupBonus(BonusBase bonus)
        {
            if (multiplayer) serverNotify.PickBonusSend(bonus.Id, (int)bonus.Config.Type);
            base.PickupBonus(bonus);
        }

        public override void PickupWeapon(IWeapon weapon)
        {
            if (!IsEnoughStrength(weapon.Config.Weight)) return;
            base.PickupWeapon(weapon);
            Core.WeaponCount += weapon.Config.CountItems;
            if (multiplayer) serverNotify.PickWeaponSend(weapon.Id, (int)weapon.Config.Type);
        }

        public override void ActivateWeapon(Vector2 aim)
        {
            if (multiplayer) serverNotify.ActivateWeaponSend(aim, (int)currentWeapon.Config.Type);
            base.ActivateWeapon(aim);
        }

        protected bool IsEnoughStrength(int weight)
        {
            return Core.Config.Strength > Core.WeaponCount * weight;
        }
    }
}

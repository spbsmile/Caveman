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
                else if (weapon.OwnerId != PlayerCore.Id)
                {
                    GetPlayerById(weapon.OwnerId).KillCount++;
                    PlayerCore.DeathCount++;
                    if (multiplayer)
                    {
                        serverNotify.PlayerDeadSend();
                        serverNotify.AddedKillStatSend(weapon.OwnerId);
                    }
                    weapon.Destroy();
                    StopAllCoroutines();
                    StartCoroutine(playerAnimation.Death(transform.position));
                    Die();
                    StartCoroutine(Respawn(GetRandomPosition()));
                }
            }

            if (other.GetComponent<PlayerModelBase>())
            {

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
            if (multiplayer) serverNotify.PickBonusSend(bonus.Id, (int) bonus.Config.Type);
            base.PickupBonus(bonus);
        }

        public override void PickupWeapon(IWeapon weapon)
        {
            if (!IsEnoughStrength(weapon.Config.Weight)) return;
            base.PickupWeapon(weapon);
            PlayerCore.WeaponCount += weapon.Config.CountItems;
            if (multiplayer) serverNotify.PickWeaponSend(weapon.Id, (int) weapon.Config.Type);
        }

        public override void ActivateWeapon(Vector2 aim)
        {
            if (multiplayer) serverNotify.ActivateWeaponSend(aim, (int) currentWeapon.Config.Type);
            base.ActivateWeapon(aim);
            PlayerCore.WeaponCount--;
        }

        protected bool IsEnoughStrength(int weight)
        {
            return PlayerCore.Config.Strength > PlayerCore.WeaponCount*weight;
        }

        private IEnumerator PerformWeaponAction()
        {
            yield return new WaitForSeconds(currentWeapon?.Config.Cooldown ?? 2f);
            if (PlayerCore.WeaponCount > 0)
            {
                var victim = FindClosestPlayer(this);
                if (victim != null)
                {
                    ActivateWeapon(victim.transform.position);
                }
            }
            StartCoroutine(PerformWeaponAction());
        }

        public virtual void OnEnable()
        {
            if (levelMode != LevelMode.designer)
            {
                StartCoroutine(PerformWeaponAction());
            }
        }
    }
}

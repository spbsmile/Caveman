using System.Collections;
using Caveman.Bonuses;
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
                if (weapon.Owner == null)
                {
                    switch (weapon.Config.Type)
                    {
                        case WeaponType.Stone:
                            PickupWeapon(other.gameObject.GetComponent<StoneModel>());
                            break;
                        case WeaponType.Skull:
                            PickupWeapon(other.gameObject.GetComponent<AxeModel>());
                            break;
                    }
                }
                else
                {
                    if (weapon.Owner != PlayerCore)
                    {
                        weapon.Owner.KillCount++;
                        PlayerCore.DeathCount++;
                        if (multiplayer)
                        {
                            serverNotify.PlayerDeadSend();
                            serverNotify.AddedKillStatSend(weapon.Owner.Id);
                        }
                        weapon.Destroy();
                        StopAllCoroutines();
                        StartCoroutine(playerAnimation.Death(transform.position));
                        Die();
                        StartCoroutine(Respawn(GetRandomPosition()));
                    }
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
            if (multiplayer) serverNotify.PickBonusSend(bonus.Id, (int) bonus.Config.Type);
            //todo hack
            base.PickupBonus(bonus);
        }

        public override void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (!IsEnoughStrength(weaponModel.Config.Weight)) return;
            base.PickupWeapon(weaponModel);
            PlayerCore.WeaponCount += weaponModel.Config.CountItems;
            if (multiplayer) serverNotify.PickWeaponSend(weaponModel.Id, (int) weaponModel.Config.Type);
        }

        public override void ActivateWeapon(Vector2 aim)
        {
            if (multiplayer) serverNotify.ActivateWeaponSend(aim, (int) WeaponConfig.Type);
            base.ActivateWeapon(aim);
            PlayerCore.WeaponCount--;
        }

        protected bool IsEnoughStrength(int weight)
        {
            return PlayerCore.Config.Strength > PlayerCore.WeaponCount*weight;
        }

        private IEnumerator PerformWeaponAction()
        {
            yield return new WaitForSeconds(WeaponConfig.Cooldown);
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
            //StartCoroutine(PerformWeaponAction());
        }
    }
}

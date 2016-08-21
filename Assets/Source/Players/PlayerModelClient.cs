using System.Collections;
using Caveman.Bonuses;
using Caveman.Setting;
using Caveman.Configs;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModelClient : PlayerModelBase
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return;
            var weapon = other.GetComponent<WeaponModelBase>();
            if (weapon)
            {
                if (weapon.Owner == null)
                {
                    switch (weapon.Config.Type)
                    {
                        case WeaponConfig.Types.Stone:
                            PickupWeapon(other.gameObject.GetComponent<StoneModel>());
                            break;
                        case WeaponConfig.Types.Skull:
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
                            //todo must see commit renames
                            //serverNotify.PlayerDeadTest(weapon.Owner.Id);
                        }
                        weapon.Destroy();
                        StopAllCoroutines();
                        Die();
                        StartCoroutine(Respawn(new Vector2(r.Next(Settings.WidthMap), r.Next(Settings.HeightMap))));
                    }
                }
            }
            else
            {
                var bonus = other.gameObject.GetComponent<BonusBase>();
                if (bonus)
                {
                    PickupBonus(bonus);
                }
            }
        }

        public override IEnumerator Respawn(Vector2 position)
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
	        //todo hack
	        base.PickupBonus(bonus);
        }

        public override void PickupWeapon(WeaponModelBase weaponModel)
        {
            //todo strong hero and weight weapon formula strong = countWeapon&weigthWeapon
            if (PlayerCore.WeaponCount > weaponModel.Config.Weight) return;
            base.PickupWeapon(weaponModel);
            PlayerCore.WeaponCount += weaponModel.Config.CountItems;
            if (multiplayer) serverNotify.PickWeaponSend(weaponModel.Id, (int)weaponModel.Config.Type);
        }

        public override void ThrowWeapon(Vector2 aim)
        {
            if (multiplayer) serverNotify.UseWeaponSend(aim, (int) WeaponConfig.Type);
            base.ThrowWeapon(aim);
            PlayerCore.WeaponCount--;
        }

        private IEnumerator ThrowWeaponOnCooldown()
        {
            yield return new WaitForSeconds(WeaponConfig.Cooldown);
            if (PlayerCore.WeaponCount > 0)
            {
                var victim = playersManager.FindClosestPlayer(this);
                if (victim != null)
                {
                    ThrowWeapon(victim.transform.position);
                }
            }
            StartCoroutine(ThrowWeaponOnCooldown());
        }
    }
}

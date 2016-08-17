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
                        weapon.Owner.Kills++;
                        PlayerCore.Deaths++;
                        if (multiplayer)
                        {
                            serverNotify.PlayerDead();
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
        /*
        public override void Play()
        {
            base.Play();
            StartCoroutine(ThrowWeaponOnCooldown());
        }
        */


        //public override bool SpendGold(int value)
        //{
        //    var res = base.SpendGold(value);
        //    if (res && multiplayer)
        //    {
        //        serverNotify.PlayerGold(Gold);    
        //    }
        //    return res;
        //}

        public override IEnumerator Respawn(Vector2 point)
        {
            yield return StartCoroutine(base.Respawn(point));
        }

        public override void Birth(Vector2 point)
        {
            base.Birth(point);
            if (multiplayer) serverNotify.Respawn(point);
        }

        public override void PickupBonus(BonusBase bonus)
        {
            //todo hack
            if (multiplayer) serverNotify.PickBonus(bonus.Id, (int)bonus.Config.Type);
            base.PickupBonus(bonus);
        }

        public override void PickupWeapon(WeaponModelBase weaponModel)
        {
            //todo strong hero and weight weapon formula strong = countWeapon&weigthWeapon
            if (PlayerCore.Weapons > weaponModel.Config.Weight) return;
            base.PickupWeapon(weaponModel);
            PlayerCore.Weapons += weaponModel.Config.CountItems;
            if (multiplayer) serverNotify.PickWeapon(weaponModel.Id, (int)weaponModel.Config.Type);
        }

        public override void ThrowWeapon(Vector2 aim)
        {
            if (multiplayer) serverNotify.UseWeapon(aim, (int) WeaponConfig.Type);
            base.ThrowWeapon(aim);
            PlayerCore.Weapons--;
        }

        private IEnumerator ThrowWeaponOnCooldown()
        {
            yield return new WaitForSeconds(WeaponConfig.Cooldown);
            if (PlayerCore.Weapons > 0)
            {
                var victim = FindClosestPlayer();
                if (victim != null)
                {
                    ThrowWeapon(victim.transform.position);
                }
            }
            StartCoroutine(ThrowWeaponOnCooldown());
        }

        private PlayerModelBase FindClosestPlayer()
        {
            var minDistance = (float) Settings.HeightMap*Settings.WidthMap;
            PlayerModelBase result = null;
            for (var i = 0; i < players.Count; i++)
            {
                if (!players[i].gameObject.activeSelf || players[i] == this ||
                    !players[i].spriteRenderer.isVisible || players[i].invulnerability) continue;
                var childDistance = Vector2.SqrMagnitude(players[i].transform.position - transform.position);
                if (minDistance > childDistance)
                {
                    result = players[i];
                    minDistance = childDistance;
                }
            }
            return result;
        }
    }
}

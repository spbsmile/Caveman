using System.Collections;
using Caveman.Bonuses;
using Caveman.Setting;
using Caveman.Specification;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModelClient : PlayerModelBase
    {
        protected virtual void Start()
        {
            ChangedWeapons += () => Player.Weapons = 0;
            print("hello subscribe ChangedWeapons" + name);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return;
            var weapon = other.GetComponent<WeaponModelBase>();
            if (weapon)
            {
                if (weapon.Owner == null)
                {
                    switch (weapon.Specification.Type)
                    {
                        case WeaponSpecification.Types.Stone:
                            PickupWeapon(other.gameObject.GetComponent<StoneModel>());
                            break;
                        case WeaponSpecification.Types.Skull:
                            PickupWeapon(other.gameObject.GetComponent<AxeModel>());
                            break;
                    }
                }
                else
                {
                    if (weapon.Owner != Player)
                    {
                        weapon.Owner.Kills++;
                        Player.Deaths++;
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

        public override void Play()
        {
            base.Play();
            StartCoroutine(ThrowOnTimer());
        }

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
            if (multiplayer) serverNotify.PickBonus(bonus.Id, (int)bonus.Specification.Type);
            base.PickupBonus(bonus);
        }

        public override void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (Player.Weapons > weaponModel.Specification.MaxOnPLayer) return;
            base.PickupWeapon(weaponModel);
            Player.Weapons += weaponModel.Specification.CountPickup;
            if (multiplayer) serverNotify.PickWeapon(weaponModel.Id, (int)weaponModel.Specification.Type);
        }

        public override void Throw(Vector2 aim)
        {
            if (multiplayer) serverNotify.UseWeapon(aim, (int) weaponSpecification.Type);
            base.Throw(aim);
            Player.Weapons--;
        }

        private IEnumerator ThrowOnTimer()
        {
            yield return new WaitForSeconds(weaponSpecification.ThrowInterval);
            if (Player.Weapons > 0)
            {
                var victim = FindClosestPlayer();
                if (victim != null)
                {
                    Throw(victim.transform.position);
                }
            }
            StartCoroutine(ThrowOnTimer());
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

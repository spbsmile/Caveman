using System.Collections;
using Caveman.Bonuses;
using Caveman.Setting;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Players
{
    public class PlayerModelClient : PlayerModelBase
    {
        protected override void Start()
        {
            base.Start();
            ChangedWeapons += () => player.Weapons = 0;
            print("hello subscribe ChangedWeapons" + name);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return;
            var weapon = other.gameObject.GetComponent<WeaponModelBase>();
            if (weapon)
            {
                if (weapon.owner == null)
                {
                    switch (weapon.type)
                    {
                        case WeaponType.Stone:
                            PickupWeapon(other.gameObject.GetComponent<StoneModel>());
                            break;
                        case WeaponType.Skull:
                            PickupWeapon(other.gameObject.GetComponent<SkullModel>());
                            break;
                    }
                }
                else
                {
                    if (weapon.owner != player)
                    {
                        weapon.owner.Kills++;
                        player.deaths++;
                        weapon.Destroy();
                        StopAllCoroutines();
                        Die();
                        if (multiplayer) serverConnection.SendPlayerDead();
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

        public override IEnumerator Respawn(Vector2 point)
        {
            if (multiplayer) serverConnection.SendRespawn(Id, point);
            yield return StartCoroutine(base.Respawn(point));
        }

        public override void PickupBonus(BonusBase bonus)
        {
            if (multiplayer) serverConnection.SendPickBonus(bonus.Id, (int) bonus.Type);
            base.PickupBonus(bonus);
        }

        public override void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (player.Weapons > Settings.MaxCountWeapons) return;
            base.PickupWeapon(weaponModel);
            player.Weapons += 1;
            if (multiplayer) serverConnection.SendPickWeapon(weaponModel.Id, (int)weaponModel.type);
        }

        public override void Throw(Vector2 aim)
        {
            if (multiplayer) serverConnection.SendUseWeapon(aim, (int) weaponType);
            base.Throw(aim);
            player.Weapons--;
        }

        private IEnumerator ThrowOnTimer()
        {
            yield return new WaitForSeconds(Settings.TimeThrowWeapon);
            if (player.Weapons > 0)
            {
                var victim = FindClosestPlayer();
                if (victim != null)
                {
                    animator.SetTrigger(Settings.AnimThrowF);
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
                    !players[i].spriteRenderer.isVisible) continue;
                var childDistance = Vector2.SqrMagnitude(players[i].transform.position - transform.position);
                if (minDistance > childDistance)
                {
                    result = players[i];
                    minDistance = childDistance;
                }
            }
            return result;
        }
     
        public void OnEnable()
        {
            StartCoroutine(ThrowOnTimer());
        }
    }
}

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
            StartCoroutine(ThrowOnTimer());
        }

        public override void PickupBonus(BonusBase bonus)
        {
            if (multiplayer) serverConnection.SendPickBonus(transform.position, (int) bonus.Type);
            base.PickupBonus(bonus);
        }

        public override bool PickupWeapon(WeaponModelBase weaponModel)
        {
            if (!base.PickupWeapon(weaponModel)) return false;
            if (multiplayer) serverConnection.SendPickWeapon(transform.position, (int) weaponModel.type);
            return true;
        }

        public override void Throw(Vector2 aim)
        {
            if (multiplayer) serverConnection.SendUseWeapon(aim, (int) weaponType);
            base.Throw(aim);
        }

        private IEnumerator ThrowOnTimer()
        {
            if (player.Weapons > 0)
            {
                var victim = FindClosestPlayer();
                if (victim != null)
                {
                    animator.SetTrigger(Settings.AnimThrowF);
                    Throw(victim.transform.position);
                }
            }
            yield return new WaitForSeconds(Settings.TimeThrowStone);
            if (gameObject.activeSelf) StartCoroutine(ThrowOnTimer());
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return; // todo подумать. 
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
                        Death(transform.position);
                        //todo id if multiplayer
                        if (multiplayer) serverConnection.SendPlayerDead();
                        Respawn();
                        poolPlayers.Store(this);
                    }
                    else
                    {
                        // for check temp
                        print(" weapon.owner == player");
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
    }
}

using System.Collections;
using Caveman.Bonuses;
using Caveman.Players;
using Caveman.Setting;
using Caveman.Weapons;
using UnityEngine;

public class PlayerSingleModel : PlayerModelBase
{
	protected override void Start () 
    {
        base.Start();
        StartCoroutine(ThrowOnTimer());
	}

    public IEnumerator ThrowOnTimer()
    {
        if (player.Weapons > 0)
        {
            //todo ждать конца интервала анимации по карутине
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
                    Respawn(player);
                    playersPool.Store(this);
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
        var minDistance = (float)Settings.HeightMap * Settings.WidthMap;
        PlayerModelBase result = null;
        for (var i = 0; i < players.Length; i++)
        {
            if (!players[i].gameObject.activeSelf || players[i] == this ||
                !players[i].renderer.isVisible) continue;
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

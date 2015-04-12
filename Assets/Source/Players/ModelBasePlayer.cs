using System;
using System.Collections;
using UnityEngine;

namespace Caveman.Players
{
    public class ModelBasePlayer : MonoBehaviour
    {
        public Transform players;
        public Player player;
        public Action<Player> Respawn;
        public void Init(Player player, Vector2 position)
        {
            name = player.name;
            this.player = player;
            transform.position = position;
        }

        protected void ThrowStone()
        {
            var stone = Instantiate(Resources.Load("weapon", typeof(GameObject))) as GameObject;
            var weaponModel = stone.GetComponent<WeaponModel>();
            //animator.SetBool("Throw", true);
            weaponModel.Move(player, transform.position, new Vector2(3, 3));
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return;
            var weapon = other.gameObject.GetComponent<WeaponModel>();
            if (weapon != null)
            {
                if (weapon.owner == null)
                {
                    player.weapons++;
                    Destroy(other.gameObject);
                }
                else
                {
                    if (weapon.owner != player)
                    {
                        print("killed");
                        weapon.owner.killed++;
                        Destroy(other.gameObject);
                        Respawn(player);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}



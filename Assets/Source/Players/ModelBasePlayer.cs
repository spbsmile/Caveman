using System;
using UnityEngine;

namespace Caveman.Players
{
    public class ModelBasePlayer : MonoBehaviour
    {
        public Action<Player> Respawn;

        private Player player;
        protected Animator animator;

        public void Start()
        {
            animator = GetComponent<Animator>();
        }
        
        public void Init(Player player, Vector2 positionStart)
        {
            name = player.name;
            this.player = player;
            transform.position = positionStart;
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

        protected void ThrowStone()
        {
            var stone = Instantiate(Resources.Load("weapon", typeof(GameObject))) as GameObject;
            var weaponModel = stone.GetComponent<WeaponModel>();
            //animator.SetBool("Throw", true);
            weaponModel.Move(player, transform.position, FindClosestEnemy());
        }

        private Vector2 FindClosestEnemy()
        {
            float minDistance = 0;
            var nearPosition = Vector2.zero;
            foreach (Transform child in transform.parent)
            {
                //todo хак, надо поправить
                if (child.name != name)
                {
                    if (minDistance < 0.1f)
                    {
                        minDistance = Vector2.Distance(child.position, transform.position);
                        nearPosition = child.position;
                    }
                    else
                    {
                        var childDistance = Vector2.Distance(child.position, transform.position);
                        if (minDistance > childDistance)
                        {
                            minDistance = childDistance;
                            nearPosition = child.position;
                        }
                    }
                }
            }
            return nearPosition;
        }
    }
}



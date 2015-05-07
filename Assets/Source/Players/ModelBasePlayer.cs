using System;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Players
{
    public class ModelBasePlayer : MonoBehaviour
    {
        protected const float Speed = 2.5f;
        
        public Action<Player> Respawn;

        protected Player player;
        protected Vector2 delta;
        protected Animator animator;
        protected Vector2 target;
        protected Random random;

        private int timeThrowStone = 300;

        public virtual void Start()
        {
            animator = GetComponent<Animator>();
        }
        
        public void Init(Player player, Vector2 positionStart, Random random)
        {
            name = player.name;
            this.player = player;
            this.random = random;
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
                        weapon.owner.kills++;
                        player.deaths++;
                        Destroy(other.gameObject);
                        Respawn(player);
                        Destroy(gameObject);
                    }
                }
            }
        }

        private void ThrowStone()
        {
            if (player.weapons > 0)
            {
                var stone = Instantiate(Resources.Load("weapon", typeof(GameObject))) as GameObject;
                var weaponModel = stone.GetComponent<WeaponModel>();
                animator.SetBool("Throw", true);
                weaponModel.Move(player, transform.position, FindClosest(transform.parent));
                player.weapons--;
            }
        }

        protected bool MoveStop()
        {
            if (delta.magnitude > UnityExtensions.ThresholdPosition &&
                Vector2.SqrMagnitude((Vector2) transform.position - target) < UnityExtensions.ThresholdPosition)
            {
                delta = Vector2.zero;
                return true;
            }
            return false;
        }

        protected void Move()
        {
            if (delta.magnitude > UnityExtensions.ThresholdPosition)
            {
                transform.position = new Vector3(transform.position.x + delta.x * Time.deltaTime,
                transform.position.y + delta.y * Time.deltaTime);
            }
        }

        protected void ThrowStoneOnTimer()
        {
            if (timeThrowStone-- >= 0) return;
            ThrowStone();
            timeThrowStone = 300;
        }


        protected Vector2 FindClosest(Transform container)
        {
            float minDistance = 0;
            var nearPosition = Vector2.zero;
            foreach (Transform child in container)
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



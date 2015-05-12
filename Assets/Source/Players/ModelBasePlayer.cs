using System;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Players
{
    public class ModelBasePlayer : MonoBehaviour
    {
        public Action<Player> Respawn;
        public Action<Vector2> Death;
        public Action<Player, Vector2, Vector2> ThrowStone;

        protected Player player;
        protected Vector2 delta;
        protected Animator animator;
        protected Vector2 target;
        protected Random random;

        private float timeCurrentThrow;

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
                    //animator.SetTrigger(Settings.AnimPickup);
                    Destroy(other.gameObject);
                }
                else
                {
                    if (weapon.owner != player)
                    {
                        weapon.owner.kills++;
                        player.deaths++;
                        Destroy(other.gameObject);
                        Death(transform.position);
                        Respawn(player);
                        Destroy(gameObject);
                    }
                }
            }
        }

        public void Throw()
        {
            ThrowStone(player, transform.position, FindClosest(transform.parent));
            player.weapons--;
        }

        protected void ThrowStoneOnTimer()
        {
            timeCurrentThrow = player.countRespawnThrow * Settings.TimeThrowStone - Time.time;
            if (timeCurrentThrow-- >= 0) return;
            player.countRespawnThrow++;

            if (player.weapons > 0)
            {
                //animator.SetBool("throw", true);
                animator.SetBool(Settings.AnimThrowB, true);
            }
            timeCurrentThrow = Settings.TimeThrowStone;
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
                 var position = new Vector3(transform.position.x + delta.x * Time.deltaTime,
                transform.position.y + delta.y * Time.deltaTime);
                 transform.position = new Vector3(Mathf.Clamp(position.x, -Settings.BoundaryEndMap, Settings.BoundaryEndMap),
                     Mathf.Clamp(position.y, -Settings.BoundaryEndMap, Settings.BoundaryEndMap));
            }
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



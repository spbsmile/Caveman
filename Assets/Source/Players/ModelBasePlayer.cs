using System;
using Assets.Source;
using Assets.Source.Settings;
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
        private ObjectPool<WeaponModel> weaponLandPool;

        public virtual void Start()
        {
            animator = GetComponent<Animator>();
        }
        
        public void Init(Player player, Vector2 positionStart, Random random, ObjectPool<WeaponModel> weaponLandPool)
        {
            name = player.name;
            this.player = player;
            this.random = random;
            transform.position = positionStart;
            this.weaponLandPool = weaponLandPool;
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
                    animator.SetTrigger(Settings.AnimPickup);
                    //todo инкапсулировать, оружие базовый класс метод дестрой
                    weapon.gameObject.SetActive(false);
                    weaponLandPool.Store(weapon);
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
                        // todo use Object pool pattern
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
            timeCurrentThrow = player.countRespawnThrow * Settings.TimeThrowStone - Time.timeSinceLevelLoad;
            if (timeCurrentThrow-- >= 0) return;
            player.countRespawnThrow++;

            if (player.weapons > 0)
            {
                animator.SetTrigger(Settings.AnimThrowF);
            }
            timeCurrentThrow = Settings.TimeThrowStone;
        }

        

        protected bool MoveStop()
        {
            if (delta.magnitude > UnityExtensions.ThresholdPosition &&
                Vector2.SqrMagnitude((Vector2) transform.position - target) < UnityExtensions.ThresholdPosition)
            {
                animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, 0);
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
                transform.position = position;
            }
        }

        protected Vector2 FindClosest(Transform container)
        {
            float minDistance = 0;
            var nearPosition = Vector2.zero;
            // todo use array instead ienumerable
            foreach (Transform child in container)
            {
				var childModelPlayer = child.gameObject.GetComponent<ModelBasePlayer>();
                if (childModelPlayer != this)
                {
                    if (minDistance < 0.1f)
                    {
                        minDistance = Vector2.Distance(child.position, transform.position);
                        nearPosition = child.position;
				    	//break;
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



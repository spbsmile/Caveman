using System;
using Caveman.Setting;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Players
{
    public class BasePlayerModel : MonoBehaviour
    {
        public Action<Player> Respawn;
        public Action<Vector2> Death;
        public Action<Player, Vector2, Vector2> ThrowStone;
        //todo внимательно посмотреть 
        public Player player;
        
        protected Vector2 delta;
        protected Animator animator;
        protected Vector2 target;
        protected Random r;

        private PlayerPool playerPool;
        private BasePlayerModel[] players;
        private ObjectPool weaponsHandPool;
        private bool inMotion;

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            Invoke("ThrowStoneOnTimer", Settings.TimeThrowStone);
        }
        
        public void Init(Player player, Vector2 positionStart, Random random, PlayerPool playerPool)
        {
            name = player.name;
            this.player = player;
            this.playerPool = playerPool;
            players = playerPool.GetArray();
            r = random;
            transform.position = positionStart;
        }

        // todo проверить рандом. использование в ai убрать
        public void RandomPosition()
        {
            transform.position = new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br));
        }
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return;
            var weapon = other.gameObject.GetComponent<BaseWeaponModel>();
            if (weapon != null)
            {
                if (weapon.owner == null)
                {
                    if (player.Weapons < Settings.MaxCountWeapons)
                    {
                        player.Weapons++;
                        if (animator)
                        {
                            animator.SetTrigger(Settings.AnimPickup);
                        }
                        else
                        {
                            Debug.LogWarning("Pickup animator null reference");
                        }
                        weapon.Destroy();
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
                        Respawn(player);
                        playerPool.Store(this);  
                    }
                }
            }
        }

        public void Throw()
        {
            ThrowStone(player, transform.position, FindClosestPlayer());
            player.Weapons--;
        }

        protected void ThrowStoneOnTimer()
        {
            if (player.Weapons > 0)
            {
                animator.SetTrigger(Settings.AnimThrowF);
            }
            Invoke("ThrowStoneOnTimer", Settings.TimeThrowStone);
        }   

        public bool InMotion
        {
            protected get
            {
                if (delta.magnitude > UnityExtensions.ThresholdPosition &&
                    Vector2.SqrMagnitude((Vector2) transform.position - target) < UnityExtensions.ThresholdPosition)
                {
                    animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, 0);
                    delta = Vector2.zero;
                    inMotion = false;
                    return inMotion;
                }
                return inMotion;
            }
            set { inMotion = value; }
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

        private Vector2 FindClosestPlayer()
        {
            float minDistance = 0;
            var nearPosition = Vector2.zero;

            for (var i = 0; i < players.Length; i++)
            {
                if (!players[i].gameObject.activeSelf) continue;
                if (players[i] != this)
                {
                    if (minDistance < 0.1f)
                    {
                        minDistance = Vector2.Distance(players[i].transform.position, transform.position);
                        nearPosition = players[i].transform.position;
                    }
                    else
                    {
                        var childDistance = Vector2.Distance(players[i].transform.position, transform.position);
                        if (minDistance > childDistance)
                        {
                            minDistance = childDistance;
                            nearPosition = players[i].transform.position;
                        }
                    }
                }
            }
            return nearPosition;
        }

        protected Vector2 FindClosestLyingWeapon(params Transform[] array)
        {
            float minDistance = 0;
            var nearPosition = Vector2.zero;
            for (var i = 0; i < array.Length; i++)
            {
                if (!array[i].gameObject.activeSelf) continue;
                if (minDistance < 0.1f)
                {
                    minDistance = Vector2.Distance(array[i].position, transform.position);
                    nearPosition = array[i].position;
                }
                else
                {
                    var childDistance = Vector2.Distance(array[i].position, transform.position);
                    if (minDistance > childDistance)
                    {
                        minDistance = childDistance;
                        nearPosition = array[i].position;
                    }
                }
            }
            return nearPosition;
        }
    }
}



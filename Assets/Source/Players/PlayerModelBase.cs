using System;
using Caveman.Setting;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Players
{
    public class PlayerModelBase : MonoBehaviour
    {
        public Action<Vector2> Death;
        public Action<Player> Respawn; 
        public Func<WeaponType, ObjectPool> ChangedWeapons; 

        //todo внимательно посмотреть 
        public Player player;
        
        protected Vector2 delta;
        protected Animator animator;
        protected Vector2 target;
        protected Random r;

        private bool inMotion;
        private float timeCurrentThrow;
        private PlayerPool playersPool;
        private ObjectPool weaponsPool;
        private WeaponType weaponType;
        private PlayerModelBase[] players;
                        
        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            //Invoke("ThrowStoneOnTimer", Settings.TimeThrowStone);
        }
        
        public void Init(Player player, Vector2 start, Random random, PlayerPool pool)
        {
            name = player.name;
            transform.GetChild(0).GetComponent<TextMesh>().text = name;
            this.player = player;
            playersPool = pool;
            // todo при сервере подписки на добавление удаление игроков
            players = pool.Players;
            r = random;
            transform.position = start;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
           // if (Time.time < 1) return;
            var weapon = other.gameObject.GetComponent<WeaponModelBase>();
            if (weapon == null) return;
            if (weapon.owner == null)
            {
                switch (weapon.Type)
                {
                    case WeaponType.Stone:
                        Pickup(other.gameObject.GetComponent<StoneModel>());
                        break;
                    case WeaponType.Skull:
                        Pickup(other.gameObject.GetComponent<SkullModel>());
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

        private void Pickup(WeaponModelBase weaponModel)
        {
            if (player.Weapons > Settings.MaxCountWeapons) return;
            if (weaponsPool == null || weaponModel.Type != weaponType)
            {
                player.Weapons = 0;
                weaponsPool = ChangedWeapons(weaponModel.Type);
                weaponType = weaponModel.Type;
            }
            player.Weapons++;
            animator.SetTrigger(Settings.AnimPickup);
            weaponModel.Take();
        }

        private void Throw(Vector2 aim)
        {
            weaponsPool.New().GetComponent<WeaponModelBase>().SetMotion(player, transform.position, aim);
            player.Weapons--;
        }

        protected void ThrowStoneOnTimer()
        {
            timeCurrentThrow = player.countRespawnThrow * Settings.TimeThrowStone - Time.timeSinceLevelLoad;
            if (timeCurrentThrow-- >= 0) return;
            player.countRespawnThrow++;
            if (player.Weapons > 0)
            {
                animator.SetTrigger(Settings.AnimThrowF);
                //todo возможно, ждать конца интервала анимации по карутине и потом кидать камень, вместо проставления в аниматоре
                Throw(FindClosestPlayer);
            }
            timeCurrentThrow = Settings.TimeThrowStone;
            //Invoke("ThrowStoneOnTimer", Settings.TimeThrowStone);
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

        private Vector2 FindClosestPlayer
        {
            get
            {
                float minDistance = 0;
                var nearPosition = Vector2.zero;

                for (var i = 0; i < players.Length; i++)
                {
                    if (!players[i].gameObject.activeSelf || players[i] == this) continue;
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
                return nearPosition;
            }
        }
    }
}



using System;
using System.Collections;
using Caveman.Bonuses;
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
        private PlayerPool playersPool;
        private ObjectPool weaponsPool;
        private WeaponType weaponType;
        private BonusBase bonusType;
        private PlayerModelBase[] players;

        public float Speed { get; set; }
                        
        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            StartCoroutine(ThrowOnTimer());
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
            Speed = Settings.SpeedPlayer;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return; // todo подумать. 
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

        private IEnumerator ThrowOnTimer()
        {
            if (player.Weapons > 0)
            {
                animator.SetTrigger(Settings.AnimThrowF);
                //todo ждать конца интервала анимации по карутине
                Throw(FindClosestPlayer);
            }
            yield return new WaitForSeconds(Settings.TimeThrowStone);
            StartCoroutine(ThrowOnTimer());
        }   

        //todo переписать
        public bool InMotion
        {
            protected get
            {
                if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition &&
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
            if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition)
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
                var minDistance = Settings.BoundaryEndMap*Settings.BoundaryEndMap;
                var nearPosition = Vector2.zero;

                for (var i = 0; i < players.Length; i++)
                {
                    if (!players[i].gameObject.activeSelf || players[i] == this) continue;
                    var childDistance = Vector2.SqrMagnitude(players[i].transform.position - transform.position);
                    if (minDistance > childDistance)
                    {
                        minDistance = childDistance;
                        nearPosition = players[i].transform.position;
                    }
                }
                return nearPosition;
            }
        }
    }
}



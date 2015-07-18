using System;
using System.Collections;
using System.Collections.Generic;
using Caveman.Bonuses;
using Caveman.Network;
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
        public Func<WeaponType, ObjectPool<WeaponModelBase>> ChangedWeapons;

        public Player player;
        public BonusBase bonusType;
        public string Id;
        public float Speed { get; set; }
        public SpriteRenderer spriteRenderer;

        protected Vector2 delta;
        protected Animator animator;
        protected Vector2 target;
        protected Random r;
        protected ServerConnection serverConnection;
        protected bool multiplayer;
        protected WeaponType weaponType;
        protected List<PlayerModelBase> players;

        private bool inMotion;
        protected PlayerPool poolPlayers;
        private ObjectPool<WeaponModelBase> poolWeapons;

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Init(Player player, Vector2 start, Random random, PlayerPool pool, ServerConnection serverConnection)
        {
            this.serverConnection = serverConnection;
            if (serverConnection != null) multiplayer = true;
            name = player.name;
            transform.GetChild(0).GetComponent<TextMesh>().text = name;
            this.player = player;
            poolPlayers = pool;
            r = random;
            transform.position = start;
            Speed = Settings.SpeedPlayer;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void PickupBonus(BonusBase bonus)
        {
            bonus.Effect(this);
        }

        public virtual bool PickupWeapon(WeaponModelBase weaponModel)
        {
            if (player.Weapons > Settings.MaxCountWeapons) return false;
            if (poolWeapons == null || weaponModel.type != weaponType)
            {
                player.Weapons = 0;
                poolWeapons = ChangedWeapons(weaponModel.type);
                weaponType = weaponModel.type;
            }
            player.Weapons += 1;
            animator.SetTrigger(Settings.AnimPickup);
            weaponModel.Take();
            return true;
        }

        public virtual void Throw(Vector2 aim)
        {
            poolWeapons.New().GetComponent<WeaponModelBase>().SetMotion(player, transform.position, aim);
            player.Weapons--;
        }

        public virtual IEnumerator Respawn()
        {
            yield return new WaitForSeconds(Settings.TimeRespawnPlayer);
            var pl = poolPlayers.New(Id).GetComponent<PlayerModelBase>();
            pl.transform.position = new Vector2(r.Next(Settings.WidthMap), r.Next(Settings.HeightMap));
        }

        //todo переписать
        public bool InMotion
        {
            protected get
            {
                if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition &&
                    Vector2.SqrMagnitude((Vector2)transform.position - target) < UnityExtensions.ThresholdPosition)
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

        public void SetPool(PlayerPool pool)
        {
            poolPlayers = pool;
        }
    }
}


